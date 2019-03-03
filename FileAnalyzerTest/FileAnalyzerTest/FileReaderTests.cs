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
        private ILogger<CSVFileReader> logger;
        private string folderPath;

        [SetUp]
        public void Setup()
        {
            this.logger = new Mock<ILogger<CSVFileReader>>().Object;
            this.folderPath = Directory.GetCurrentDirectory() + $"\\UnitTestFiles\\";
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
        public void ReadValidFile()
        {
            var mockLogger = new Mock<ILogger<CSVProcessor>>();
            CSVFileReader fileReader = new CSVFileReader(this.logger);
            fileReader.Init(this.folderPath + "TOU_1.csv");
            Assert.DoesNotThrow(() => fileReader.ReadFile());
            Assert.True(fileReader.LinesRead.Count == 8);
        }

        [Test]
        public void ReadHeaderOnlyFile()
        {
            var mockLogger = new Mock<ILogger<CSVProcessor>>();
            CSVFileReader fileReader = new CSVFileReader(this.logger);
            fileReader.Init(this.folderPath + "TOU_4.csv");
            Assert.DoesNotThrow(() => fileReader.ReadFile());
            Assert.True(fileReader.LinesRead.Count == 0);
        }

        [Test]
        public void ReadEmptyFile()
        {
            var mockLogger = new Mock<ILogger<CSVProcessor>>();
            CSVFileReader fileReader = new CSVFileReader(this.logger);
            fileReader.Init(this.folderPath + "TOU_5.csv");
            Assert.DoesNotThrow(() => fileReader.ReadFile());
            Assert.True(fileReader.LinesRead.Count == 0);
        }

        [Test]
        public void ReadNonExistingFile()
        {
            var mockLogger = new Mock<ILogger<CSVProcessor>>();
            CSVFileReader fileReader = new CSVFileReader(this.logger);
            fileReader.Init(this.folderPath + "TOU_51.csv");
            Assert.Throws(typeof(FileNotFoundException), () => fileReader.ReadFile());
            Assert.True(fileReader.LinesRead.Count == 0);
        }
    }
}
