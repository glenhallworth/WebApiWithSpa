using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;

namespace WebApiWithSpa.Api.Infrastructure.Middleware
{
    public static class AppBuilderStaticFileServer
    {
        public static IApplicationBuilder UseStaticFileServerWithFallbackToIndex(this IApplicationBuilder app)
        {
            return app.Use((context, next) =>
                {
                    if (ShouldServeIndexPage(context))
                    {
                        context.Request.Path = new PathString("/index.html");
                    }

                    return next();
                })
                .UseFileServer(new FileServerOptions
                {
                    EnableDirectoryBrowsing = false,
                    EnableDefaultFiles = true,
                    DefaultFilesOptions =
                    {
                        DefaultFileNames = { "index.html" }
                    },
                    StaticFileOptions =
                    {
                        OnPrepareResponse = NoCacheOnIndex
                    }
                });
        }

        private static bool ShouldServeIndexPage(HttpContext context)
        {
            var path = context.Request.Path.ToString().ToLowerInvariant();

            if (path.StartsWith("/api"))
                return false;

            return string.IsNullOrEmpty(Path.GetExtension(path));
        }

        private static void NoCacheOnIndex(StaticFileResponseContext context)
        {
            if (context.File.Name == "index.html")
            {
                context.Context.Response.Headers.Add("Cache-Control", new[] { "no-cache", "no-store", "must-revalidate" });
                context.Context.Response.Headers.Add("Pragma", new[] { "no-cache" });
                context.Context.Response.Headers.Add("Expires", new[] { "0" });
            }
        }
    }
}
