using Serilog;
using Serilog.Core;
using System;
using System.IO;

namespace WSATools.Libs
{
    public sealed class LogManager
    {
        private static LogManager _Instance;
        public static LogManager Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new LogManager();
                return _Instance;
            }
        }
        private Logger Logger { get; }
        private LogManager()
        {
            var LogFile = Path.Combine(Environment.CurrentDirectory, "wsa_tools_log", "log-.txt");
            Logger = new LoggerConfiguration().MinimumLevel.Debug()
                .WriteTo.Async(a => a.File(LogFile, rollingInterval: RollingInterval.Day))
                .CreateLogger();
        }
        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="messageTemplate">错误方法或者备注</param>
        /// <param name="exception">错误信息</param>
        /// <param name="Serious">是否严重错误</param>
        public void LogError(string messageTemplate, Exception exception)
        {
            try
            {
                if (exception != null)
                    Logger.Error(exception, messageTemplate);
            }
            catch { }
        }
        public void LogInfo(string messageTemplate)
        {
            try
            {
                Logger.Information(messageTemplate);
            }
            catch { }
        }
    }
}