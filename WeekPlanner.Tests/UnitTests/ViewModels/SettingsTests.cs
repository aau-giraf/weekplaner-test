using AutoFixture;
using IO.Swagger.Model;
using WeekPlanner.Tests.UnitTests.Base;
using WeekPlanner.ViewModels;
using Xunit;

namespace WeekPlanner.Tests.UnitTests.ViewModels
{
    public class SettingsTests : TestsBase
    {
        [Fact]
        public void NumberOfDaysShownAtOnce_OnSet_RaisesPropertyChanged()
        {
            // Arrange
            var sut = Fixture.Create<SettingsViewModel>();

            bool invoked = false;
            sut.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals(nameof(sut.NumberOfShownDaysAtOnce)))
                    invoked = true;
            };

            // Act
            sut.NumberOfShownDaysAtOnce = 4;

            // Assert
            Assert.True(invoked);
        }

        [Fact]
        public void NumberOfDaysShownAtOnce_OnSet_IsUpperBoundOn7()
        {
            // Arrange
            var sut = Fixture.Create<SettingsViewModel>();

            // Act
            sut.NumberOfShownDaysAtOnce = 8;

            // Assert
            Assert.Equal(7, sut.NumberOfShownDaysAtOnce);
        }

        [Fact]
        public void NumberOfDaysShownAtOnce_OnSet_IsLowerBoundOn1()
        {
            // Arrange
            var sut = Fixture.Create<SettingsViewModel>();

            // Act
            sut.NumberOfShownDaysAtOnce = 0;

            // Assert
            Assert.Equal(1, sut.NumberOfShownDaysAtOnce);
        }


        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        [InlineData(3, 3)]
        [InlineData(5, 5)]
        [InlineData(6, 6)]
        [InlineData(7, 7)]
        public void NumberOfDaysShownAtOnce_OnSet_NumberIsCorrect(int x,int y)
        {
            // Arrange
            var sut = Fixture.Create<SettingsViewModel>();

            // Act
            sut.NumberOfShownDaysAtOnce = x;

            // Assert
            Assert.Equal(y, sut.NumberOfShownDaysAtOnce);
        }

        [Fact]
        public void ActivitiesShown_OnSet_IsLowerBoundOn1()
        {
            // Arrange
            var sut = Fixture.Create<SettingsViewModel>();

            // Act
            sut.ActivitiesShown = "0";

            // Assert
            Assert.Equal("1", sut.ActivitiesShown);
        }

        [Fact]
        public void ActivitiesShown_OnSet_IsUpperBoundOnAlle()
        {
            // Arrange
            var sut = Fixture.Create<SettingsViewModel>();

            // Act
            sut.ActivitiesShown = "Alle";

            // Assert
            Assert.Equal("30", sut.ActivitiesShown);
        }


        [Fact]
        public void ActivitiesShown_OnSet_IsUpperBoundOnNull()
        {
            // Arrange
            var sut = Fixture.Create<SettingsViewModel>();

            // Act
            sut.ActivitiesShown = null;

            // Assert
            Assert.Equal("30", sut.ActivitiesShown);
        }

        [Theory]
        [InlineData("1","1")]
        [InlineData("2", "2")]
        [InlineData("3", "3")]
        [InlineData("5", "5")]
        [InlineData("10", "10")]
        [InlineData("20", "20")]
        public void ActivitiesShown_OnSet_NumberIsCorrect(string x,string y)
        {
            // Arrange
            var sut = Fixture.Create<SettingsViewModel>();

            // Act
            sut.ActivitiesShown = x;

            // Assert
            Assert.Equal(y, sut.ActivitiesShown);
        }


        [Fact]
        public void activitiesShown_OnSet_RaisesPropertyChanged()
        {
            // Arrange
            var sut = Fixture.Create<SettingsViewModel>();

            bool invoked = false;
            sut.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals(nameof(sut.ActivitiesShown)))
                    invoked = true;
            };

            // Act
            sut.ActivitiesShown = "1";

            // Assert
            Assert.True(invoked);
        }

    }
}