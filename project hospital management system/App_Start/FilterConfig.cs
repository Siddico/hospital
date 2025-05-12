using System.Web;
using System.Web.Mvc;
using project_hospital_management_system.Utilities;

namespace project_hospital_management_system
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CustomHandleErrorAttribute());
        }
    }
}
