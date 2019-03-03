// <copyright file="ICSVFileReader.cs" company="Supun De Silva">
// All rights reserved (C) Supun De Silva 2019
// </copyright>

namespace FileAnalyzer.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface ICSVFileReader
    {
        /// <summary>
        /// Gets the lines read.
        /// </summary>
        /// <value>
        /// The lines read.
        /// </value>
        List<string[]> LinesRead { get; }

        /// <summary>
        /// Gets the lines read.
        /// </summary>
        /// <value>
        /// The lines read.
        /// </value>
        List<int> ErrorLines { get; }

        /// <summary>
        /// Processes the file.
        /// </summary>
        void ReadFile();

        /// <summary>
        /// Initializes the specified file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        void Init(string filePath);

        /// <summary>
        /// Gets the file path.
        /// </summary>
        /// <returns>Current file path</returns>
        string GetFilePath();
    }
}
