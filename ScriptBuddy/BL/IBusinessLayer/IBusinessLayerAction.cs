/* Author:      Stephen Behnke
 * 
 * Description: This interface handles all entity framework / database interaction
 *              related to the "Action" class.
 */

using ScriptBuddy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptBuddy.BL
{
    /// <summary>
    /// An interface for all methods related to the Action object.
    /// </summary>
    public partial interface IBusinessLayer
    {
        /// <summary>
        /// Gets a script's sequence (action list) by the id of the script sequence.
        /// </summary>
        /// <param name="scriptId">The id of the script sequence to get.</param>
        /// <returns>A script sequence list.</returns>
        public List<Models.Action> GetAllActions(int scriptId);

        /// <summary>
        /// Gets the action by the actionId.
        /// </summary>
        /// <param name="actionId">The aciton id of the action being requested.</param>
        /// <returns>The action that matches the specified actionId, or null otherwise.</returns>
        public Models.Action GetAction(int id);

        /// <summary>
        /// In the case that the action id is not known, the action can be retrieved based on
        /// the script id and the action's position within the script.
        /// </summary>
        /// <param name="scriptId">The id of the script the action is contained within.</param>
        /// <param name="actionPosition">The position of the action in the script sequence.</param>
        /// <returns>The action requested.</returns>
        public Models.Action GetAction(int scriptId, int actionPosition);

        /// <summary>
        /// Inserts the provided action into the database, as well all the properties of the class into other required tables.
        /// </summary>
        /// <param name="action">The action object to insert.</param>
        /// <returns>True if the action is successfully inserted, false otherwise.</returns>
        public bool InsertAction(Models.Action scriptSequence);

        /// <summary>
        /// Deletes all actions related to the given script id.
        /// </summary>
        /// <param name="scriptId">The script id which every action related to will be deleted.</param>
        /// <returns>True if successfully deleted, false otherwise.</returns>
        public bool DeleteAllActions(int scriptId);

        /// <summary>
        /// Deletes a specific action by the id of the action
        /// NOTE: If one would like to delete all actions related to a script, this is not the right method.
        /// </summary>
        /// <param name="actionId">The id of the action to deleted.</param>
        /// <returns>True if successfully deleted, false otherwise.</returns>
        public bool DeleteAction(int actionId);
    }
}
