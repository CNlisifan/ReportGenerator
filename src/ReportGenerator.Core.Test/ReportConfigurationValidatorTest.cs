﻿using System.IO;
using Moq;
using Palmmedia.ReportGenerator.Core.Logging;
using Palmmedia.ReportGenerator.Core.Reporting;
using Palmmedia.ReportGeneratorTest;
using Xunit;

namespace Palmmedia.ReportGenerator.Core.Test
{
    [Collection("FileManager")]
    public class ReportConfigurationValidatorTest
    {
        private static readonly string ReportPath = Path.Combine(FileManager.GetCSharpReportDirectory(), "OpenCover.xml");

        private Mock<IReportBuilderFactory> reportBuilderFactoryMock = new Mock<IReportBuilderFactory>();

        public ReportConfigurationValidatorTest()
        {
            this.reportBuilderFactoryMock
                .Setup(r => r.GetAvailableReportTypes())
                .Returns(new[] { "Latex", "Xml", "Html", "Something" });
        }

        [Fact]
        public void Validate_AllPropertiesApplied_ValidationPasses()
        {
            var configuration = new ReportConfiguration(
                new[] { ReportPath },
                "C:\\temp",
                null,
                new[] { "Latex", "Xml", "Html" },
                new[] { "+Test", "-Test" },
                new[] { "+Test2", "-Test2" },
                new[] { "+Test3", "-Test3" },
                VerbosityLevel.Info.ToString(),
                null);

            var sut = new ReportConfigurationValidator(this.reportBuilderFactoryMock.Object);

            Assert.True(sut.Validate(configuration));
        }

        [Fact]
        public void Validate_NoReport_ValidationFails()
        {
            var configuration = new ReportConfiguration(
                new string[] { },
                "C:\\temp",
                null,
                new[] { "Latex" },
                new[] { "+Test", "-Test" },
                new[] { "+Test2", "-Test2" },
                new string[] { },
                VerbosityLevel.Info.ToString(),
                null);

            var sut = new ReportConfigurationValidator(this.reportBuilderFactoryMock.Object);

            Assert.False(sut.Validate(configuration));
        }

        [Fact]
        public void Validate_NonExistingReport_ValidationFails()
        {
            var configuration = new ReportConfiguration(
                new[] { "123.xml" },
                "C:\\temp",
                null,
                new[] { "Latex" },
                new[] { "+Test", "-Test" },
                new[] { "+Test2", "-Test2" },
                new string[] { },
                VerbosityLevel.Info.ToString(),
                null);

            var sut = new ReportConfigurationValidator(this.reportBuilderFactoryMock.Object);

            Assert.False(sut.Validate(configuration));
        }

        [Fact]
        public void Validate_NoTargetDirectory_ValidationFails()
        {
            var configuration = new ReportConfiguration(
                new[] { ReportPath },
                string.Empty,
                null,
                new[] { "Latex" },
                new[] { "+Test", "-Test" },
                new[] { "+Test2", "-Test2" },
                new string[] { },
                VerbosityLevel.Info.ToString(),
                null);

            var sut = new ReportConfigurationValidator(this.reportBuilderFactoryMock.Object);

            Assert.False(sut.Validate(configuration));
        }

        [Fact]
        public void Validate_InvalidTargetDirectory_ValidationFails()
        {
            var configuration = new ReportConfiguration(
                new[] { ReportPath },
                "C:\\temp:?$",
                null,
                new[] { "Latex" },
                new[] { "+Test", "-Test" },
                new[] { "+Test2", "-Test2" },
                new string[] { },
                VerbosityLevel.Info.ToString(),
                null);

            var sut = new ReportConfigurationValidator(this.reportBuilderFactoryMock.Object);

            Assert.False(sut.Validate(configuration));
        }

        [Fact]
        public void Validate_InvalidHistoryDirectory_ValidationFails()
        {
            var configuration = new ReportConfiguration(
                new[] { ReportPath },
                "C:\\temp",
                "C:\\temp:?$",
                new[] { "Latex" },
                new[] { "+Test", "-Test" },
                new[] { "+Test2", "-Test2" },
                new string[] { },
                VerbosityLevel.Info.ToString(),
                null);

            var sut = new ReportConfigurationValidator(this.reportBuilderFactoryMock.Object);

            Assert.False(sut.Validate(configuration));
        }

        [Fact]
        public void Validate_InvalidReportType_ValidationFails()
        {
            var configuration = new ReportConfiguration(
                new[] { ReportPath },
                "C:\\temp",
                null,
                new[] { "DoesNotExist" },
                new[] { "+Test", "-Test" },
                new[] { "+Test2", "-Test2" },
                new string[] { },
                VerbosityLevel.Info.ToString(),
                null);

            var sut = new ReportConfigurationValidator(this.reportBuilderFactoryMock.Object);

            Assert.False(sut.Validate(configuration));
        }

        [Fact]
        public void Validate_InvalidFilter_ValidationFails()
        {
            var configuration = new ReportConfiguration(
                new[] { ReportPath },
                @"C:\\temp",
                null,
                new[] { "Latex" },
                new[] { "Test" },
                new[] { "Test2" },
                new string[] { },
                VerbosityLevel.Info.ToString(),
                null);

            var sut = new ReportConfigurationValidator(this.reportBuilderFactoryMock.Object);

            Assert.False(sut.Validate(configuration));
        }
    }
}