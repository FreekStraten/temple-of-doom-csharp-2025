using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace TempleOfDoom.BusinessLogic
{
    public static class CodeExtractor
    {
        /// <summary>
        /// Extracts code from the specified directories and consolidates it into a single text file.
        /// </summary>
        /// <param name="projectRootPath">The root path of the project.</param>
        /// <param name="outputFilePath">The output file path where the consolidated code will be saved.</param>
        /// <param name="includedDirectories">An array of directories to include in the extraction.</param>
        public static void ExtractCodeToTxt(string projectRootPath, string outputFilePath, string[] includedDirectories)
        {
            try
            {
                // Ensure the output directory exists
                string outputDirectory = Path.GetDirectoryName(outputFilePath);
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                    Console.WriteLine($"Created output directory: {outputDirectory}");
                }

                // Initialize or clear the output file and write the header
                using (StreamWriter writer = new StreamWriter(outputFilePath, false))
                {
                    writer.WriteLine("// Consolidated Code Extracted on " + DateTime.Now);
                    writer.WriteLine();
                }

                // Define directories to exclude
                var excludedDirectories = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                {
                    "bin",
                    "obj",
                    ".git",
                    ".vs",
                    "Properties",
                    "Resources" // Add other directories as needed
                };

                // Retrieve all .cs files from the specified included directories
                var csFiles = includedDirectories.SelectMany(dir => Directory.GetFiles(Path.Combine(projectRootPath, dir), "*.cs", SearchOption.AllDirectories))
                    .Where(file => !excludedDirectories.Any(exDir =>
                        Path.GetFullPath(file)
                            .Split(Path.DirectorySeparatorChar)
                            .Contains(exDir, StringComparer.OrdinalIgnoreCase)));

                if (!csFiles.Any())
                {
                    Console.WriteLine("No .cs files found in the specified directories.");
                    return;
                }

                Console.WriteLine($"Found {csFiles.Count()} .cs files. Processing...");

                foreach (var file in csFiles)
                {
                    try
                    {
                        // Read all lines from the current file
                        var lines = File.ReadAllLines(file);

                        // Filter out 'using' directives
                        var filteredLines = lines.Where(line => !Regex.IsMatch(line.Trim(), @"^using\s+")).ToList();

                        // Optionally, remove empty lines for cleaner output
                        filteredLines = filteredLines.Where(line => !string.IsNullOrWhiteSpace(line)).ToList();

                        // Prepare content to append
                        var header = $"// File: {Path.GetRelativePath(projectRootPath, file)}";
                        var content = string.Join(Environment.NewLine, filteredLines);

                        // Append to the output file
                        using (StreamWriter writer = new StreamWriter(outputFilePath, true))
                        {
                            writer.WriteLine(header);
                            writer.WriteLine(content);
                            writer.WriteLine(); // Add a newline for separation
                        }

                        Console.WriteLine($"Processed: {Path.GetRelativePath(projectRootPath, file)}");
                    }
                    catch (Exception exFile)
                    {
                        Console.WriteLine($"Error processing file {file}: {exFile.Message}");
                    }
                }

                Console.WriteLine($"Extraction complete. Check the '{outputFilePath}' file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during extraction: {ex.Message}");
            }
        }
    }
}
