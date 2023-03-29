using AutoMapper;
using Demo.API.Contracts;
using Demo.API.Controllers;
using Demo.API.Entities;
using Demo.API.Profiles;
using Demo.API.Tests;
using Demos.API.Models.GamesDtos;
using Demos.API.Tests.TestData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace Demos.API.Tests
{
    public class GameControllerTests
    {
        public IMapper GetMapper()
        {
            var mappingUserProfile = new UserProfile();
            var mappingGameProfile = new GameProfile();

            var configuration = new MapperConfiguration(cfg => {
                cfg.AddProfile(mappingUserProfile);
                cfg.AddProfile(mappingGameProfile);
            });

            //configuration.AssertConfigurationIsValid();
            
            return new Mapper(configuration);
        }

        //[Fact]
        [Theory]
        [ClassData(typeof(StatusCodesTestData))]
        public async void WhenHasAValidUserInput_ShouldReturnUserGames(HttpStatusCode statusCode)
        {
            // arrange
            // Configurar los datos, objetc...
            var r = new Game()
            {
                Id = Guid.Empty,
                Name = "Game Test",
            };
            var mockGameRepo = new Mock<IGameRepository>();
            mockGameRepo.Setup(repo => repo.CreateUserGameAsync(It.IsAny<Guid>(), It.IsAny<Game>()));
            var mockUserRepo = new Mock<IUserRepository>();
            mockUserRepo.Setup(m => m.GetUserAsync(It.IsAny<Guid>())).Returns(Task.FromResult<User>(new User()));
            var mockLogger = new Mock<ILogger<GamesController>>();
            ILogger<GamesController> logger = mockLogger.Object;
            var mapper = GetMapper();
            var controller = new GamesController(mockGameRepo.Object, mockUserRepo.Object, mapper, logger);

            //controller.ModelState.AddModelError("Name", "Name is required");

            // act
            // Donde invocas lo que vas a probar
            var result = await controller.UserGamesAsync(Guid.Empty, null, null, null);

            // assert
            // Verificar el resultado de la invocacio o act
            Assert.IsAssignableFrom<ActionResult<ICollection<GameDto>>>(result);
        }
    }
}