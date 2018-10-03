using System.Web;
using System.Web.Mvc;

namespace MVC_MUSIC_STORE
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new HandleErrorAttribute());
            filters.Add(new RequireHttpsAttribute());
        }
    }
}
