using FileAnalyzer;
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
    public class FileReaderTests
    {
        private IConfiguration config;
        private ILogger<CSVFileReader> logger;

        [SetUp]
        public void Setup()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            this.config = builder.Build();
            this.logger = new Mock<ILogger<CSVFileReader>>().Object;
        }

        [Test]
        public void ReadWhenFileNameInvalid()
        {
            var mockLogger = new Mock<ILogger<CSVProcessor>>();


            CSVFileReader fileReader = new CSVFileReader(this.logger);
            fileReader.Init("");
            Assert.Throws(typeof(ArgumentException), () => fileReader.ReadFile());
        }

        [Test]
        public void ReadWhenFileNameValid()
        {
            var mockLogger = new Mock<ILogger<CSVProcessor>>();
            CSVFileReader fileReader = new CSVFileReader(this.logger);
            fileReader.Init($"C:\\temp\\files_to_read\\files_to_read\\TOU_1.csv");
            Assert.DoesNotThrow(() => fileReader.ReadFile());
            Assert.True(fileReader.LinesRead.Count == 8);
        }

        [Test]
        public void ReadHeaderOnlyFile()
        {
            var mockLogger = new Mock<ILogger<CSVProcessor>>();
            CSVFileReader fileReader = new CSVFileReader(this.logger);
            fileReader.Init($"C:\\temp\\files_to_read\\files_to_read\\TOU_4.csv");
            Assert.DoesNotThrow(() => fileReader.ReadFile());
            Assert.True(fileReader.LinesRead.Count == 0);
        }

        [Test]
        public void ReadEmptyFile()
        {
            var mockLogger = new Mock<ILogger<CSVProcessor>>();
            CSVFileReader fileReader = new CSVFileReader(this.logger);
            fileReader.Init($"C:\\temp\\files_to_read\\files_to_read\\TOU_5.csv");
            Assert.DoesNotThrow(() => fileReader.ReadFile());
            Assert.True(fileReader.LinesRead.Count == 0);
        }
    }
}
