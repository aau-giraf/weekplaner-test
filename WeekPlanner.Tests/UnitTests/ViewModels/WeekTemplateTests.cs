using System;

using Xamarin.Forms;
using WeekPlanner.ViewModels.Base;
using WeekPlanner.ViewModels;
using Xunit;
using AutoFixture;
using IO.Swagger.Api;
using IO.Swagger.Model;
using Moq;
using WeekPlanner.Services.Login;
using System.Collections.Generic;
using WeekPlanner.Services.Navigation;

namespace WeekPlanner.Tests.UnitTests.ViewModels
{
    public class WeekTemplateTests : Base.TestsBase
    {
        [Fact]
        public void WeekTemplate_OnSet_RaisesPropertyChanged() {
            //Arrange
            var sut = Fixture.Create<WeekTemplateViewModel>();
            var weekTemplateDTO = Fixture.Create<WeekNameDTO>();
            bool invoked = false;
            sut.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals(sut.WeekNameDtos))
                {
                    invoked = true;
                }
            };

            //Act
            sut.WeekNameDtos = new List<WeekNameDTO>();

            //Assert
            Assert.True(invoked);
        }

        [Fact]
        public void WeekTemplate_OnClick_InvokesNavigationPop() {
            //Arrange
            var sut = Fixture.Create<WeekTemplateViewModel>();
            var navService = Fixture.Freeze<Mock<INavigationService>>();
            var weekTemplateDTO = Fixture.Create<WeekNameDTO>();

            //Act
            sut.ChooseTemplateCommand.Execute(weekTemplateDTO);

            //Assert
            navService.Verify(x => x.NavigateToAsync<WeekPlannerViewModel>(weekTemplateDTO), Times.Once);
        }


    }
}

