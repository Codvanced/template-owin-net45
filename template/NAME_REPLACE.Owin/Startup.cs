using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.StaticFiles;
using System.Web.Http;
using NAME_REPLACE.WebMvcApp.Infrastructure;
using NAME_REPLACE.Configs;

[assembly: OwinStartup(typeof(NAME_REPLACE.WebMvcApp.Startup))]

namespace NAME_REPLACE.WebMvcApp
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var staticFilesConfig = StaticFilesConfiguration();
            var webApiConfig = WebApiConfiguration();

            IoCFrameworkConfig.Register(webApiConfig);

            app.UseFileServer(staticFilesConfig)
                .UseWebApi(webApiConfig);
        }

        private FileServerOptions StaticFilesConfiguration()
        {
            var options = new FileServerOptions
            {
                EnableDefaultFiles = true,
                FileSystem = new WebPhysicalFileSystem(Constants.STATIC_FOLDER)
            };

            return options;
        }

        private HttpConfiguration WebApiConfiguration()
        {
            var webApiConfig = new HttpConfiguration();
            webApiConfig.MapHttpAttributeRoutes();
            webApiConfig.Routes.MapHttpRoute(
                "default",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional }
            );

            return webApiConfig;
        }
    }
}
