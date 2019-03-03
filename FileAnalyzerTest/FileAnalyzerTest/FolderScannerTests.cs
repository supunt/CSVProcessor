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
            Mock<IConfiguration> mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(x => x["folderPath"]).Returns(Directory.GetCurrentDirectory() + $"\\UnitTestFiles\\");
            this.config = mockConfig.Object;

            Mock<IConfiguration> mockConfigInvalid = new Mock<IConfiguration>();
            mockConfigInvalid.Setup(x => x["folderPath"]).Returns("C:\\random");
            this.invalidConfig = mockConfigInvalid.Object;

            this.logger = new Mock<ILogger<FolderScanner>>().Object;
        }

        [Test]
        public void ReadValidFolderPath()
        {
            var mockLogger = new Mock<ILogger<CSVProcessor>>();

            FolderScanner folderScanner = new FolderScanner(this.logger, this.config);
            List<FoundCSVItem> filesFound = null;
            Assert.DoesNotThrow(() => filesFound = folderScanner.FindCSVFiles());
            Assert.True(filesFound.Count == 8);
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