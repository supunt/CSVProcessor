using FileAnalyzer;
using FileAnalyzer.Interfaces;
using FileAnalyzer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FileAnalyzerTest
{
    public class CSVProcessorTest
    {

        private IConfiguration config;
        private ICSVFileReader legitimateFileReader;
        private ICSVFileReader emptyFileReader;
        private ICSVFileReader headerOnlyFileReader;
        private string folderPath;

        [SetUp]
        public void Setup()
        {
            this.folderPath = Directory.GetCurrentDirectory() + $"\\UnitTestFiles\\";
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var mockFrLogger = new Mock<ILogger<CSVFileReader>>();

            this.legitimateFileReader = new CSVFileReader(mockFrLogger.Object);
            this.emptyFileReader = new CSVFileReader(mockFrLogger.Object);
            this.headerOnlyFileReader = new CSVFileReader(mockFrLogger.Object);

            this.config = builder.Build();
        }

        [Test]
        public async Task ProcessHeaderOnlyFile()
        {
            var mockLogger = new Mock<ILogger<CSVProcessor>>();

            CSVProcessor processor = new CSVProcessor(mockLogger.Object, this.emptyFileReader, this.config);

            processor.Init(new FoundCSVItem()
            {
                FilePath = (this.folderPath + "TOU_4.csv")
            });
            TaskResult result = null;
            Assert.DoesNotThrowAsync(async () => { result = await processor.ProcessAsync(); });
            Assert.True(result.LowerBoundsValues.Count == 0);
            Assert.True(result.UpperBoundsValues.Count == 0);
            Assert.True(result.Median == 0.0);
        }

        [Test]
        public async Task ProcessEmptyFile()
        {
            var mockLogger = new Mock<ILogger<CSVProcessor>>();

            CSVProcessor processor = new CSVProcessor(mockLogger.Object, this.emptyFileReader, this.config);

            processor.Init(new FoundCSVItem()
            {
                FilePath = (this.folderPath + "TOU_5.csv")
            });
            TaskResult result = null;
            Assert.DoesNotThrowAsync(async () => { result = await processor.ProcessAsync(); });
            Assert.True(result.LowerBoundsValues.Count == 0);
            Assert.True(result.UpperBoundsValues.Count == 0);
            Assert.True(result.Median == 0.0);
        }

        [Test]
        public async Task ProcessKnownResultFile1()
        {
            var mockLogger = new Mock<ILogger<CSVProcessor>>();

            CSVProcessor processor = new CSVProcessor(mockLogger.Object, this.emptyFileReader, this.config);
            processor.Init(new FoundCSVItem()
            {
                FilePath = (this.folderPath + "TOU_1.csv")
            });
            TaskResult result = null;
            Assert.DoesNotThrowAsync(async () => { result = await processor.ProcessAsync(); });
            Assert.True(result.MedianLowerBound == 2);
            Assert.True(result.MedianUpperBound == 3);
            Assert.True(result.LowerBoundsValues.Count == 2);
            Assert.True(result.UpperBoundsValues.Count == 2);
            Assert.True(result.Median == 2.5);
        }

        [Test]
        public async Task ProcessKnownResultFile2AllZeros()
        {
            var mockLogger = new Mock<ILogger<CSVProcessor>>();

            CSVProcessor processor = new CSVProcessor(mockLogger.Object, this.emptyFileReader, this.config);
            processor.Init(new FoundCSVItem()
            {
                FilePath = (this.folderPath + "LP_AllZeros.csv")
            });
            TaskResult result = null;
            Assert.DoesNotThrowAsync(async () => { result = await processor.ProcessAsync(); });
            Assert.True(result.MedianLowerBound == 0);
            Assert.True(result.MedianUpperBound == 0);
            Assert.True(result.LowerBoundsValues.Count == 256);
            Assert.True(result.UpperBoundsValues.Count == 256);
            Assert.True(result.Median == 0);
        }

        [Test]
        public async Task ProcessKnownResultFile3()
        {
            var mockLogger = new Mock<ILogger<CSVProcessor>>();

            CSVProcessor processor = new CSVProcessor(mockLogger.Object, this.emptyFileReader, this.config);
            processor.Init(new FoundCSVItem()
            {
                FilePath = (this.folderPath + "LP_BIG.csv")
            });
            TaskResult result = null;
            Assert.DoesNotThrowAsync(async () => { result = await processor.ProcessAsync(); });
            Assert.True(result.MedianLowerBound == 1.512);
            Assert.True(result.MedianUpperBound == 2.268);
            Assert.True(result.LowerBoundsValues.Count == 42);
            Assert.True(result.UpperBoundsValues.Count == 59);
            Assert.True(result.Median == 1.89);
        }
    }
}