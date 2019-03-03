// <copyright file="IFolderScanner.cs" company="Supun De Silva">
// All rights reserved (C) Supun De Silva 2019
// </copyright>

namespace FileAnalyzer.Interfaces
{
    using System.Collections.Generic;
    using FileAnalyzer.Models;

    /// <summary>
    /// FolderScanner interface
    /// </summary>
    public interface IFolderScanner
    {
        /// <summary>
        /// Scans the folders.
        /// </summary>
        /// <returns>A List of file objects</returns>
        List<FoundCSVItem> FindCSVFiles();
    }
}
