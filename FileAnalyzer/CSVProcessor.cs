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
        /// Processes the asynchronous.
        /// </summary>
        /// <returns>Task</returns>
        public async Task ProcessAsync()
        {
            string logStrStage = "reading";
            try
            {
                this.csvFileReader.ReadFile();
                this.logger.LogDebug($"\tItems read {this.csvFileReader.LinesRead.Count}\n" +
                                           $"\tItems failed { this.csvFileReader.ErrorLines.Count}");

                logStrStage = "processing";
                this.csvDataFile.BuildFromCSVStringArray(this.csvFileReader.LinesRead);
                this.logger.LogDebug($"\tItems Processed {this.csvDataFile.FileEntries.Count}\n" +
                                           $"\tItems failed { this.csvDataFile.Errors.Count}");

                logStrStage = "calculating";
                if (this.csvDataFile.FileEntries.Count > 0)
                {
                    await this.CalculateMedianAndVarience20Async(this.csvDataFile.FileEntries, this.csvFileReader.GetFilePath());
                }
                else
                {
                    this.logger.LogInformation(
                    $"-------------------------------------------\n\n" +
                    $"File          :'{this.csvFileReader.GetFilePath()} (EMPTY)'\n" +
                    $"Median        : N/A \n" +
                    $"Entrites      : 0 \n" +
                    $"Middle        : N/A \n" +
                    $"-20 %         : N/A \n" +
                    $"+20%          : N/A \n" +
                    $"Left Count    : N/A \n" +
                    $"Right Count   : N/A \n" +
                    $"-------------------------------------------\n\n");
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Error while {logStrStage} file ${this.csvFileReader.GetFilePath()}");
                this.logger.LogError($"Error {ex.Message}");
                this.logger.LogError($"StackTrace {ex.StackTrace}");
            }
        }

        /// <summary>
        /// Calculates the median and varience20 distributed.
        /// </summary>
        /// <param name="fileEntries">The file entries.</param>
        /// <param name="filePath">The file path.</param>
        /// <returns>Task</returns>
        private async Task CalculateMedianAndVarience20Async(List<CSVFileEntry> fileEntries, string filePath)
        {
            List<CSVFileEntry> fileEntriesSorted = fileEntries.OrderBy(x => x.Value).ToList();

            double median = 0.0;
            int leftMiddle = 0;
            int rightMiddle = 0;
            int middle = 0;
            if (fileEntriesSorted.Count > 0)
            {
                if (fileEntriesSorted.Count % 2 == 0)
                {
                    leftMiddle = (fileEntriesSorted.Count / 2) - 1;
                    rightMiddle = fileEntriesSorted.Count / 2;
                    median = (fileEntriesSorted[leftMiddle].Value + fileEntriesSorted[rightMiddle].Value) / 2;
                }
                else
                {
                    middle = fileEntriesSorted.Count / 2;
                    median = fileEntriesSorted[middle].Value;
                }
            }

            double medianPlus20Percent = median * 1.2;
            double medianMinus20Percent = median * 0.8;

            int leftLookup = 0;
            int rightLookup = 0;
            if (fileEntriesSorted.Count % 2 == 0)
            {
                leftLookup = leftMiddle;
                rightLookup = rightMiddle;
            }
            else
            {
                leftLookup = middle - 1;
                rightLookup = middle + 1;
            }

            // Upper bound entries
            List<CSVFileEntry> upperBoundEntries = new List<CSVFileEntry>();
            await Task.Run(() => this.FindUpperBound(
                fileEntriesSorted,
                rightLookup,
                fileEntriesSorted.Count - 1,
                medianPlus20Percent,
                upperBoundEntries));

            // Lower bound entries
            List<CSVFileEntry> lowerBoundEntries = new List<CSVFileEntry>();
            await Task.Run(() => this.FindLowerBound(
                fileEntriesSorted,
                0,
                leftLookup,
                medianMinus20Percent,
                lowerBoundEntries));

            this.logger.LogInformation(
                $"-------------------------------------------\n\n" +
                $"File          :'{filePath}'\n" +
                $"Median        : {median}\n" +
                $"Entrites      : {fileEntriesSorted.Count}\n" +
                $"Middle        : {fileEntriesSorted.Count / 2}\n" +
                $"-20 %         : {medianMinus20Percent}\n" +
                $"+20%          : {medianPlus20Percent}\n" +
                $"Left Count    : {lowerBoundEntries.Count}\n" +
                $"Right Count   : {upperBoundEntries.Count}\n" +
                $"-------------------------------------------\n\n");
        }

        /// <summary>
        /// Finds the upper bound.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="upper20Percent">The upper20 percent.</param>
        /// <param name="store">The store.</param>
        private void FindUpperBound(List<CSVFileEntry> data, int start, int end, double upper20Percent, List<CSVFileEntry> store)
        {
            if (start > end)
            {
                return;
            }

            int middle = (end + start) / 2;
            if (data[middle].Value > upper20Percent)
            {
                this.FindUpperBound(data, start, middle - 1, upper20Percent, store);
            }
            else
            {
                // +1 is to add the middle entry we just processed as well
                store.AddRange(data.GetRange(start, middle - start + 1));
                this.FindUpperBound(data, middle + 1, end, upper20Percent, store);
            }
        }

        /// <summary>
        /// Finds the lower bound.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="lower20Percent">The lower20 percent.</param>
        /// <param name="store">The store.</param>
        private void FindLowerBound(List<CSVFileEntry> data, int start, int end, double lower20Percent, List<CSVFileEntry> store)
        {
            if (start > end)
            {
                return;
            }

            int middle = (end + start) / 2;
            if (data[middle].Value < lower20Percent)
            {
                this.FindLowerBound(data, middle + 1, end, lower20Percent, store);
            }
            else
            {
                // +1 is to add the middle entry we just processed as well
                store.AddRange(data.GetRange(middle, end - middle + 1));
                this.FindLowerBound(data, start, middle - 1, lower20Percent, store);
            }
        }
    }
}
