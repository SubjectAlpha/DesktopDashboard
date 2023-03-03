using System;
using DesktopDashboard.Data.Helpers;
using Newtonsoft.Json;

namespace DesktopDashboard.Data.Services;

public class FileService
{
	private string rootDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "desktopdashboard");

	public FileService()
	{
		if(!Directory.Exists(rootDir))
		{
			Directory.CreateDirectory(rootDir);
		}
	}

    public ServiceResponse WriteSettings(Dictionary<string, string> settings)
	{
		return WriteSettingsAsync(settings).Result;
	}

	public async Task<ServiceResponse> WriteSettingsAsync(Dictionary<string, string> settings)
	{
		var settingsPath = Path.Combine(rootDir, "settings.json");
		var response = new ServiceResponse();

		try
		{
			if (!File.Exists(settingsPath))
			{
				File.Create(settingsPath);
            }

            using var logStream = new FileStream(settingsPath, FileMode.Append, FileAccess.Write);
            using var logWriter = new StreamWriter(logStream);
			var s = JsonConvert.SerializeObject(settings);
			await logWriter.WriteAsync(s);
        }
		catch (Exception ex)
		{
			response.Message = ex.Message;
		}

		return response;
	}

	public ServiceResponse WriteLog(params string[] messages)
	{
		return WriteLogAsync(messages).Result;
	}

	public async Task<ServiceResponse> WriteLogAsync(params string[] messages)
	{
		var logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log");
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
