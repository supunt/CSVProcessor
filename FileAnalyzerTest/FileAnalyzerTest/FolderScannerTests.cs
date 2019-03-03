using FileAnalyzer;
using FileAnalyzer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileAnalyzerTest
{
    public class FolderScannerTests
    {
        private IConfiguration config;
        private IConfiguration invalidConfig;
        private ILogger<FolderScanner> logger;

        [SetUp]
        public void Setup()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings_test.json", optional: true, reloadOnChange: true);

            this.config = builder.Build();

            builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings_test_invalid.json", optional: true, reloadOnChange: true);

            this.invalidConfig = builder.Build();
            this.logger = new Mock<ILogger<FolderScanner>>().Object;
        }

        [Test]
        public void ReadValidFolderPath()
        {
            var mockLogger = new Mock<ILogger<CSVProcessor>>();

            FolderScanner folderScanner = new FolderScanner(this.logger, this.config);
            List<FoundCSVItem> filesFound = null;
            Assert.DoesNotThrow(() => filesFound = folderScanner.FindCSVFiles());
            Assert.True(filesFound.Count == 5);
        }

        [Test]
        public void ReadInvalidFolderPath()
        {
            var mockLogger = new Mock<ILogger<CSVProcessor>>();

            FolderScanner folderScanner = new FolderScanner(this.logger, this.invalidConfig);
            List<FoundCSVItem> filesFound = null;
            Assert.DoesNotThrow(() => filesFound = folderScanner.FindCSVFiles());
            Assert.True(filesFound.Count == 0);
        }
    }
}