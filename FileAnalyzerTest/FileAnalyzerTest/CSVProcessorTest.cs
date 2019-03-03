using FileAnalyzer;
using FileAnalyzer.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Tests
{
    public class CSVProcessorTest
    {

        private IConfiguration config;

        [SetUp]
        public void Setup()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            this.config = builder.Build();
        }

        [Test]
        public async Task Test1()
        {
            var mockLogger = new Mock<ILogger<CSVProcessor>>();

            var mockCSVFileReader = new Mock<ICSVFileReader>();
            mockCSVFileReader.Setup(x => x.LinesRead).Returns(
                new List<string[]>()
                {
                });

            mockCSVFileReader.Setup(x => x.GetFilePath()).Returns(
                "MockFile");


            CSVProcessor processor = new CSVProcessor(mockLogger.Object, mockCSVFileReader.Object, this.config);
            var result = await processor.ProcessAsync();
        }
    }
}