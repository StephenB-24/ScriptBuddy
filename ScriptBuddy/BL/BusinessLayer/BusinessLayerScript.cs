/* Author:      Stephen Behnke
 * Co-author:   Hailey Vadnais
 * 
 * Description: This class handles all entity framework / database interaction
 *              related to the "Script" object.
 */

using ScriptBuddy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ScriptBuddy.BL.CodeGen;
using Microsoft.EntityFrameworkCore;
using ScriptBuddy.Logger;

namespace ScriptBuddy.BL
{
    /// <summary>
    /// Implementation of all related code to the Scripts class for the BusinessLayer.
    /// </summary>
    public partial class BusinessLayer : IBusinessLayer
    {
        /// <summary>
        /// Gets all the scripts from the database.
        /// </summary>
        /// <param name="userId">The userId for the scripts.</param>
        /// <returns>A list of all of the scripts.</returns>
        public List<Script> GetAllScripts(int userId)
        {
            using (ScriptBuddyDBContext context = new ScriptBuddyDBContext())
            {
                List<Script> scripts = new List<Script>();

                try
                {
                    // Get the script from the Script table.
                    List<Script> unfilledScripts = context.Scripts.Where(i => i.UserId == userId).ToList();

                    foreach (Script s in unfilledScripts)
                    {
                        scripts.Add(GetScript(s.Id));
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e.ToString());
                    throw;
                }

                return scripts;
            }
        }

        /// <summary>
        /// Gets all the scripts from the database that are community scripts.
        /// Optionally, one can specify to refine their results with a query or community tag.
        /// </summary>
        /// <param name="query">The string to search for within the script</param>
        /// <param name="tag">The tag type that the script should be.</param>
        /// <returns>A list of community scripts that match the specified parameters.</returns>
        public List<Script> GetAllCommunityScripts(string query = "", CommunityTagsEnum tag = CommunityTagsEnum.Tagless)
        {
            using (ScriptBuddyDBContext context = new ScriptBuddyDBContext())
            {
                List<Script> scripts = new List<Script>();

                try
                {
                    List<Script> unfilledScripts;

                    if (tag == CommunityTagsEnum.Tagless)
                    {
                        unfilledScripts = context.Scripts.Where(i => i.Name.Contains(query)
                            && i.Accessibility == true).ToList();
                    }
                    else
                    {
                        unfilledScripts = context.Scripts.Where(i => i.Name.Contains(query)
                            && i.CommunityTagId == (int)tag && i.Accessibility == true).ToList();
                    }

                    if (unfilledScripts != null)
                    {
                        foreach (Script s in unfilledScripts)
                        {
                            scripts.Add(GetScript(s.Id));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e.ToString());
                    throw;
                }

                return scripts;
            }
        }

        /// <summary>
        ///  Gets a specific script by the provided user id and script name.
        /// </summary>
        /// <param name="userId">User id of the account to look in.</param>
        /// <param name="scriptName">Name of the script to find within this user's account.</param>
        /// <returns>The script that matches the provided userId and scriptName.</returns>
        public Script GetScript(int userId, string scriptName)
        {
            using (ScriptBuddyDBContext context = new ScriptBuddyDBContext())
            {
                Script script = null;

                try
                {
                    // Get the script from the Script table.
                    script = context.Scripts.Where(i => i.UserId == userId && i.Name.Equals(scriptName))
                        .Include(script => script.Actions)
                        .ThenInclude(i => i.KeyPressProperties)
                        .Include(script => script.Actions)
                        .ThenInclude(i => i.KeyListenerProperties)
                        .Include(script => script.Actions)
                        .ThenInclude(i => i.MouseClickProperties)
                        .Include(script => script.Actions)
                        .ThenInclude(i => i.MouseMoveProperties)
                        .Include(script => script.Actions)
                        .ThenInclude(i => i.PauseProperties)
                        .Include(script => script.Actions)
                        .ThenInclude(i => i.MediaKeyProperties)
                        .Include(script => script.Actions)
                        .ThenInclude(i => i.CharacterSequenceProperties)
                        .Include(script => script.Actions)
                        .ThenInclude(i => i.HotStringProperties)
                        .FirstOrDefault();

                    //Include data from the connected tables(complete each script object)
                    if (script != null)
                    {
                        foreach (Models.Action a in script.Actions)
                        {
                            a.Id = 0;
                            a.ScriptId = 0;

                            if (a.KeyListenerProperties.Count != 0)
                            {
                                a.Property = a.KeyListenerProperties.ToList().FirstOrDefault();
                                a.KeyListenerProperties.ToList()[0].Id = 0;
                                a.KeyListenerProperties.ToList()[0].ActionId = 0;
                            }
                            else if (a.KeyPressProperties.Count != 0)
                            {
                                a.Property = a.KeyPressProperties.ToList().FirstOrDefault();
                                a.KeyPressProperties.ToList()[0].Id = 0;
                                a.KeyPressProperties.ToList()[0].ActionId = 0;
                            }
                            else if (a.MouseMoveProperties.Count != 0)
                            {
                                a.Property = a.MouseMoveProperties.ToList().FirstOrDefault();
                                a.MouseMoveProperties.ToList()[0].Id = 0;
                                a.MouseMoveProperties.ToList()[0].ActionId = 0;
                            }
                            else if (a.MouseClickProperties.Count != 0)
                            {
                                a.Property = a.MouseClickProperties.ToList().FirstOrDefault();
                                a.MouseClickProperties.ToList()[0].Id = 0;
                                a.MouseClickProperties.ToList()[0].ActionId = 0;
                            }
                            else if (a.PauseProperties.Count != 0)
                            {
                                a.Property = a.PauseProperties.ToList().FirstOrDefault();
                                a.PauseProperties.ToList()[0].Id = 0;
                                a.PauseProperties.ToList()[0].ActionId = 0;
                            }
                            else if (a.CharacterSequenceProperties.Count != 0)
                            {
                                a.Property = a.CharacterSequenceProperties.ToList().FirstOrDefault();
                                a.CharacterSequenceProperties.ToList()[0].Id = 0;
                                a.CharacterSequenceProperties.ToList()[0].ActionId = 0;
                            }
                            else if (a.MediaKeyProperties.Count != 0)
                            {
                                a.Property = a.MediaKeyProperties.ToList().FirstOrDefault();
                                a.MediaKeyProperties.ToList()[0].Id = 0;
                                a.MediaKeyProperties.ToList()[0].ActionId = 0;
                            }
                            else if (a.HotStringProperties.Count != 0)
                            {
                                a.Property = a.HotStringProperties.ToList().FirstOrDefault();
                                a.HotStringProperties.ToList()[0].Id = 0;
                                a.HotStringProperties.ToList()[0].ActionId = 0;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e.ToString());
                    throw;
                }

                return script;
            }
        }

        /// <summary>
        /// Gets a specific script by the provided script id
        /// </summary>
        /// <param name="scriptId">The script id of the script requested.</param>
        /// <returns>A script that matches the script id.</returns>
        public Script GetScript(int scriptId)
        {
            using (ScriptBuddyDBContext context = new ScriptBuddyDBContext())
            {
                Script script;

                try
                {
                    // Get the script from the Script table.
                    script = context.Scripts.Where(i => i.Id == scriptId)
                        .Include(script => script.Actions)
                        .ThenInclude(i => i.KeyPressProperties)
                        .Include(script => script.Actions)
                        .ThenInclude(i => i.KeyListenerProperties)
                        .Include(script => script.Actions)
                        .ThenInclude(i => i.MouseClickProperties)
                        .Include(script => script.Actions)
                        .ThenInclude(i => i.MouseMoveProperties)
                        .Include(script => script.Actions)
                        .ThenInclude(i => i.PauseProperties)
                        .Include(script => script.Actions)
                        .ThenInclude(i => i.MediaKeyProperties)
                        .Include(script => script.Actions)
                        .ThenInclude(i => i.CharacterSequenceProperties)
                        .Include(script => script.Actions)
                        .ThenInclude(i => i.HotStringProperties)
                        .FirstOrDefault();

                    //Include data from the connected tables(complete each script object)
                    if (script != null)
                    {
                        foreach (Models.Action a in script.Actions)
                        {
                            a.Id = 0;
                            a.ScriptId = 0;

                            if (a.KeyListenerProperties.Count != 0)
                            {
                                a.Property = a.KeyListenerProperties.ToList().FirstOrDefault();
                                a.KeyListenerProperties.ToList()[0].Id = 0;
                                a.KeyListenerProperties.ToList()[0].ActionId = 0;
                            }
                            else if (a.KeyPressProperties.Count != 0)
                            {
                                a.Property = a.KeyPressProperties.ToList().FirstOrDefault();
                                a.KeyPressProperties.ToList()[0].Id = 0;
                                a.KeyPressProperties.ToList()[0].ActionId = 0;
                            }
                            else if (a.MouseMoveProperties.Count != 0)
                            {
                                a.Property = a.MouseMoveProperties.ToList().FirstOrDefault();
                                a.MouseMoveProperties.ToList()[0].Id = 0;
                                a.MouseMoveProperties.ToList()[0].ActionId = 0;
                            }
                            else if (a.MouseClickProperties.Count != 0)
                            {
                                a.Property = a.MouseClickProperties.ToList().FirstOrDefault();
                                a.MouseClickProperties.ToList()[0].Id = 0;
                                a.MouseClickProperties.ToList()[0].ActionId = 0;
                            }
                            else if (a.PauseProperties.Count != 0)
                            {
                                a.Property = a.PauseProperties.ToList().FirstOrDefault();
                                a.PauseProperties.ToList()[0].Id = 0;
                                a.PauseProperties.ToList()[0].ActionId = 0;
                            }
                            else if (a.CharacterSequenceProperties.Count != 0)
                            {
                                a.Property = a.CharacterSequenceProperties.ToList().FirstOrDefault();
                                a.CharacterSequenceProperties.ToList()[0].Id = 0;
                                a.CharacterSequenceProperties.ToList()[0].ActionId = 0;
                            }
                            else if (a.MediaKeyProperties.Count != 0)
                            {
                                a.Property = a.MediaKeyProperties.ToList().FirstOrDefault();
                                a.MediaKeyProperties.ToList()[0].Id = 0;
                                a.MediaKeyProperties.ToList()[0].ActionId = 0;
                            }
                            else if (a.HotStringProperties.Count != 0)
                            {
                                a.Property = a.HotStringProperties.ToList().FirstOrDefault();
                                a.HotStringProperties.ToList()[0].Id = 0;
                                a.HotStringProperties.ToList()[0].ActionId = 0;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e.ToString());
                    throw;
                }

                return script;
            }
        }

        /// <summary>
        /// Inserts a script into the database.
        /// </summary>
        /// <param name="script">The instance of Script to be inserted.</param>
        /// <returns>The script if it was successfully inserted,
        /// otherwise null.</returns>
        public bool InsertScript(Script script)
        {
            try
            {
                using (ScriptBuddyDBContext context = new ScriptBuddyDBContext())
                {
                    if (context.Users.Where(i => i.Id == script.UserId).FirstOrDefault() == null)
                    {
                        throw new Exception("User does not exist with the specified id of: " + script.UserId);
                    }
                    if (context.Scripts.Where(i => i.UserId == script.UserId && i.Name == script.Name).FirstOrDefault() != null)
                    {
                        throw new Exception("A script with this name alreay exists for this user.");
                    }

                    foreach (Models.Action a in script.Actions)
                    {
                        a.Id = 0;
                        if (a.ActionTypeId == (int)ActionTypeEnum.KeyListener)
                        {
                            a.KeyListenerProperties = new List<KeyListenerProperty>()
                            {
                                (KeyListenerProperty)a.Property
                            };
                        }
                        else if (a.ActionTypeId == (int)ActionTypeEnum.KeyPress)
                        {
                            a.KeyPressProperties = new List<KeyPressProperty>()
                            {
                                (KeyPressProperty)a.Property
                            };
                        }
                        else if (a.ActionTypeId == (int)ActionTypeEnum.MouseMove)
                        {
                            a.MouseMoveProperties = new List<MouseMoveProperty>()
                            {
                                (MouseMoveProperty)a.Property
                            };
                        }
                        else if (a.ActionTypeId == (int)ActionTypeEnum.MouseClick)
                        {
                            a.MouseClickProperties = new List<MouseClickProperty>()
                            {
                                (MouseClickProperty)a.Property
                            };
                        }
                        else if (a.ActionTypeId == (int)ActionTypeEnum.Pause)
                        {
                            a.PauseProperties = new List<PauseProperty>()
                            {
                                (PauseProperty)a.Property
                            };
                        }
                        else if (a.ActionTypeId == (int)ActionTypeEnum.CharacterSequence)
                        {
                            a.CharacterSequenceProperties = new List<CharacterSequenceProperty>()
                            {
                                (CharacterSequenceProperty)a.Property
                            };
                        }
                        else if (a.ActionTypeId == (int)ActionTypeEnum.MediaKey)
                        {
                            a.MediaKeyProperties = new List<MediaKeyProperty>()
                            {
                                (MediaKeyProperty)a.Property
                            };
                        }
                        else if (a.ActionTypeId == (int)ActionTypeEnum.HotString)
                        {
                            a.HotStringProperties = new List<HotStringProperty>()
                            {
                                (HotStringProperty)a.Property
                            };
                        }
                    }

                    context.Scripts.Add(script);
                    return context.SaveChanges() > 0;
                }
            }
            catch (Exception e)
            {
                ErrorLogger.LogError(e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Deletes a given script and all the underlying dependencies that related objects have with the script.
        /// </summary>
        /// <param name="scriptId">The scriptId of the script to delete.</param>
        /// <returns>True if the script deletion was successful, false otherwise.</returns>
        public bool DeleteScript(int scriptId)
        {
            try
            {
                Script script;
                using (ScriptBuddyDBContext context = new ScriptBuddyDBContext())
                {
                    script = GetScript(scriptId);
                    if (script != null)
                    {
                        if (script.Actions != null)
                        {
                            foreach (Models.Action a in script.Actions)
                            {
                                DeleteAction(a.Id); // Delete each action of the script we are deleting
                            }
                        }

                        context.Remove(script);
                        return context.SaveChanges() > 0;
                    }

                    return true; // The script was not found, all done
                }
            }
            catch (Exception e)
            {
                ErrorLogger.LogError(e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Deletes ALL scripts that have the provided userId.
        /// </summary>
        /// <param name="userId">The userId of the account to delete all scripts from.</param>
        /// <returns>True if the scripts were all deleted successfully, false otherwise.</returns>
        public bool DeleteAllScripts(int userId, bool deleteCommunityScripts = true)
        {
            try
            {
                List<Script> scripts;
                using (ScriptBuddyDBContext context = new ScriptBuddyDBContext())
                {
                    if (deleteCommunityScripts)
                    {
                        scripts = context.Scripts.Where(i => i.UserId == userId).ToList();
                    }
                    else
                    {
                        scripts = context.Scripts.Where(i => i.UserId == userId && i.Accessibility
                            == deleteCommunityScripts).ToList();
                    }

                    if (scripts != null)
                    {
                        foreach (Script script in scripts)
                        {
                            DeleteScript(script.Id);
                        }

                        return context.SaveChanges() > 0;
                    }

                    return true;    // There were no scripts, we are done.
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
