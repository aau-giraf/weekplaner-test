﻿using System;
using WeekPlanner.Tests.UnitTests.Base;
using Xunit;
using AutoFixture;
using Moq;
using WeekPlanner.ViewModels;
using WeekPlanner.Services.Navigation;
using IO.Swagger.Model;
using System.Threading.Tasks;

namespace WeekPlanner.Tests.UnitTests.ViewModels
{
    public class ActivityViewModelTests : TestsBase
    {
        [Fact]
        public void ChangePictoCommand_NavigateToPictoSearch() {
            // Arrange
            var navServiceMock = Fixture.Freeze<Mock<INavigationService>>();
            var sut = Fixture.Create<ActivityViewModel>();

            // Act
            sut.ChangePictoCommand.Execute(null);

            // Assert
            navServiceMock.Verify(n => n.NavigateToAsync<PictogramSearchViewModel>(null));

        }

        //[Fact]
        //public async Task Popped_ChangesImageUrl()
        //{
            // TODO needs refactoring
           // // Arrange
           // var sut = Fixture.Build<ActivityViewModel>()
           //                  .With(x => x.ImageUrl, "http://before-url.com")
           //                  .Create();
           // var picto = Fixture.Build<PictogramDTO>()
           //                    .With(x => x.ImageUrl, "hey")
           //                    .Create();

           // bool propertyOnChangedIsInvoked = false;

           // // Act
           // await sut.PoppedAsync(picto);
           // sut.PropertyChanged += (sender, e) => {
           //     if (e.PropertyName == nameof(sut.ImageUrl))
           //         propertyOnChangedIsInvoked = true;
           // };

           // // Assert ImageSource changed
           //Assert.True(propertyOnChangedIsInvoked);
        //}

        [Fact]
        public void ToggleStateCommand_ChangesState() {
            // Arrange
            var sut = Fixture.Create<ActivityViewModel>();
            var state = sut.State;

            // Act
            sut.ToggleStateCommand.Execute(null);

            // Assert
            Assert.NotEqual(state, sut.State);
        }
    }
}
