/* Author:      Stephen Behnke
 * Co-author:   Hailey Vadnais
 * 
 * Description: This interface handles all entity framework / database interaction
 *              related to the "Script" class.
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
    /// An interface for all methods related to the Script class.
    /// </summary>
    public partial interface IBusinessLayer
    {
        /// <summary>
        /// Gets all the scripts from the database.
        /// </summary>
        /// <param name="userId">The userId for the scripts.</param>
        /// <returns>A list of all of the scripts.</returns>
        public List<Script> GetAllScripts(int userId);

        /// <summary>
        /// Gets all the scripts from the database that are community scripts.
        /// Optionally, one can specify to refine their results with a query or community tag.
        /// </summary>
        /// <param name="query">The string to search for within the script</param>
        /// <param name="tag">The tag type that the script should be.</param>
        /// <returns>A list of community scripts that match the specified parameters.</returns>
        public List<Script> GetAllCommunityScripts(string query = "", CommunityTagsEnum tag = CommunityTagsEnum.Tagless);

        /// <summary>
        ///  Gets a specific script by the provided user id and script name.
        /// </summary>
        /// <param name="userId">User id of the account to look in.</param>
        /// <param name="scriptName">Name of the script to find within this user's account.</param>
        /// <returns>The script that matches the provided userId and scriptName.</returns>
        public Script GetScript(int userId, string scriptName);

        /// <summary>
        /// Gets a specific script by the provided script id
        /// </summary>
        /// <param name="scriptId">The script id of the script requested.</param>
        /// <returns>A script that matches the script id.</returns>
        public Script GetScript(int scriptId);

        /// <summary>
        /// Inserts a script into the database.
        /// </summary>
        /// <param name="script">The instance of Script to be inserted.</param>
        /// <returns>The script if it was successfully inserted,
        /// otherwise null.</returns>
        public bool InsertScript(Script script);

        /// <summary>
        /// Deletes a given script and all the underlying dependencies that related objects have with the script.
        /// </summary>
        /// <param name="scriptId">The scriptId of the script to delete.</param>
        /// <returns>True if the script deletion was successful, false otherwise.</returns>
        public bool DeleteScript(int scriptId);

        /// <summary>
        /// Deletes ALL scripts that have the provided userId.
        /// </summary>
        /// <param name="userId">The userId of the account to delete all scripts from.</param>
        /// <returns>True if the scripts were all deleted successfully, false otherwise.</returns>
        public bool DeleteAllScripts(int userId, bool deleteCommunityScripts = true);
    }
}
