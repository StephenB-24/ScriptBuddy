/* Author:      Stephen Behnke
 * 
 * Description: This class handles all entity framework / database interaction
 *              related to the "Action" class.
 */

using ScriptBuddy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ScriptBuddy.BL.CodeGen;
using ScriptBuddy.Logger;

namespace ScriptBuddy.BL
{
    /// <summary>
    /// Implementation of all related code to the Action class for the BusinessLayer.
    /// </summary>
    public partial class BusinessLayer : IBusinessLayer
    {
        /// <summary>
        /// Gets a script's sequence (action list) by the id of the script sequence.
        /// </summary>
        /// <param name="scriptId">The id of the script sequence to get.</param>
        /// <returns>A script sequence list.</returns>
        public List<Models.Action> GetAllActions(int scriptId)
        {
            using (ScriptBuddyDBContext context = new ScriptBuddyDBContext())
            {
                try
                {
                    List<Models.Action> actions = context.Actions.Where(i => i.ScriptId == scriptId).ToList();
                    List<Models.Action> filledActions = null;

                    if (actions != null)
                    {
                        filledActions = new List<Models.Action>();

                        foreach (Models.Action action in actions)
                        {
                            filledActions.Add(GetAction(action.Id));
                        }
                    }

                    return filledActions;
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the action by the actionId.
        /// </summary>
        /// <param name="actionId">The action id of the action being requested.</param>
        /// <returns>The action that matches the specified actionId, or null otherwise.</returns>
        public Models.Action GetAction(int actionId)
        {
            using (ScriptBuddyDBContext context = new ScriptBuddyDBContext())
            {
                try
                {
                    Models.Action action = context.Actions.Where(i => i.Id == actionId).FirstOrDefault();

                    return action;
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// In the case that the action id is not known, the action can be retrieved based on
        /// the script id and the action's position within the script.
        /// </summary>
        /// <param name="scriptId">The id of the script the action is contained within.</param>
        /// <param name="actionPosition">The position of the action in the script sequence.</param>
        /// <returns>The action requested.</returns>
        public Models.Action GetAction(int scriptId, int actionPosition)
        {
            try
            {
                List<Models.Action> actions = GetAllActions(scriptId);

                if (actions != null)
                {
                    foreach (Models.Action action in actions)
                    {
                        if (action.ActionPosition == actionPosition)
                        {
                            return GetAction(action.Id);
                        }
                    }
                }

                return null;    // An action within this script id does not exist at this position.
            }
            catch (Exception e)
            {
                ErrorLogger.LogError(e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Inserts the provided action into the database, as well all the properties of the class into other required tables.
        /// </summary>
        /// <param name="action">The action object to insert.</param>
        /// <returns>True if the action is successfully inserted, false otherwise.</returns>
        public bool InsertAction(Models.Action action)
        {
            try
            {
                using (ScriptBuddyDBContext context = new ScriptBuddyDBContext())
                {
                    List<Models.Action> matchingActions = context.Actions.Where(i => i.ScriptId == action.ScriptId
                        && i.ActionPosition == action.ActionPosition).ToList();

                    if (matchingActions != null && matchingActions.Count != 0)
                    {
                        throw new Exception("An action with this script id already exists in this position.");
                    }

                    // Doing this just to check that the script actually exists (key constraint check)
                    Script script = GetScript(action.ScriptId);
                    if (script == null)
                    {
                        throw new Exception("A script with this Id does not exist. " +
                            "This script must be created before an action can be paired with it.");
                    }

                    int nextId = 1;
                    try
                    {
                        nextId = nextId = context.Actions.OrderBy(i => i.Id).LastOrDefault().Id + 1;
                    }
                    catch (Exception)
                    {
                        nextId = 1;
                    }
                    action.Id = nextId;
                    action.Property.ActionId = nextId;

                    context.Actions.Add(action);
                    bool insertActionResult = context.SaveChanges() > 0;

                    return insertActionResult;
                }
            }
            catch (Exception e)
            {
                ErrorLogger.LogError(e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Deletes all actions related to the given script id.
        /// </summary>
        /// <param name="scriptId">The script id which every action related to will be deleted.</param>
        /// <returns>True if successfully deleted, false otherwise.</returns>
        public bool DeleteAllActions(int scriptId)
        {
            try
            {
                using (ScriptBuddyDBContext context = new ScriptBuddyDBContext())
                {
                    List<Models.Action> actions = context.Actions.Where(i => i.ScriptId == scriptId).ToList();

                    if (actions != null)
                    {
                        foreach (Models.Action action in actions)
                        {
                            DeleteAction(action.Id);
                        }
                    }

                    return true;   // There were no actions related to this script id, all done.
                }
            }
            catch (Exception e)
            {
                ErrorLogger.LogError(e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Deletes a specific action by the id of the action
        /// NOTE: If one would like to delete all actions related to a script, this is not the right method.
        /// </summary>
        /// <param name="actionId">The id of the action to deleted.</param>
        /// <returns>True if successfully deleted, false otherwise.</returns>
        public bool DeleteAction(int actionId)
        {
            try
            {
                using (ScriptBuddyDBContext context = new ScriptBuddyDBContext())
                {
                    Models.Action action = context.Actions.Where(i => i.Id == actionId).ToList().FirstOrDefault();

                    if (action != null)
                    {
                        context.Remove(action);
                        return context.SaveChanges() > 0;
                    }

                    return true;    // This action did not exist, we are done.
                }
            }
            catch (Exception e)
            {
                ErrorLogger.LogError(e.ToString());
                throw;
            }
        }
    }
}
