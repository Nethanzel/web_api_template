﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace dataCoreService.Middleware
{
    public class UserKeyValidatorsMiddleware
    {
        private readonly RequestDelegate _next;
        public UserKeyValidatorsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.Keys.Contains("token"))
            {
                context.Response.StatusCode = 400; //Bad Request ** Missing auth token               
                return;
            }
            else
            {
                HttpClient validator = new HttpClient //init http client
                {
                    Timeout = TimeSpan.FromSeconds(30)
                };

                var values = new Dictionary<string, string> // init data to be sent
                {
                    { "token", context.Request.Headers["token"] },
                };

                try
                {
                    HttpResponseMessage response = await validator.PostAsync("http://127.0.0.1/auth/validate", new FormUrlEncodedContent(values)); //send token to auth service
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