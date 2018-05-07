﻿using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using WeekPlanner.Services.Login;
using WeekPlanner.Services.Navigation;
using WeekPlanner.Services.Settings;
using WeekPlanner.Tests.UnitTests.Base;
using WeekPlanner.Validations;
using WeekPlanner.ViewModels;
using Xunit;

namespace WeekPlanner.Tests.UnitTests.ViewModels
{
    public class LoginTests : TestsBase
    {
        [Fact]
        public void PasswordProperty_AfterCreation_IsNotNull()
        {
            // Arrange
            var sut = Fixture.Build<LoginViewModel>()
                .OmitAutoProperties()
                .Create();
            
            // Assert
            Assert.NotNull(sut.Password);
        }
        
        [Fact]
        public void PasswordProperty_OnSet_RaisesPropertyChanged()
        {
            // Arrange
            var sut = Fixture.Create<LoginViewModel>();
            
            bool invoked = false;
            sut.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals(nameof(sut.Password)))
                    invoked = true;
            };
            
            // Act
            sut.Password = Fixture.Create<ValidatableObject<string>>();
            
            // Assert
            Assert.True(invoked);
        }
        
        [Fact]
        public void ValidatePasswordCommand_IsNotNull()
        {
            // Arrange
            var sut = Fixture.Build<LoginViewModel>()
                .OmitAutoProperties()
                .Create();
            
            // Assert
            Assert.NotNull(sut.ValidatePasswordCommand);
        }
        
        [Theory]
        [InlineData("NotEmpty", "Also Not Empty")]
        [InlineData("   .", "Not empty")]
        [InlineData("#€%€&%/%&", "#€%=/()&(&/(  34345")]
        public void UserNameAndPasswordIsValid_UserNameAndPasswordNotNullOrEmpty_True(string username, 
            string password)
        {
            // Arrange
            var sut = Fixture.Build<LoginViewModel>()
                .OmitAutoProperties()
                .Create();
            
            // Act
            sut.Username.Value = username;
            sut.Password.Value = password;
            
            // Assert
            Assert.True(sut.UserNameAndPasswordIsValid());
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void UserNameAndPasswordIsValid_UserNameOrPasswordIsNullOrEmpty_IsFalse(string password)
        {
            // Arrange
            var sut = Fixture.Build<LoginViewModel>()
                .OmitAutoProperties()
                .Create();
            
            // Act
            sut.Password.Value = password;
            
            // Assert
            Assert.False(sut.UserNameAndPasswordIsValid());
        }

        [Theory]
        [InlineData("Not Empty", "Not Empty")]
        public async void LoginCommand_ExecutedWithValidCredentials_InvokesLoginAndThen(string username, string password)
        {
            // Arrange 
            var loginServiceMock = Fixture.Freeze<Mock<ILoginService>>();
            var sut = Fixture.Build<LoginViewModel>()
                .OmitAutoProperties()
                .Create();

            sut.Username.Value = username;
            sut.Password.Value = password;

            // Act
            await Task.Run(() => sut.LoginCommand.Execute(null));

            // Assert
            loginServiceMock.Verify(ls => ls.LoginAndThenAsync(It.IsAny<UserType>(), 
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Func<Task>>()));
        }

        [Fact]
        public async void LoginCommand_ExecutedWithInvalidCredentials_DoesNothing()
        {
            // Arrange 
            var loginServiceMock = Fixture.Freeze<Mock<ILoginService>>();
            var sut = Fixture.Build<LoginViewModel>()
                .OmitAutoProperties()
                .Create();
            sut.Password.Value = "";

            // Act
            await Task.Run(() => sut.LoginCommand.Execute(null));

            // Assert
            loginServiceMock.Verify(ls => ls.LoginAndThenAsync(UserType.Guardian, 
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Func<Task>>()), Times.Never);
        }

        [Theory]
        [InlineData("Not Empty", "Not Empty")]
        public async void LoginCommand_ExecutedWithValidCredentials_EventuallyNavigatesToChooseCitizenViewModel(string username, string password)
        {
            // Arrange
            async Task LoginAndThenMock(Func<Task> onSuccess, UserType userType, string innerUsername, string innerPassword) 
                => await onSuccess.Invoke();

            var navigationServiceMock = Fixture.Freeze<Mock<INavigationService>>();
            
            Fixture.Freeze<Mock<ILoginService>>()
                .Setup(l => l.LoginAndThenAsync(UserType.Guardian, It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<Func<Task>>()))
                .Returns((Func<Func<Task>, UserType, string, string, Task>) LoginAndThenMock);
            
            var sut = Fixture.Build<LoginViewModel>()
                .OmitAutoProperties()
                .Create();

            sut.Username.Value = username;
            sut.Password.Value = password;
            
            // Act
            await Task.Run(() => sut.LoginCommand.Execute(null));
            
            // Assert
            navigationServiceMock.Verify(n => n.NavigateToAsync<ChooseCitizenViewModel>(null));
        }
    }
}