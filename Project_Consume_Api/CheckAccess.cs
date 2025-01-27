using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Project_Consume_Api
{
	public class CheckAccess : ActionFilterAttribute, IAuthorizationFilter
	{
		public void OnAuthorization(AuthorizationFilterContext filterContext)
		{
			var st = filterContext.HttpContext.Session.GetString("StudentID");
			var t = filterContext.HttpContext.Session.GetString("TeacherID");
			if (filterContext.HttpContext.Session.GetString("StudentID") == null && filterContext.HttpContext.Session.GetString("TeacherID") == null)
			{
				filterContext.Result = new RedirectResult("~/Teacher/Auth/LogIn");
			}

		}

		public override void OnResultExecuting(ResultExecutingContext context)
		{
			context.HttpContext.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
			context.HttpContext.Response.Headers["Expires"] = "-1";
			context.HttpContext.Response.Headers["Pragma"] = "no-cache";
			base.OnResultExecuting(context);
		}
	}
}
