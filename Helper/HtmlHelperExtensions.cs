using Microsoft.AspNetCore.Mvc.Rendering;

namespace RestaurantMVC.Helper
{
    public static class HtmlHelperExtensions
    {
        public static string IsActive(this IHtmlHelper html, string controller)
        {
            var currentController = html.ViewContext.RouteData.Values["controller"]?.ToString();
            return controller == currentController ? "active" : "";
        }
    }
}
