namespace DynamicRouteIssue.Localization
{
    using System.Globalization;
    using System.Linq;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.Extensions.DependencyInjection;

    public static class Extensions
    {
        public static void ConfigureRequestLocalization(this IServiceCollection services)
        {
            var cultures = new CultureInfo[]
            {
                new CultureInfo("en-gb"),
                new CultureInfo("en-us"),
                new CultureInfo("it-it"),
                new CultureInfo("fr-fr"),
                new CultureInfo("de-de"),
                new CultureInfo("es-es")
                {
                    DateTimeFormat =
                    {
                        Calendar = new GregorianCalendar(),
                    },
                    NumberFormat =
                    {
                        NativeDigits = "0 1 2 3 4 5 6 7 8 9".Split(" "),
                    },
                },
            };

            services.Configure<RequestLocalizationOptions>(ops =>
            {
                ops.DefaultRequestCulture = new RequestCulture("en-gb");
                ops.SupportedCultures = cultures.OrderBy(x => x.EnglishName).ToList();
                ops.SupportedUICultures = cultures.OrderBy(x => x.EnglishName).ToList();
                ops.RequestCultureProviders.Insert(0, new RouteValueRequestCultureProvider(cultures));
            });
        }

        public static string CustomCultureName(string cultureCode = null)
        {
            return (cultureCode ?? CultureInfo.CurrentCulture.Name).ToLower() switch
            {
                "en-gb" => "English (UK)",
                "en-us" => "English (US)",
                "es-es" => "Español",
                "de-de" => "Deutsch",
                "it-it" => "Italiano",
                "fr-fr" => "Français",
                _ => null,
            };
        }
    }
}
