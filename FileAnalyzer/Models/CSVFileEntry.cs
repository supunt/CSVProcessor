// <copyright file="FileEntryBase.cs" company="Supun De Silva">
// All rights reserved (C) Supun De Silva 2019
// </copyright>

namespace FileAnalyzer.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// FileEntryBase
    /// </summary>
    public class CSVFileEntry
    {
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public string Date { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public double Value { get; set; }

        /// <summary>
        /// Froms the CSV string array.
        /// </summary>
        /// <param name="csvSplit">The CSV split.</param>
        /// <param name="dateColIndex">Index of the date col.</param>
        /// <param name="valueColIndex">Index of the value col.</param>
        /// <exception cref="Exception">Invalid CSV string
        /// or
        /// Invalid Date string
        /// or
        /// Invalid Value string</exception>
        public void FromCSVStringArray(string[] csvSplit, int dateColIndex, int valueColIndex)
        {
            if (csvSplit == null ||
                csvSplit.Length == 0 ||
                csvSplit.Length < dateColIndex ||
                csvSplit.Length < valueColIndex)
            {
                throw new Exception("Invalid CSV string");
            }

            if (csvSplit[dateColIndex] == null ||
                csvSplit[dateColIndex].Trim() == string.Empty)
            {
                throw new Exception("Invalid Date string");
            }

            this.Date = csvSplit[dateColIndex];

            if (csvSplit[valueColIndex] == null ||
                csvSplit[valueColIndex].Trim() == string.Empty)
            {
                throw new Exception("Invalid Value string");
            }

            this.Value = Convert.ToDouble(csvSplit[valueColIndex]);
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{this.Date} {this.Value}";
        }
    }
}
