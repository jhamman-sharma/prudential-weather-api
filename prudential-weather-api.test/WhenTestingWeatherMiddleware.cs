using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using prudential_weather_api.business;
using prudential_weather_api.repository;
using Entities = prudential_weather_api.repository.Entities;
using Model = prudential_weather_api.business.Model;

namespace prudential_weather_api.test
{
    [TestClass]
    public class WhenTestingWeatherMiddleware
    {
        private int cityId;
        private int invalidCityId;
        private Entities.OpenWeather serviceResponse, serviceResponseNull;
        private IEnumerable<Model.City> cities;
        private IList<Entities.OpenWeather> openWeathersResponseEntities;
        private IList<Model.OpenWeather> openWeathersResponseModel;

        [TestInitialize]
        public void SetupTestData()
        {
            cityId = 1275339;
            invalidCityId = 4324323;
            serviceResponse = new Entities.OpenWeather { id = cityId, name = "Mumbai" };
            serviceResponseNull = null;
            openWeathersResponseEntities = new List<Entities.OpenWeather> { new Entities.OpenWeather { id = cityId, name = "Mumbai" } };
            openWeathersResponseModel = new List<Model.OpenWeather> { new Model.OpenWeather { id = cityId, name = "Mumbai" } };
            cities = new List<Model.City> { new Model.City { Id = cityId } };
        }

        [TestMethod]
        public async Task TestGetWeatherWithData()
        {
            using(var mock = AutoMock.GetLoose())
            {
                var middleware = mock.Create<WeatherMiddleware>();
                mock.Mock<IOpenWeatherServiceRepository>().Setup(s => s.GetWeather(cityId)).ReturnsAsync(serviceResponse);
                mock.Mock<IMapper>().Setup(s => s.Map<IList<Model.OpenWeather>>(It.IsAny<IList<Entities.OpenWeather>>())).Returns(openWeathersResponseModel);
                var result = await middleware.GetWeather(cities);
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public async Task TestGetWeatherWithNoServiceResponse()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var middleware = mock.Create<WeatherMiddleware>();
                mock.Mock<IOpenWeatherServiceRepository>().Setup(s => s.GetWeather(invalidCityId)).ReturnsAsync(serviceResponseNull);          
                var result = await middleware.GetWeather(cities);
                Assert.IsNull(result);
            }
        }
    }
}
