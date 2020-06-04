namespace DynamicRouteIssue
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using DynamicRouteIssue.Localization;

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
            services.AddScoped<CustomRouteValueTransformer>();
            services.ConfigureRequestLocalization();
            services.AddRazorPages().AddRazorPagesOptions(options =>
            {
                options.Conventions.Add(new CultureTemplateRouteModelConvention());
                options.Conventions.AddFolderRouteModelConvention("/", model =>
                {
                    var selectorCount = model.Selectors.Count;
                    for (var i = selectorCount - 1; i >= 0; i--)
                    {
                        var selectorTemplate = model.Selectors[i].AttributeRouteModel.Template;
                        if (selectorTemplate.EndsWith("Index"))
                        {
                            model.Selectors.RemoveAt(i);
                        }
                    }
                });
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseRequestLocalization();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // endpoints.MapRazorPages();
                endpoints.MapDynamicPageRoute<CustomRouteValueTransformer>("{**url}");
            });
        }
    }
}
