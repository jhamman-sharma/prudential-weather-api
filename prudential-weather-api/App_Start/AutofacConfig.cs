using System.Configuration;
using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using AutoMapper;
using prudential_weather_api.business;
using prudential_weather_api.repository;

namespace prudential_weather_api
{
    public static class AutofacConfig
    {
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<OpenWeatherServiceRepository>().As<IOpenWeatherServiceRepository>()
            .WithParameter("baseUrl",ConfigurationManager.AppSettings["OpenWeatherSericeBaseAddress"])
            .WithParameter("appId", ConfigurationManager.AppSettings["OpenWeatherSericeAppId"])
            .InstancePerRequest();

            builder.RegisterType<WeatherMiddleware>().As<IWeatherMiddleware>().InstancePerRequest();

            builder.Register(c => AutoMapperConfiguration.Configuration()).As<IMapper>().SingleInstance();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(builder.Build());
        }
    }
}