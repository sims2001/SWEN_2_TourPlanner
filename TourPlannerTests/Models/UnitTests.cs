using TourPlanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using Moq;
using TourPlanner.DbContexts;
using TourPlanner.DTOs;
using TourPlanner.Services;

namespace TourPlanner.Models.Tests {
    [TestFixture]
    public class UnitTests {
        private OnlineRoute route;

        private Mock<IServiceProvider> serviceProviderMock = new();
        private Mock<IConfiguration> configurationMock = new();
        Tour testTour = new();
        TourDTO testTourDTO = new();
        TourLog testLog = new();
        private LogDTO testLogDTO = new();

        private Guid tourGuid = new();
        private Guid logGuid = new();

        [OneTimeSetUp]
        public async Task SetUp() {
            configurationMock = new Mock<IConfiguration>();
            var mapConfigurationSectionMock = new Mock<IConfigurationSection>();
            mapConfigurationSectionMock.Setup(x => x.Value).Returns("By1wHXsGAwuUXAsjCqHgg5yTAxAq1KfW");

            configurationMock
                .Setup(x => x.GetSection("MapQuestAPI:key"))
                .Returns(mapConfigurationSectionMock.Object);

            var connectionConfigurationSectionMock = new Mock<IConfigurationSection>();
            connectionConfigurationSectionMock.Setup(x => x.Value).Returns("Host=localhost; Database=tourplannerdb; Username=postgres; Password=admin; Port=5432");
            configurationMock
                .Setup(x => x.GetSection("ConnectionStrings:LocalPostgreSQL"))
                .Returns(connectionConfigurationSectionMock.Object);

            serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(typeof(IConfiguration)))
                .Returns(configurationMock.Object);

            
            /*
            var mockTourSet = new Mock<DbSet<TourDTO>>();
            var mockLogSet = new Mock<DbSet<LogDTO>>();

            var contextMock = new Mock<TourPlannerDbContext>();
            contextMock.Setup(m => m.Tours).Returns(mockTourSet.Object);
            contextMock.Setup(m => m.Logs).Returns(mockLogSet.Object);
            

            var factoryMock = new Mock<TourPlannerDbContextFactory>(serviceProviderMock);
            factoryMock.Setup(x => x.CreateTourPlannerDbContext())
                .Returns(() => new TourPlannerDbContext(new DbContextOptionsBuilder()
                    .UseNpgsql("Host=localhost; Database=tourplannerdb; Username=postgres; Password=admin; Port=5432").Options));
            
            serviceProviderMock
                .Setup(x => x.GetService(typeof(TourPlannerDbContextFactory)))
                .Returns(factoryMock.Object); */

            

            
            tourGuid = new Guid("55555555-4444-3333-2222-111111111111");
            logGuid = new Guid("33333333-2222-1111-4444-555555555555");

            testTour = new Tour() {
                Id = tourGuid,
                Description = "TestDescription",
                Name = "TestName",
                From = "TestFrom",
                To = "TestTo",
                TransportType = TransportType.Bicycle,
                Distance = 4f,
                PicturePath = "TestPicPath",
                Time = 432,
                Logs = new List<TourLog>()
            };

            testLog = new TourLog() {
                Id = logGuid,
                Comment = "test",
                Date = DateTime.MinValue,
                Difficulty = Difficulty.Easy,
                Rating = Popularity.Good,
                TotalTime = 450
            };

            testTourDTO = new TourDTO() {
                Id = tourGuid,
                Description = "TestDescription",
                Name = "TestName",
                From = "TestFrom",
                To = "TestTo",
                TransportType = TransportType.Bicycle,
                Distance = 4f,
                PicturePath = "TestPicPath",
                Time = 432,
                LogDTOs = new List<LogDTO>()
            };

            testLogDTO = new LogDTO() {
                Id = logGuid,
                Comment = "test",
                Date = DateTime.MinValue,
                Difficulty = Difficulty.Easy,
                Rating = Popularity.Good,
                TotalTime = 450,
                TourId = tourGuid,
                TourDTO = testTourDTO
            };
            
            route = await OnlineRoute.GetOnlineRoute("Wilfersdorf", "Mistelbach", TransportType.Fastest.ToString(), serviceProviderMock.Object);
        }

        

        [Test]
        public async Task RouteNotNullTest() {
            Assert.That(route, Is.Not.Null);
        }

        [Test]
        public async Task RouteDistanceTest() {
            Assert.AreEqual(Math.Round(route.Distance, 0), testTour.Distance);
        }

        [Test]
        public async Task RouteTimeTest() {
            Assert.AreEqual(route.Time, testTour.Time);
        }

        [Test]
        public async Task RouteImageTest() {
            Assert.That(File.Exists(route.PicPath));

            File.Delete(route.PicPath);
        }

        [Test]
        public void MapTourIdTest() {
            var t = PersonalMapper.TourFromDTO(testTourDTO);

            Assert.AreEqual(testTour.Id, t.Id);
        }

        [Test]
        public void MapTourDescriptionTest() {
            var t = PersonalMapper.TourFromDTO(testTourDTO);

            Assert.AreEqual(t.Description, testTour.Description);
        }

        [Test]
        public void MapTourFromTest() {
            var t = PersonalMapper.TourFromDTO(testTourDTO);

            Assert.AreEqual(t.From, testTour.From);
        }

        [Test]
        public void MapTourToTest() {
            var t = PersonalMapper.TourFromDTO(testTourDTO);

            Assert.AreEqual(t.To, testTour.To);
        }

        [Test]
        public void MapTourTransportTest() {
            var t = PersonalMapper.TourFromDTO(testTourDTO);

            Assert.AreEqual(t.TransportType, testTour.TransportType);
        }

        [Test]
        public void MapTourDistanceTest() {
            var t = PersonalMapper.TourFromDTO(testTourDTO);

            Assert.AreEqual(testTour.Distance, t.Distance);
        }

        [Test]
        public void MapTourTimeTest() {
            var t = PersonalMapper.TourFromDTO(testTourDTO);

            Assert.AreEqual(testTour.Time, t.Time);
        }

        [Test]
        public void MapTourSearchTest() {
            var t = PersonalMapper.TourFromDTO(testTourDTO);

            Assert.AreEqual(testTour.SearchString, t.SearchString);
        }


        [Test]
        public void CheckChildFriendly() {
            Assert.That(testTour.ChildFriendly);
        }

        [Test]
        public void CheckTourPopularity() {
            Assert.AreEqual(Popularity.Good, testTour.Popularity);
        }

        [Test]
        public void CheckTourDifficulty() {
            Assert.AreEqual(Difficulty.Medium, testTour.Difficulty);
        }

        [Test]
        public void CheckCalculatedTourTime() {
            Assert.AreEqual("00:07:12", testTour.FormatedTime);
        }

        [Test]
        public void MapDtoIdTest() {
            var t = PersonalMapper.DTOFromTour(testTour);

            Assert.AreEqual(testTourDTO.Id, t.Id);
        }

        [Test]
        public void MapDtoNameTest() {
            var t = PersonalMapper.DTOFromTour(testTour);

            Assert.AreEqual(testTourDTO.Name, t.Name);
        }

        [Test]
        public void MapDtoDescriptionTest() {
            var t = PersonalMapper.DTOFromTour(testTour);

            Assert.AreEqual(testTourDTO.Description, t.Description);
        }

        [Test]
        public void MapDtoTimeTest() {
            var t = PersonalMapper.DTOFromTour(testTour);

            Assert.AreEqual(testTourDTO.Time, t.Time);
        }

        [Test]
        public void MapDtoDistanceTest() {
            var t = PersonalMapper.DTOFromTour(testTour);

            Assert.AreEqual(testTourDTO.Distance, t.Distance);
        }
    }
}