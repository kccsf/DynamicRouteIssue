namespace DynamicRouteIssue.Localization
{
    using Microsoft.AspNetCore.Mvc.ApplicationModels;

    public class CultureTemplateRouteModelConvention : IPageRouteModelConvention
    {
        // https://github.com/dotnet/aspnetcore/issues/16960
        public void Apply(PageRouteModel model)
        {
            var selectorCount = model.Selectors.Count;
            for (var i = 0; i < selectorCount; i++)
            {
                var selector = model.Selectors[i];
                model.Selectors.Add(new SelectorModel
                {
                    AttributeRouteModel = new AttributeRouteModel
                    {
                        Order = -1,
                        Template = AttributeRouteModel.CombineTemplates("{culture}", selector.AttributeRouteModel.Template), // Ref above github issue - culture changed to required
                    },
                });

                // Ref above github issue - original routes suppressed so URLs can't be made using its non-prefixed template
                selector.AttributeRouteModel.SuppressLinkGeneration = true;
            }
        }
    }
}
