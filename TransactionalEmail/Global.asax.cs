using System.Web.Http;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using TransactionalEmail.Infrastructure.DependencyInjection;

namespace TransactionalEmail
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ConfigureSimpleInjector();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AutoMapperConfig.Bootstrap();
        }

        private static void ConfigureSimpleInjector()
        {
            var container = new Container();
            new Registry().RegisterServices(container);
            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
        }
    }
}
