using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public static class Logger
    {
        private static string _logsPath;
        private static string _workingDirectory = @$"{Directory.GetCurrentDirectory()}\Logs\";
        public static int? SeverityToLog { get; set; } = null;

        // Create log
        public static void ConfigLogger(int severityToLog)
        {
            SeverityToLog = severityToLog;
            _logsPath = $"{_workingDirectory + DateTime.UtcNow.ToString("yyyy-MM-dd_hh-mm")}.txt";
            Directory.CreateDirectory(_workingDirectory);
        }

        public static void WriteData(int severity, string category, string message)
        {
            // If seveerity is null stop logging
            if (SeverityToLog is null)
                return;

            // Log only the desired severity
            if (SeverityToLog.Value == severity)
                File.AppendAllText(_logsPath, $"{DateTime.UtcNow.ToString("hh:mm")}|{severity}|{category}|{message}\n");
        }

        public static void ClearLogs()
        {
            // Delete every log file
            foreach (string logName in Directory.GetFiles($@"{Directory.GetCurrentDirectory()}\Logs"))
            {
                File.Delete(_workingDirectory + logName);
            }
        }
    }
}
