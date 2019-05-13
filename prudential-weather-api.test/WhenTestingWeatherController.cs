using System.Collections.Generic;
using Autofac.Extras.Moq;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using prudential_weather_api.business;
using prudential_weather_api.Controllers;
using Model = prudential_weather_api.business.Model;
using prudential_weather_api.Contract;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace prudential_weather_api.test
{
    [TestClass]
    public class WhenTestingWeatherController
    {

        private IList<Model.City> cities;
        private int cityId;
        private IList<City> inputCities;
        private IList<Model.OpenWeather> openWeathersResponseModel, openWeathersResponseModelNull;

        [TestInitialize]
        public void SetupTestData()
        {
            cityId = 1275339;
            cities = new List<Model.City> { new Model.City { Id = cityId } };
            inputCities = new List<City> { new City { Id = cityId } };
            openWeathersResponseModel = new List<Model.OpenWeather> { new Model.OpenWeather { id = cityId, name = "Mumbai" } };
            openWeathersResponseModelNull = null;
        }

        [TestMethod]
        public async Task TestPostWithOkResponse()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IMapper>().Setup(s => s.Map<IEnumerable<Model.City>>(It.IsAny<IList<City>>())).Returns(cities);
                mock.Mock<IWeatherMiddleware>().Setup(s => s.GetWeather(cities)).ReturnsAsync(openWeathersResponseModel);             
                var controller = new WeatherController(mock.Create<IWeatherMiddleware>(), mock.Create<IMapper>());

                IHttpActionResult actionResult = await controller.Post(inputCities);
                var contentResult = actionResult as OkNegotiatedContentResult<IList<Model.OpenWeather>>;

                // Assert
                Assert.IsNotNull(contentResult);                        
            }
        }

        [TestMethod]
        public async Task TestPostWithInternalServerErrorResponse()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IMapper>().Setup(s => s.Map<IEnumerable<Model.City>>(It.IsAny<IList<City>>())).Returns(cities);
                mock.Mock<IWeatherMiddleware>().Setup(s => s.GetWeather(cities)).ReturnsAsync(openWeathersResponseModelNull);
                var controller = new WeatherController(mock.Create<IWeatherMiddleware>(), mock.Create<IMapper>());

                IHttpActionResult actionResult = await controller.Post(inputCities);           
                Assert.IsInstanceOfType(actionResult, typeof(InternalServerErrorResult));
            }
        }
    }
}
