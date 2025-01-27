namespace Project_Consume_Api
{

	public class CommonVariable
	{
		private static IHttpContextAccessor _HttpContextAccessor;

		static CommonVariable()
		{
			_HttpContextAccessor = new HttpContextAccessor();
		}


		public static int? StudentID()
		{

			if (_HttpContextAccessor.HttpContext.Session.GetString("StudentID") == null)
			{
				return null;
			}

			return Convert.ToInt32(_HttpContextAccessor.HttpContext.Session.GetString("StudentID"));
		}

		public static int? TeacherID()
		{

			if (_HttpContextAccessor.HttpContext.Session.GetString("TeacherID") == null)
			{
				return null;
			}

			return Convert.ToInt32(_HttpContextAccessor.HttpContext.Session.GetString("TeacherID"));
		}

		public static string Email()
		{
			if (_HttpContextAccessor.HttpContext.Session.GetString("EmailAddress") == null)
			{
				return null;
			}

			return _HttpContextAccessor.HttpContext.Session.GetString("EmailAddress");
		}


	}
}
