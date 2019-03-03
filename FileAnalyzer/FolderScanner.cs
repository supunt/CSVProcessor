// <copyright file="FolderScanner.cs" company="Supun De Silva">
// All rights reserved (C) Supun De Silva 2019
// </copyright>

namespace FileAnalyzer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using FileAnalyzer.Exceptions;
    using FileAnalyzer.Extensions;
    using FileAnalyzer.Interfaces;
    using FileAnalyzer.Models;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    public class FolderScanner : IFolderScanner
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<FolderScanner> logger;

        /// <summary>
        /// The folder path
        /// </summary>
        private readonly string folderPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="FolderScanner" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="appSettings">The application settings.</param>
        public FolderScanner(ILogger<FolderScanner> logger, IConfiguration appSettings)
        {
            this.logger = logger;
            this.folderPath = appSettings["folderPath"];
        }

        /// <summary>
        /// Scans the folders.
        /// </summary>
        /// <returns>
        /// A List of file objects
        /// </returns>
        public List<FoundCSVItem> FindCSVFiles()
        {
            List<FoundCSVItem> filesFound = new List<FoundCSVItem>();
            try
            {
                this.logger.LogInformation($"Analyzing Folder '{this.folderPath}'");

                List<string> files = Directory.GetFiles(this.folderPath, "*.csv").ToList();

                foreach (string filePath in files)
                {
                    try
                    {
                        filesFound.Add(new FoundCSVItem()
                        {
                            FilePath = filePath
                        });
                    }
                    catch (FileTypeException)
                    {
                        continue;
                    }
                }

                this.logger.LogInformation($"Files found \n {files.ToPrintableString()}");
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Error while searching for available data files : {ex.Message}");
            }

            return filesFound;
        }
    }
}
