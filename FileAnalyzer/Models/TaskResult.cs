// <copyright file="TaskResult.cs" company="Supun De Silva">
// All rights reserved (C) Supun De Silva 2019
// </copyright>

namespace FileAnalyzer.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// TaskResult
    /// </summary>
    public class TaskResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskResult"/> class.
        /// </summary>
        public TaskResult()
        {
            this.LowerBoundsValues = new List<CSVFileEntry>();
            this.UpperBoundsValues = new List<CSVFileEntry>();
            this.FileName = string.Empty;
            this.Median = 0.0;
            this.MedianUpperBound = 0.0;
            this.MedianLowerBound = 0.0;
        }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public int EntryCount { get; set; }

        /// <summary>
        /// Gets or sets the median.
        /// </summary>
        /// <value>
        /// The median.
        /// </value>
        public double Median { get; set; }

        /// <summary>
        /// Gets or sets the median.
        /// </summary>
        /// <value>
        /// The median.
        /// </value>
        public double MedianUpperBound { get; set; }

        /// <summary>
        /// Gets or sets the median.
        /// </summary>
        /// <value>
        /// The median.
        /// </value>
        public double MedianLowerBound { get; set; }

        /// <summary>
        /// Gets or sets the lower bounds values.
        /// </summary>
        /// <value>
        /// The lower bounds values.
        /// </value>
        public List<CSVFileEntry> LowerBoundsValues { get; set; }

        /// <summary>
        /// Gets or sets the upper bounds values.
        /// </summary>
        /// <value>
        /// The upper bounds values.
        /// </value>
        public List<CSVFileEntry> UpperBoundsValues { get; set; }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"File : '{this.FileName}");
            builder.AppendLine($"File Entry Count : {this.EntryCount}");
            builder.AppendLine($"Median : {this.Median}");
            builder.AppendLine($"Median Lower bound : {this.MedianLowerBound}");
            builder.AppendLine($"Median Upper bound : {this.MedianUpperBound}");
            builder.AppendLine($"Median Lower bound Entry count : {this.LowerBoundsValues?.Count}");
            builder.AppendLine($"Median Upper bound Entry count : {this.UpperBoundsValues?.Count}");
            return builder.ToString();
        }
    }
}
