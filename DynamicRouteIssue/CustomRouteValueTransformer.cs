namespace DynamicRouteIssue
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Routing;
    using Microsoft.AspNetCore.Routing;

    public class CmsPage
    {
        public string Url { get; set; }

        public string Template { get; set; }
    }

    public class CustomRouteValueTransformer : DynamicRouteValueTransformer
    {
        private readonly List<CmsPage> pages = new List<CmsPage>();

        public CustomRouteValueTransformer()
        {
            this.pages.Add(new CmsPage() { Url = "privacy", Template = "/Privacy" });
            this.pages.Add(new CmsPage() { Url = "error", Template = "/Index" });
            this.pages.Add(new CmsPage() { Url = "", Template = "/Index" });
        }

        public override async ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
        {
            return await Task.Run(() =>
            {
                if (values["url"] != null)
                {
                    var url = values["url"].ToString();
                    var urlParts = url.Split('/');
                     if (urlParts.First().Equals(CultureInfo.CurrentCulture.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        url = string.Join('/', urlParts.Skip(1));
                    }

                    var page = pages.FirstOrDefault(x => x.Url.Equals(url, StringComparison.CurrentCultureIgnoreCase));
                    if (page != null)
                    {
                        values["page"] = page.Template;
                    }
                }

                return values;
            });
        }
    }
}
