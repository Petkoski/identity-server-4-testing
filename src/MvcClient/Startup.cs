using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IdentityModel;
using System.Net.Http;
using Clients;
using MvcClient.AutomaticTokenManagement;

namespace MvcClient
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddHttpClient();

            //services.AddSingleton<IDiscoveryCache>(r =>
            //{
            //    var factory = r.GetRequiredService<IHttpClientFactory>();
            //    return new DiscoveryCache(Constants.AuthorityBaseAddress, () => factory.CreateClient());
            //});

            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            services
                .AddAuthentication(options => //Adds the authentication services to DI
                {
                    options.DefaultScheme = "Cookies"; //We are using a cookie to locally sign-in the user (via "Cookies" as the DefaultScheme)
                    options.DefaultChallengeScheme = "oidc"; //OpenID Connect protocol
                })
                .AddCookie("Cookies")
                .AddAutomaticTokenManagement()
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;

                    options.ClientId = "jovanmvc";
                    options.ClientSecret = "mypass2";
                    options.ResponseType = "code"; //We use the so called "authorization code" flow with PKCE to connect to the OpenID Connect provider.

                    options.SaveTokens = true; //To persist the tokens from IdentityServer in the cookie
                    //Since SaveTokens is enabled, ASP.NET Core will automatically store the resulting access 
                    //and refresh token in the authentication session

                    //Ask for the additional resources via the scope parameter
                    options.Scope.Add("api1");
                    options.Scope.Add("offline_access");
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints
                    .MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}")
                    .RequireAuthorization(); //Disables anonymous access for the entire application. You can also use the [Authorize] attribute, if you want to specify that on a per controller or action method basis.
            });
        }
    }
}
