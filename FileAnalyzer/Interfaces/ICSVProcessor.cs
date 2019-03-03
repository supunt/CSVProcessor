// <copyright file="ICSVProcessor.cs" company="Supun De Silva">
// All rights reserved (C) Supun De Silva 2019
// </copyright>

namespace FileAnalyzer.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using FileAnalyzer.Models;

    /// <summary>
    /// CSVProcessor interface
    /// </summary>
    public interface ICSVProcessor
    {
        /// <summary>
        /// Processes the file.
        /// </summary>
        /// <returns>a processing task</returns>
        Task<TaskResult> ProcessAsync();

        /// <summary>
        /// Initializes the specified file path.
        /// </summary>
        /// <param name="csvPathItem">The CSV path item.</param>
        void Init(FoundCSVItem csvPathItem);
    }
}
