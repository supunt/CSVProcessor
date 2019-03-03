// <copyright file="CSVFile.cs" company="Supun De Silva">
// All rights reserved (C) Supun De Silva 2019
// </copyright>

namespace FileAnalyzer.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class CSVFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CSVFile" /> class.
        /// </summary>
        /// <param name="dateColIndex">Index of the date col.</param>
        /// <param name="valueColIndex">Index of the value col.</param>
        public CSVFile(int dateColIndex, int valueColIndex)
        {
            this.DateColumnIndex = dateColIndex;
            this.ValueColumnIndex = valueColIndex;
            this.FileEntries = new List<CSVFileEntry>();
            this.Errors = new List<string>();
        }

        /// <summary>
        /// Gets the index of the date column.
        /// </summary>
        /// <value>
        /// The index of the date column.
        /// </value>
        public int DateColumnIndex { get; private set; }

        /// <summary>
        /// Gets the index of the date column.
        /// </summary>
        /// <value>
        /// The index of the date column.
        /// </value>
        public int ValueColumnIndex { get; private set; }

        /// <summary>
        /// Gets or sets the file entries.
        /// </summary>
        /// <value>
        /// The file entries.
        /// </value>
        public List<CSVFileEntry> FileEntries { get; set; }

        /// <summary>
        /// Gets or sets the file entries.
        /// </summary>
        /// <value>
        /// The file entries.
        /// </value>
        public List<string> Errors { get; set; }

        /// <summary>
        /// Froms the CSV string array.
        /// </summary>
        /// <param name="csvSplits">The CSV splits.</param>
        /// <exception cref="Exception">Invalid CSV string
        /// or
        /// Invalid Date string
        /// or
        /// Invalid Value string</exception>
        public void BuildFromCSVStringArray(List<string[]> csvSplits)
        {
            int lineNumber = 1;
            foreach (string[] lineSplit in csvSplits)
            {
                try
                {
                    CSVFileEntry entry = new CSVFileEntry();
                    entry.FromCSVStringArray(lineSplit, this.DateColumnIndex, this.ValueColumnIndex);
                    this.FileEntries.Add(entry);
                }
                catch (Exception ex)
                {
                    this.Errors.Add($"Error while processing file entry {lineNumber}. [Error : '{ex.Message}'");
                }

                lineNumber++;
            }
        }
    }
}
