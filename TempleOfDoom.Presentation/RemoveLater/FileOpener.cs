using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempleOfDoom.Presentation.RemoveLater
{
    public static class FileOpener
    {
        /// <summary>
        /// Opens the specified file in the default application.
        /// </summary>
        /// <param name="filePath">The full path to the file to open.</param>
        public static void OpenFile(string filePath)
        {
            try
            {
                Console.WriteLine($"Attempting to open file: {filePath}");
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to open the file: {ex.Message}");
            }
        }
    }
}
