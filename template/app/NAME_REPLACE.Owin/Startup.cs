using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.StaticFiles;
using System.Web.Http;
using NAME_REPLACE.WebMvcApp.Infrastructure;
using Microsoft.Owin.FileSystems;

[assembly: OwinStartup(typeof(NAME_REPLACE.WebMvcApp.Startup))]

namespace NAME_REPLACE.WebMvcApp
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var webApiConfig = new HttpConfiguration();
            var staticFilesConfig = StaticFilesConfiguration();

            IoCFrameworkConfig.Register(webApiConfig);
            WebApiConfig.Register(webApiConfig);

            app.UseFileServer(staticFilesConfig)
                .UseWebApi(webApiConfig);
        }

        private FileServerOptions StaticFilesConfiguration()
        {
            var physicalFileSystem = new PhysicalFileSystem(Constants.STATIC_FOLDER);
            var options = new FileServerOptions
            {
                EnableDefaultFiles = true,
                FileSystem = physicalFileSystem
            };
            options.StaticFileOptions.FileSystem = physicalFileSystem;
            options.StaticFileOptions.ServeUnknownFileTypes = true;
            options.DefaultFilesOptions.DefaultFileNames = new[] { "index.html" };
            //appBuilder.UseFileServer(options);


            //var options = new FileServerOptions
            //{
            //    EnableDefaultFiles = true,
            //    FileSystem = new WebPhysicalFileSystem(Constants.STATIC_FOLDER)
            //};

            //options.StaticFileOptions.ServeUnknownFileTypes = true;

            return options;
        }
    }
}
