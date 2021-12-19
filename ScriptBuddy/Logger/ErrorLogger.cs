/* Author:      Stephen Behnke
 * 
 * Description: This class handles logging errors to a local file.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptBuddy.Logger
{
    public static class ErrorLogger
    {
        /// <summary>
        /// Logs the specified string to a locally specified file name.
        /// Defaults to "log.txt"
        /// </summary>
        /// <param name="error">Required: the error string that is to be logged.</param>
        /// <param name="fileName">Optional: the name of the log file to use. Defaults to "log.txt".</param>
        public static void LogError(string error, string fileName = "log.txt")
        {
            try
            {
                StreamWriter sw = new StreamWriter(fileName, append: true);
                sw.WriteLine(DateTime.Now + " Error Message: " + error);
                sw.Close();
            }
            catch
            {
                // This means we have both had an error within the program AND an error while logging it.
                // At this point we really have nothing to do.
            }
        }
    }
}
