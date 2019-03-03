// <copyright file="Program.cs" company="Supun De Silva">
// All rights reserved (C) Supun De Silva 2019
// </copyright>

namespace FileAnalyzer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
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

                var services = new ServiceCollection()
                                    .AddLogging(logBuilder =>
                                    {
                                        logBuilder.AddConsole();
                                        logBuilder.SetMinimumLevel(LogLevel.Debug);
                                    })
                                    .AddTransient<ICSVFileReader, CSVFileReader>()
                                    .AddTransient<IFolderScanner, FolderScanner>()
                                    .AddTransient<ICSVProcessor, CSVProcessor>()
                                    .AddSingleton(configuration)
                                    .BuildServiceProvider();

                var logger = services.GetService<ILogger<Program>>();

                logger.LogInformation("CSV reader starting");
                IFolderScanner fs = services.GetService<IFolderScanner>();
                List<FoundCSVItem> filesFound = fs.FindCSVFiles();

                List<Task> processingTasks = new List<Task>();
                foreach (FoundCSVItem item in filesFound)
                {
                    ICSVProcessor fp = services.GetService<ICSVProcessor>();
                    fp.Init(item);

                    // processingTasks.Add(Task.Run(async () => await Task.Run(() => { fp.Process(); })));
                    processingTasks.Add(fp.ProcessAsync());
                }

                Task.WaitAll(processingTasks.ToArray());

                Console.ReadKey();
                services.Dispose();
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
