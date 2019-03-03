// <copyright file="Program.cs" company="Supun De Silva">
// All rights reserved (C) Supun De Silva 2019
// </copyright>

namespace FileAnalyzer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using FileAnalyzer.Extensions;
    using FileAnalyzer.Interfaces;
    using FileAnalyzer.Models;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class Program
    {
        /// <summary>
        /// Mains the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            try
            {
                var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                IConfiguration configuration = builder.Build();

                using (var services = new ServiceCollection()
                                    .AddLogging(logBuilder =>
                                    {
                                        logBuilder.AddConsole();
                                        logBuilder.AddConfiguration(configuration.GetSection("Logging"));
                                    })
                                    .AddTransient<ICSVFileReader, CSVFileReader>()
                                    .AddTransient<IFolderScanner, FolderScanner>()
                                    .AddTransient<ICSVProcessor, CSVProcessor>()
                                    .AddSingleton(configuration)
                                    .BuildServiceProvider())
                {
                    var logger = services.GetService<ILogger<Program>>();

                    logger.LogInformation("CSV reader starting");
                    IFolderScanner fs = services.GetService<IFolderScanner>();
                    List<FoundCSVItem> filesFound = fs.FindCSVFiles();

                    List<Task<TaskResult>> processingTasks = new List<Task<TaskResult>>();
                    foreach (FoundCSVItem item in filesFound)
                    {
                        ICSVProcessor fp = services.GetService<ICSVProcessor>();
                        fp.Init(item);

                        processingTasks.Add(fp.ProcessAsync());
                    }

                    using (Task<TaskResult[]> concatTask = Task.WhenAll(processingTasks.ToArray()))
                    {
                        foreach (TaskResult taskResult in concatTask.Result)
                        {
                            logger.LogDebug(taskResult.ToString());

                            logger.LogInformation(
                            $"------------------------------------------------------\n" +
                            $"Lower Bound Entries of {taskResult.FileName} (Count : {taskResult.LowerBoundsValues.Count})\n" +
                            $"------------------------------------------------------\n" +
                            $"{taskResult.LowerBoundsValues.ToPrintableString(taskResult.FileName, taskResult.Median.ToString())}");

                            logger.LogInformation(
                            $"------------------------------------------------------\n" +
                            $"Upper Bound Entries of {taskResult.FileName} (Count : {taskResult.UpperBoundsValues.Count})\n" +
                            $"------------------------------------------------------\n" +
                            $"{taskResult.UpperBoundsValues.ToPrintableString(taskResult.FileName, taskResult.Median.ToString())}");
                        }
                    }
                }

                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Uncaught Exception occured : {ex.Message}");
                Console.WriteLine($"Stack Trace : {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Uncaught Exception occured : {ex.InnerException.Message}");
                    Console.WriteLine($"Stack Trace : {ex.InnerException.StackTrace}");
                }

                Console.ReadKey();
            }
        }
    }
}
