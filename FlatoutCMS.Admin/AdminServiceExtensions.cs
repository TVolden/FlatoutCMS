using Microsoft.Extensions.DependencyInjection;

namespace FlatoutCMS.Admin
{
    public static class AdminServiceExtensions
    {
        public static IServiceCollection AddFlatoutAdmin(this IServiceCollection services)
        {
            services.AddScoped<PasswordFileService>();
            services.AddAuthentication("FlatoutAdmin")
                .AddCookie("FlatoutAdmin", options =>
                {
                    options.LoginPath = "/Admin/Login";
                    options.AccessDeniedPath = "/Admin/Login";
                    options.ExpireTimeSpan = TimeSpan.FromHours(8);
                    options.SlidingExpiration = true;
                });
            services.AddAuthorization();
            return services;
        }
    }
}
