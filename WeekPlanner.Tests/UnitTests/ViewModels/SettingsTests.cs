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
    }
}