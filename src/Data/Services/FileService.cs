using System;
using DesktopDashboard.Data.Helpers;

namespace DesktopDashboard.Data.Services
{
	public class FileService
	{
		private static string logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log");

        public FileService()
		{
		}

		public static ServiceResponse WriteLog(params string[] messages)
		{
			return WriteLogAsync(messages).Result;
		}

		public static async Task<ServiceResponse> WriteLogAsync(params string[] messages)
		{
			var logPath = Path.Combine(logDir, $"[{DateTime.Now.ToShortDateString}]log.txt");
			var response = new ServiceResponse(false);
			try
			{
				if(!Directory.Exists(logDir))
				{
					Directory.CreateDirectory(logDir);
				}

				if(!File.Exists(logPath))
				{
					File.Create(logPath);
				}

				using var logStream = new FileStream(logPath, FileMode.Append, FileAccess.Write);
				using var logWriter = new StreamWriter(logStream);

				foreach(var message in messages)
				{
					await logWriter.WriteLineAsync(message);
				}

				response.Success = true;
			}
			catch (Exception ex)
			{
				response.Message = ex.Message;
			}

			return response;
		}
	}
}

