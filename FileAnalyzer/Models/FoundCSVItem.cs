// <copyright file="FoundCSVItem.cs" company="Supun De Silva">
// All rights reserved (C) Supun De Silva 2019
// </copyright>

namespace FileAnalyzer.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class FoundCSVItem
    {
        private string filePath;

        /// <summary>
        /// CSVProviderType
        /// </summary>
        public enum CSVProviderType
        {
            LP,
            TOU
        }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <value>
        /// The file path.
        /// </value>
        public string FilePath
        {
            get => this.filePath;
            set
            {
                this.filePath = value;
                if (Utils.GetFileNameFromPath(this.filePath).StartsWith("LP"))
                {
                    this.ProviderType = CSVProviderType.LP;
                }
                else
                {
                    this.ProviderType = CSVProviderType.TOU;
                }
            }
        }

        /// <summary>
        /// Gets the type of the provider.
        /// </summary>
        /// <value>
        /// The type of the provider.
        /// </value>
        public CSVProviderType ProviderType { get; private set; }
    }
}
