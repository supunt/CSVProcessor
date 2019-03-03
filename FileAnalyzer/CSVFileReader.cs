// <copyright file="CSVFileReader.cs" company="Supun De Silva">
// All rights reserved (C) Supun De Silva 2019
// </copyright>

namespace FileAnalyzer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using FileAnalyzer.Interfaces;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    public class CSVFileReader : ICSVFileReader
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<CSVFileReader> logger;

        /// <summary>
        /// The application settings
        /// </summary>
        private readonly IConfiguration appSettings;

        /// <summary>
        /// The file path
        /// </summary>
        private string filePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="CSVFileReader" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public CSVFileReader(ILogger<CSVFileReader> logger)
        {
            this.logger = logger;
            this.LinesRead = new List<string[]>();
            this.ErrorLines = new List<int>();
        }

        /// <summary>
        /// Gets the lines read.
        /// </summary>
        /// <value>
        /// The lines read.
        /// </value>
        public List<string[]> LinesRead { get; private set; }

        /// <summary>
        /// Gets the lines read.
        /// </summary>
        /// <value>
        /// The lines read.
        /// </value>
        public List<int> ErrorLines { get; private set; }

        /// <summary>
        /// Gets the file path.
        /// </summary>
        /// <returns>Gets the current file path</returns>
        public string GetFilePath()
        {
            return this.filePath;
        }

        /// <summary>
        /// Initializes the specified file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public void Init(string filePath)
        {
            this.filePath = filePath;
        }

        /// <summary>
        /// Processes the file.
        /// </summary>
        public void ReadFile()
        {
            int lineNum = 1;
            using (StreamReader reader = new StreamReader(this.filePath))
            {
                while (true)
                {
                    string line = reader.ReadLine();

                    if (line == null)
                    {
                        break;
                    }

                    // Ignore headers
                    if (lineNum == 1)
                    {
                        lineNum++;
                        continue;
                    }

                    try
                    {
                        this.LinesRead.Add(line.Split(','));
                    }
                    catch (Exception)
                    {
                        this.ErrorLines.Add(lineNum);
                    }

                    lineNum++;
                }
            }
        }
    }
}
