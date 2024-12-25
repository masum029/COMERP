using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text;
using UI.ApiSettings;
using UI.Dtos;
using UI.Models;
using UI.Services.Implemettions;
using UI.Services.Interface;

namespace UI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Reading the BaseUrl value from configuration
            var baseUrl = configuration["BaseUrl:AuthenticationAPI"];
            services.Configure<ApiUrlSettings>(configuration.GetSection("ApiUrls"));

            // Assign it to Helper.BaseUrl if Helper is a static class
            ApiUrlSettings._BaseUrl = baseUrl;



            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            services.AddHttpClient();
            services.AddScoped<IHttpService, HttpService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IFileUploader, FileUploader>();
            services.AddScoped<IFileHelper, FileHelper>();
            services.AddScoped<IUtilityHelper, UtilityHelper>();


            services.AddScoped<IClientServices<Register>, ClientServices<Register>>();
            services.AddScoped<IClientServices<Login>, ClientServices<Login>>();
            services.AddScoped<IClientServices<User>, ClientServices<User>>();
            services.AddScoped<IClientServices<Role>, ClientServices<Role>>();
            services.AddScoped<IClientServices<Company>, ClientServices<Company>>();
            services.AddScoped<IClientServices<Client>, ClientServices<Client>>();
            services.AddScoped<IClientServices<CompanyDetails>, ClientServices<CompanyDetails>>();
            services.AddScoped<IClientServices<ContactFormSubmission>, ClientServices<ContactFormSubmission>>();
            services.AddScoped<IClientServices<Event>, ClientServices<Event>>();
            services.AddScoped<IClientServices<FooterLink>, ClientServices<FooterLink>>();
            services.AddScoped<IClientServices<Menu>, ClientServices<Menu>>();
            services.AddScoped<IClientServices<News>, ClientServices<News>>();
            services.AddScoped<IClientServices<PageContent>, ClientServices<PageContent>>();
            services.AddScoped<IClientServices<Project>, ClientServices<Project>>();
            services.AddScoped<IClientServices<Service>, ClientServices<Service>>();
            services.AddScoped<IClientServices<SiteSettings>, ClientServices<SiteSettings>>();
            services.AddScoped<IClientServices<Slider>, ClientServices<Slider>>();
            services.AddScoped<IClientServices<SocialMediaLink>, ClientServices<SocialMediaLink>>();



            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Auth/Login";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(120);
                    options.ReturnUrlParameter = "ReturnUrl";
                });
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(120);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            return services;
        }
    }
}
