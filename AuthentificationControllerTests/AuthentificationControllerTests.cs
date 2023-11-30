using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;
using UserDataManagingService.Controllers;
using UserDataManagingService.Controllers.Requests;
using UserDataManagingService.Models;
using UserDataManagingService.Models.DTOs;
using UserDataManagingService.Models.Repositories;
using UserDataManagingService.Services;

namespace AuthentificationControllerTests
{
    public class AuthentificationControllerTests
    {
        [Fact]
        public async Task SignUpNewAcc_ShouldReturnBadRequest_IfNickNameNotAvailable()
        {
            //Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var userLoginServiceMock = new Mock<IUserLoginService>(); //nr1
            var jwtServiceMock = new Mock<IJWTService>();
            var livingPlaceRepositoryMock = new Mock<ILivingPlaceRepository>();
            var avatarRepositoryMock = new Mock<IAvatarRepository>();
            var loggerMock = new Mock<ILogger<AuthenticationController>>();

            var sut = new AuthenticationController(loggerMock.Object, userRepositoryMock.Object, userLoginServiceMock.Object, 
                jwtServiceMock.Object, livingPlaceRepositoryMock.Object, avatarRepositoryMock.Object);

            var requestAdapterMock = userLoginServiceMock.Setup(x => x.SignupNewUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((true, null)); // setupinam betkokias reiksmes ir imituojam, jog existuoja nickname - true, o sukurtas user - null

            var expectedStatusCode = 400;
            var exptectedDescription = "requested NickName is not availible";

            //Act
            var nameFromRequest = new SignUpNewUserRequest
            {                
                Username = "username",
                UserLastName = "userLastName",
                NickName = "nickName",
                Password = "password",
                PersonalCode = "personalCode",
                PhoneNr = "phoneNr",
                Email = "email"
            };

            var controllerResult = await sut.SignUpNewAcc(nameFromRequest) as ObjectResult; //paleidziam metoda

            // Assert
            Assert.NotNull(controllerResult);
            Assert.Equal(expectedStatusCode, controllerResult.StatusCode);
            Assert.Equal(exptectedDescription, controllerResult.Value);
        }

        [Fact]
        public async Task SignUpNewAcc_ShouldReturnOkAndUser()
        {
            //Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var userLoginServiceMock = new Mock<IUserLoginService>(); //nr1
            var jwtServiceMock = new Mock<IJWTService>();
            var livingPlaceRepositoryMock = new Mock<ILivingPlaceRepository>();
            var avatarRepositoryMock = new Mock<IAvatarRepository>();
            var loggerMock = new Mock<ILogger<AuthenticationController>>();

            var sut = new AuthenticationController(loggerMock.Object, userRepositoryMock.Object, userLoginServiceMock.Object,
                jwtServiceMock.Object, livingPlaceRepositoryMock.Object, avatarRepositoryMock.Object);

            var fakeUser = new User();
            var requestAdapterMock = userLoginServiceMock.Setup(x => x.SignupNewUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((false, fakeUser)); // setupinam betkokias reiksmes, nickname neuzimtas, todel sukurimas user


            //Act
            var nameFromRequest = new SignUpNewUserRequest
            {
                Username = "username",
                UserLastName = "userLastName",
                NickName = "nickName",
                Password = "password",
                PersonalCode = "personalCode",
                PhoneNr = "phoneNr",
                Email = "email"
            };

            var controllerResult = await sut.SignUpNewAcc(nameFromRequest) as ObjectResult; //paleidziam metoda

            // Assert
            Assert.NotNull(controllerResult);
            Assert.IsType<OkObjectResult>(controllerResult);
            Assert.IsType<User>(controllerResult.Value);
        }

        [Fact]
        public async Task ConfirmUserCreationaByUserIdRouteProvided_ShouldReturnsBadRequest_IfLivingPlaceIsNull()
        {
            {
                // Arrange
                var userRepositoryMock = new Mock<IUserRepository>();
                var userLoginServiceMock = new Mock<IUserLoginService>();
                var jwtServiceMock = new Mock<IJWTService>();
                var livingPlaceRepositoryMock = new Mock<ILivingPlaceRepository>();
                var avatarRepositoryMock = new Mock<IAvatarRepository>();
                var loggerMock = new Mock<ILogger<AuthenticationController>>();

                var sut = new AuthenticationController(loggerMock.Object, userRepositoryMock.Object, userLoginServiceMock.Object,
                  jwtServiceMock.Object, livingPlaceRepositoryMock.Object, avatarRepositoryMock.Object);

                userLoginServiceMock.Setup(m => m.ConvertStringToGuid(It.IsAny<string>())).Returns(Guid.NewGuid());
                livingPlaceRepositoryMock.Setup(x => x.GetLivingPlaceDataByUserID(It.IsAny<Guid>())).Returns(Task.FromResult((LivingPlace)null));

                // Act
                var controllerResult = await sut.ConfirmUserCreationaByUserIdRouteProvided("userIdAsString") as ObjectResult;

                // Assert
                Assert.Equal(400, controllerResult.StatusCode);
                Assert.Equal("nera user living place", controllerResult.Value);
            }
        }

        [Fact]
        public async Task ConfirmUserCreationaByUserIdRouteProvided_ShouldReturnsBadRequest_IfAvatarIsNull()
        {
            
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var userLoginServiceMock = new Mock<IUserLoginService>();
            var jwtServiceMock = new Mock<IJWTService>();
            var livingPlaceRepositoryMock = new Mock<ILivingPlaceRepository>();
            var avatarRepositoryMock = new Mock<IAvatarRepository>();
            var loggerMock = new Mock<ILogger<AuthenticationController>>();

            var sut = new AuthenticationController(loggerMock.Object, userRepositoryMock.Object, userLoginServiceMock.Object,
                jwtServiceMock.Object, livingPlaceRepositoryMock.Object, avatarRepositoryMock.Object);

            var fakeLlinigPlace = new LivingPlace
            {
                LivingPlace_Id = Guid.NewGuid(),
            };

            userLoginServiceMock.Setup(m => m.ConvertStringToGuid(It.IsAny<string>())).Returns(Guid.NewGuid());
            livingPlaceRepositoryMock.Setup(x => x.GetLivingPlaceDataByUserID(It.IsAny<Guid>())).Returns(Task.FromResult(fakeLlinigPlace));

            avatarRepositoryMock.Setup(x => x.GetAvatarByUserID(It.IsAny<Guid>())).Returns(Task.FromResult((Avatar)null));

            // Act
            var controllerResult = await sut.ConfirmUserCreationaByUserIdRouteProvided("userIdAsString") as ObjectResult;

            // Assert
            Assert.Equal(400, controllerResult.StatusCode);
            Assert.Equal("nera user avatar", controllerResult.Value);
            
        }

        [Fact]
        public async Task ConfirmUserCreationaByUserIdRouteProvided_ShouldReturnsOk()
        {


            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var userLoginServiceMock = new Mock<IUserLoginService>();
            var jwtServiceMock = new Mock<IJWTService>();
            var livingPlaceRepositoryMock = new Mock<ILivingPlaceRepository>();
            var avatarRepositoryMock = new Mock<IAvatarRepository>();
            var loggerMock = new Mock<ILogger<AuthenticationController>>();

            var sut = new AuthenticationController(loggerMock.Object, userRepositoryMock.Object, userLoginServiceMock.Object,
                jwtServiceMock.Object, livingPlaceRepositoryMock.Object, avatarRepositoryMock.Object);

            var fakeUserGuidId = Guid.NewGuid();
            var fakeLlinigPlace = new LivingPlace
            {
                LivingPlace_Id = Guid.NewGuid(),
            };
            var fakeAvatar = new Avatar
            {
                Avatar_Id = Guid.NewGuid(),
            };

            userLoginServiceMock.Setup(m => m.ConvertStringToGuid(It.IsAny<string>())).Returns(fakeUserGuidId);
            livingPlaceRepositoryMock.Setup(x => x.GetLivingPlaceDataByUserID(It.IsAny<Guid>())).Returns(Task.FromResult(fakeLlinigPlace));
            avatarRepositoryMock.Setup(x => x.GetAvatarByUserID(It.IsAny<Guid>())).Returns(Task.FromResult(fakeAvatar));

            userLoginServiceMock
                .Setup(c => c.CompleteUserCreating(It.IsAny<Guid>()))
                .ReturnsAsync((true, ""));
            
            // Act
            var controllerResult = await sut.ConfirmUserCreationaByUserIdRouteProvided("userIdAsString") as ObjectResult;

            // Assert
            Assert.NotNull(controllerResult);
            Assert.IsType<OkObjectResult>(controllerResult);
        }

        [Fact]
        public async Task ConfirmUserCreationaByUserIdRouteProvided_ShouldBadRequest_IfUserWasNotCreated()
        {


            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var userLoginServiceMock = new Mock<IUserLoginService>();
            var jwtServiceMock = new Mock<IJWTService>();
            var livingPlaceRepositoryMock = new Mock<ILivingPlaceRepository>();
            var avatarRepositoryMock = new Mock<IAvatarRepository>();
            var loggerMock = new Mock<ILogger<AuthenticationController>>();

            var sut = new AuthenticationController(loggerMock.Object, userRepositoryMock.Object, userLoginServiceMock.Object,
                jwtServiceMock.Object, livingPlaceRepositoryMock.Object, avatarRepositoryMock.Object);

            var fakeUserGuidId = Guid.NewGuid();
            var fakeLlinigPlace = new LivingPlace
            {
                LivingPlace_Id = Guid.NewGuid(),
            };
            var fakeAvatar = new Avatar
            {
                Avatar_Id = Guid.NewGuid(),
            };

            userLoginServiceMock.Setup(m => m.ConvertStringToGuid(It.IsAny<string>())).Returns(fakeUserGuidId);
            livingPlaceRepositoryMock.Setup(x => x.GetLivingPlaceDataByUserID(It.IsAny<Guid>())).Returns(Task.FromResult(fakeLlinigPlace));
            avatarRepositoryMock.Setup(x => x.GetAvatarByUserID(It.IsAny<Guid>())).Returns(Task.FromResult(fakeAvatar));

            userLoginServiceMock
                .Setup(c => c.CompleteUserCreating(It.IsAny<Guid>()))
                .ReturnsAsync((false, ""));

            // Act
            var controllerResult = await sut.ConfirmUserCreationaByUserIdRouteProvided("userIdAsString") as ObjectResult;

            // Assert
            Assert.NotNull(controllerResult);            
            Assert.Equal(400, controllerResult.StatusCode);
        }

        [Fact]
        public async Task UserLogin_ShouldReturnNotFound_IfIsUserExistFalse()
        {
            //Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var userLoginServiceMock = new Mock<IUserLoginService>(); //nr1
            var jwtServiceMock = new Mock<IJWTService>();
            var livingPlaceRepositoryMock = new Mock<ILivingPlaceRepository>();
            var avatarRepositoryMock = new Mock<IAvatarRepository>();
            var loggerMock = new Mock<ILogger<AuthenticationController>>();

            var sut = new AuthenticationController(loggerMock.Object, userRepositoryMock.Object, userLoginServiceMock.Object,
                jwtServiceMock.Object, livingPlaceRepositoryMock.Object, avatarRepositoryMock.Object);

            var notExistedUserStatusDTO = new UserStatusDTO(false);
            
            var fakeUser = new User();
            userLoginServiceMock.Setup(x => x.UserLogin(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(notExistedUserStatusDTO); 

            //Act
            var nameAndPasswordFromRequest = new LoginRequest
            {
                NickName = "anyNick",
                Password = "anyPassword"
            };

            var controllerResult = await sut.UserLogin(nameAndPasswordFromRequest) as ObjectResult; //paleidziam metoda

            // Assert
            Assert.NotNull(controllerResult);
            Assert.Equal(404, controllerResult.StatusCode);
            Assert.Equal("user nerastas arba blogas slaptazodis", controllerResult.Value);
        }

        [Fact]
        public async Task UserLogin_ShouldReturnOk()
        {
            //Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var userLoginServiceMock = new Mock<IUserLoginService>(); //nr1
            var jwtServiceMock = new Mock<IJWTService>();
            var livingPlaceRepositoryMock = new Mock<ILivingPlaceRepository>();
            var avatarRepositoryMock = new Mock<IAvatarRepository>();
            var loggerMock = new Mock<ILogger<AuthenticationController>>();

            var sut = new AuthenticationController(loggerMock.Object, userRepositoryMock.Object, userLoginServiceMock.Object,
                jwtServiceMock.Object, livingPlaceRepositoryMock.Object, avatarRepositoryMock.Object);

            var existedUserStatusDTO = new UserStatusDTO(true, Role.DefaultUser);

            var fakeUser = new User();
            userLoginServiceMock.Setup(x => x.UserLogin(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(existedUserStatusDTO); 

            //Act
            var nameAndPasswordFromRequest = new LoginRequest
            {
                NickName = "anyNick",
                Password = "anyPassword"
            };

            var controllerResult = await sut.UserLogin(nameAndPasswordFromRequest) as ObjectResult; //paleidziam metoda

            // Assert
            Assert.NotNull(controllerResult);
            //Assert.Equal(200, controllerResult.StatusCode);
            Assert.IsType<OkObjectResult>(controllerResult);
        }

        [Fact]
        public async Task ShowAllUserData_ShouldReturnOk_IfCurrentUserAreAdmin()
        {
            //Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var userLoginServiceMock = new Mock<IUserLoginService>(); //nr1
            var jwtServiceMock = new Mock<IJWTService>();
            var livingPlaceRepositoryMock = new Mock<ILivingPlaceRepository>();
            var avatarRepositoryMock = new Mock<IAvatarRepository>();
            var loggerMock = new Mock<ILogger<AuthenticationController>>();

            var sut = new AuthenticationController(loggerMock.Object, userRepositoryMock.Object, userLoginServiceMock.Object,
                jwtServiceMock.Object, livingPlaceRepositoryMock.Object, avatarRepositoryMock.Object);

            var fakeUserGuidId = Guid.NewGuid();

            var fakeTargetUser = new User
            {
                NickName = "targetNickname"
            };

            var fakeCurrentUser = new User
            {
                NickName = "admin"
            };

            var fakeUserDto = new UserDTO
            {
                UserId = fakeUserGuidId
            };

            userRepositoryMock.Setup(s => s.ConvertStringToGuid(It.IsAny<string>())).Returns(fakeUserGuidId); //var userguid
            userRepositoryMock.Setup(g => g.GetFullUserById(It.IsAny<Guid>())).ReturnsAsync(fakeTargetUser); //var getfulluser

            // hardcore :D user vardas per HttpContext.User
            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, "admin") 
                    }, "mock"))
                }
            };

            /*            sut.ControllerContext = new ControllerContext //skipinam nicko  grazinima is claim
                        {
                            HttpContext = new DefaultHttpContext { User = null }
                        };*/

            //            var isUserAuthorisedCorrectly = (fakeTargetUser.NickName == fakeCurrentUser.NickName || fakeCurrentUser.NickName == "admin");

            userLoginServiceMock.Setup(d => d.CreateUserDTO(fakeTargetUser)).ReturnsAsync(fakeUserDto);

            //Act

            var controllerResult = await sut.ShowAllUserData("userIdAsString") as ObjectResult;

            // Assert
            Assert.NotNull(controllerResult);
            Assert.IsType<OkObjectResult>(controllerResult); 
            Assert.Equal(fakeUserDto, controllerResult.Value);
            //Assert.Equal(404, controllerResult.StatusCode); //unauthorised
        }

        [Fact]
        public async Task DeleteUser_ShouldReturnBadRequest_IfCurrentUserAreNotAdmin()
        {
            //Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var userLoginServiceMock = new Mock<IUserLoginService>(); //nr1
            var jwtServiceMock = new Mock<IJWTService>();
            var livingPlaceRepositoryMock = new Mock<ILivingPlaceRepository>();
            var avatarRepositoryMock = new Mock<IAvatarRepository>();
            var loggerMock = new Mock<ILogger<AuthenticationController>>();

            var sut = new AuthenticationController(loggerMock.Object, userRepositoryMock.Object, userLoginServiceMock.Object,
                jwtServiceMock.Object, livingPlaceRepositoryMock.Object, avatarRepositoryMock.Object);

            var fakeUserGuidId = Guid.NewGuid();

            var fakeTargetUser = new User
            {
                NickName = "targetNickname"
            };

            var fakeCurrentUser = new User
            {
                NickName = "admin",
                Role = Role.DefaultUser
            };

            var fakeUserDto = new UserDTO
            {
                UserId = fakeUserGuidId
            };

            userRepositoryMock.Setup(s => s.ConvertStringToGuid(It.IsAny<string>())).Returns(fakeUserGuidId); //var userguid
            userRepositoryMock.Setup(s => s.ConvertStringToGuid(It.IsAny<string>())).Returns(fakeUserGuidId); //var userguid
            userRepositoryMock.Setup(g => g.GetUserRoleById(It.IsAny<Guid>())).ReturnsAsync(fakeCurrentUser.Role.ToString);

            var fakeUserToRemoveRequest = new UserToRemoveRequest
            {
                UserId = "userIdAsString"
            };

            //Act

            var controllerResult = await sut.DeleteUser("userIdAsString", fakeUserToRemoveRequest) as ObjectResult;

            // Assert
            Assert.NotNull(controllerResult);
            Assert.Equal(400, controllerResult.StatusCode);
            Assert.Equal("neturite teises trinti useriu", controllerResult.Value);

            //Assert.IsType<OkObjectResult>(controllerResult);
            //Assert.Equal(fakeUserDto, controllerResult.Value);
            //Assert.Equal(404, controllerResult.StatusCode); //unauthorised
        }

        [Fact]
        public async Task DeleteUser_ShouldReturnBadRequest_IfTargetUserIdIsWrong()
        {
            //Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var userLoginServiceMock = new Mock<IUserLoginService>(); //nr1
            var jwtServiceMock = new Mock<IJWTService>();
            var livingPlaceRepositoryMock = new Mock<ILivingPlaceRepository>();
            var avatarRepositoryMock = new Mock<IAvatarRepository>();
            var loggerMock = new Mock<ILogger<AuthenticationController>>();

            var sut = new AuthenticationController(loggerMock.Object, userRepositoryMock.Object, userLoginServiceMock.Object,
                jwtServiceMock.Object, livingPlaceRepositoryMock.Object, avatarRepositoryMock.Object);

            var fakeUserGuidId = Guid.NewGuid();

            var fakeTargetUser = new User
            {
                NickName = "targetNickname"
            };

            var fakeCurrentUser = new User
            {
                NickName = "admin",
                Role = Role.Admin
            };

            var fakeUserDto = new UserDTO
            {
                UserId = fakeUserGuidId
            };

            userRepositoryMock.Setup(s => s.ConvertStringToGuid(It.IsAny<string>())).Returns(fakeUserGuidId); //var userguid
            userRepositoryMock.Setup(s => s.ConvertStringToGuid(It.IsAny<string>())).Returns(fakeUserGuidId); //var userguid
            userRepositoryMock.Setup(g => g.GetUserRoleById(It.IsAny<Guid>())).ReturnsAsync(fakeCurrentUser.Role.ToString); 
            userRepositoryMock.Setup(g => g.GetFullUserById(It.IsAny<Guid>())).ReturnsAsync((User)null); //var getfulluser
            
            //            userLoginServiceMock.Setup(d => d.CreateUserDTO(fakeTargetUser)).ReturnsAsync(fakeUserDto);

            var fakeUserToRemoveRequest = new UserToRemoveRequest
            {
                UserId = "userIdAsString"
            };

            //Act

            var controllerResult = await sut.DeleteUser("userIdAsString", fakeUserToRemoveRequest) as ObjectResult;

            // Assert
            Assert.NotNull(controllerResult);
            Assert.Equal(400, controllerResult.StatusCode);
            Assert.Equal("nerastas user su tokiu id", controllerResult.Value);

            //Assert.IsType<OkObjectResult>(controllerResult);
            //Assert.Equal(fakeUserDto, controllerResult.Value);
            //Assert.Equal(404, controllerResult.StatusCode); //unauthorised
        }

        [Fact]
        public async Task DeleteUser_ShouldReturnOk()
        {
            //Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var userLoginServiceMock = new Mock<IUserLoginService>(); //nr1
            var jwtServiceMock = new Mock<IJWTService>();
            var livingPlaceRepositoryMock = new Mock<ILivingPlaceRepository>();
            var avatarRepositoryMock = new Mock<IAvatarRepository>();
            var loggerMock = new Mock<ILogger<AuthenticationController>>();

            var sut = new AuthenticationController(loggerMock.Object, userRepositoryMock.Object, userLoginServiceMock.Object,
                jwtServiceMock.Object, livingPlaceRepositoryMock.Object, avatarRepositoryMock.Object);

            var fakeUserGuidId = Guid.NewGuid();

            var fakeTargetUser = new User
            {
                NickName = "targetNickname"
            };

            var fakeCurrentUser = new User
            {
                NickName = "admin",
                Role = Role.Admin
            };

            var fakeUserDto = new UserDTO
            {
                UserId = fakeUserGuidId
            };

            userRepositoryMock.Setup(s => s.ConvertStringToGuid(It.IsAny<string>())).Returns(fakeUserGuidId); //var userguid
            userRepositoryMock.Setup(s => s.ConvertStringToGuid(It.IsAny<string>())).Returns(fakeUserGuidId); //var userguid
            userRepositoryMock.Setup(g => g.GetUserRoleById(It.IsAny<Guid>())).ReturnsAsync(fakeCurrentUser.Role.ToString);
            userRepositoryMock.Setup(g => g.GetFullUserById(It.IsAny<Guid>())).ReturnsAsync(fakeTargetUser); //var getfulluser

            userRepositoryMock.Setup(g => g.DeleteUserAsync(It.IsAny<Guid>())).ReturnsAsync(true); //var getfulluser
            var fakeUserToRemoveRequest = new UserToRemoveRequest
            {
                UserId = "userIdAsString"
            };

            //Act

            var controllerResult = await sut.DeleteUser("userIdAsString", fakeUserToRemoveRequest) as ObjectResult;

            // Assert
            Assert.NotNull(controllerResult);
            Assert.Equal(200, controllerResult.StatusCode);
            Assert.Equal("user pasalintas", controllerResult.Value);

            //Assert.IsType<OkObjectResult>(controllerResult);
            //Assert.Equal(fakeUserDto, controllerResult.Value);
            //Assert.Equal(404, controllerResult.StatusCode); //unauthorised
        }
    }    
}