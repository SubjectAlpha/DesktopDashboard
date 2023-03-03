using System;
namespace DesktopDashboard.Data.Helpers
{
	public class ServiceResponse
	{
		public object DataObject;
		public string Message;
		public bool Success;

		public ServiceResponse(bool success, string message = "", object o = null)
		{
			this.Message = message;
			this.Success = success;
			this.DataObject = o;
		}
	}
}

