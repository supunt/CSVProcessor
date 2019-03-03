// <copyright file="CSVProcessor.cs" company="Supun De Silva">
// All rights reserved (C) Supun De Silva 2019
// </copyright>

namespace FileAnalyzer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using FileAnalyzer.Extensions;
    using FileAnalyzer.Interfaces;
    using FileAnalyzer.Models;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using static FileAnalyzer.Models.CSVFile;
    using static FileAnalyzer.Models.FoundCSVItem;

    public class CSVProcessor : ICSVProcessor
    {
        /// <summary>
        /// The configuration
        /// </summary>
        private readonly IConfiguration config;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<CSVProcessor> logger;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ICSVFileReader csvFileReader;

        /// <summary>
        /// The provider
        /// </summary>
        private CSVProviderType provider;

        /// <summary>
        /// The CSV data file
        /// </summary>
        private CSVFile csvDataFile;

        /// <summary>
        /// Initializes a new instance of the <see cref="CSVProcessor" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="fileReader">The file reader.</param>
        public CSVProcessor(ILogger<CSVProcessor> logger, ICSVFileReader fileReader, IConfiguration config)
        {
            this.logger = logger;
            this.csvFileReader = fileReader;
            this.config = config;
        }

        /// <summary>
        /// Initializes the specified file path.
        /// </summary>
        /// <param name="csvPathItem">The CSV path item.</param>
        public void Init(FoundCSVItem csvPathItem)
        {
            this.csvFileReader.Init(csvPathItem.FilePath);
            this.provider = csvPathItem.ProviderType;
            this.csvDataFile = new CSVFile(
                this.config.GetValue<int>($"Providers:{this.provider.ToString()}:dateColIndex"),
                this.config.GetValue<int>($"Providers:{this.provider.ToString()}:valueColIndex"));
        }

        /// <summary>
        /// Processes the file.
        /// </summary>
        public void Process()
        {
            try
            {
                this.csvFileReader.ReadFile();
                this.logger.LogDebug($"\tItems read {this.csvFileReader.LinesRead.Count}\n" +
                                           $"\tItems failed { this.csvFileReader.ErrorLines.Count}");
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Error while reading file ${this.csvFileReader.GetFilePath()}");
                this.logger.LogError($"Error {ex.Message}");
                this.logger.LogError($"StackTrace {ex.StackTrace}");
            }

            try
            {
                this.csvDataFile.BuildFromCSVStringArray(this.csvFileReader.LinesRead);
                this.logger.LogDebug($"\tItems Processed {this.csvDataFile.FileEntries.Count}\n" +
                                           $"\tItems failed { this.csvDataFile.Errors.Count}");

                this.CalculateMedianAndVarience20(this.csvDataFile.FileEntries, this.csvFileReader.GetFilePath());
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Error while preocessing file ${this.csvFileReader.GetFilePath()}");
                this.logger.LogError($"Error {ex.Message}");
                this.logger.LogError($"StackTrace {ex.StackTrace}");
            }
        }

        /// <summary>
        /// Calculates the median and varience20 distributed.
        /// </summary>
        /// <param name="fileEntries">The file entries.</param>
        /// <param name="filePath">The file path.</param>
        private void CalculateMedianAndVarience20Distributed(List<CSVFileEntry> fileEntries, string filePath)
        {
            this.CalculateMedianAndVarience20(fileEntries, filePath);
        }

        /// <summary>
        /// Calculates the median and varience20.
        /// </summary>
        /// <param name="fileEntries">The file entries.</param>
        /// <param name="filePath">The file path.</param>
        private void CalculateMedianAndVarience20(List<CSVFileEntry> fileEntries, string filePath)
        {
            List<CSVFileEntry> fileEntriesSorted = fileEntries.OrderBy(x => x.Value).ToList();

            double median = 0.0;
            int leftMiddle = 0;
            if (fileEntriesSorted.Count > 0)
            {
                if (fileEntriesSorted.Count % 2 == 0)
                {
                    leftMiddle = (fileEntriesSorted.Count / 2) - 1;
                    median = (fileEntriesSorted[leftMiddle].Value + fileEntriesSorted[leftMiddle + 1].Value) / 2;
                }
                else
                {
                    leftMiddle = fileEntriesSorted.Count / 2;
                    median = fileEntriesSorted[leftMiddle].Value;
                }
            }

            double medianPlus20Percent = median * 1.2;
            double medianMinus20Percent = median * 0.8;

            this.logger.LogInformation($"Median of '{filePath}': {median} ({leftMiddle + 1} of {fileEntriesSorted.Count})");

            List<CSVFileEntry> store = new List<CSVFileEntry>();
            this.FindUpperBound(
                fileEntriesSorted,
                leftMiddle + 1,
                fileEntriesSorted.Count - 1,
                medianPlus20Percent,
                store);
        }

        private void FindUpperBound(List<CSVFileEntry> data, int start, int end, double upper20Percent, List<CSVFileEntry> store)
        {
            if (start > end)
            {
                return;
            }

            int middle = start + ((end - start) / 2);
            if (data[middle].Value > upper20Percent)
            {
                this.FindUpperBound(data, start, middle, upper20Percent, store);
            }
            else
            {
                store.AddRange(data.GetRange(start, middle - start));
                this.FindUpperBound(data, middle, end, upper20Percent, store);
            }
        }
    }
}
