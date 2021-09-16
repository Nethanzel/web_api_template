using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace dataCoreService.Middleware
{
    public class UserKeyValidatorsMiddleware
    {
        private readonly RequestDelegate _next;
        public IConfiguration Configuration { get; }

        public UserKeyValidatorsMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            Configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.Value.StartsWith("/swagger"))
            {
                await _next.Invoke(context); // call next middleware
                return;
            }

            if (!context.Request.Headers.Keys.Contains("token"))
            {
                context.Response.StatusCode = 400; //Bad Request ** Missing auth token               
                return;
            }
            else
            {
                HttpClient validator = new HttpClient //init http client
                {
                    Timeout = TimeSpan.FromSeconds(30),
                };

                validator.DefaultRequestHeaders.Add("token", String.Format(context.Request.Headers["token"])); // set request header

                try
                {
                    HttpResponseMessage response = await validator.GetAsync(Configuration["external:authServer"]); //send token to auth service
                    var statusCode = ((int)response.StatusCode);
                    if (statusCode == 401)  //if code 401 means invalid token
                    {
                        context.Response.StatusCode = 401; //Denied ** Missing correct auth token               
                        return;
                    }
                }
                catch (Exception)
                {
                    context.Response.StatusCode = 503; // timeout exception response
                    return;
                }

                //if code 204 means valid token
            }
            await _next.Invoke(context);
        }
    }

    public static class UserKeyValidatorsExtension
    {
        public static IApplicationBuilder ApplyUserKeyValidation(this IApplicationBuilder app)
        {
            app.UseMiddleware<UserKeyValidatorsMiddleware>();
            return app;
        }
    }
}
