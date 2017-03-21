using Microsoft.SharePoint.Administration;
using System;
using System.Diagnostics;


namespace Custom.SP2013.Branding
{
    public class CustomPerformanceTimerLogger : IDisposable

    {
        SPDiagnosticsService diagSvc = SPDiagnosticsService.Local;
        private string message;
        private string webUrl;
        public CustomPerformanceTimerLogger(string message, string url)
        {
            this.message = message;
            this.webUrl = url;
            this.timer = new Stopwatch();
            this.timer.Start();

        }
        Stopwatch timer;
        public void Dispose()
        {
            this.timer.Stop();
            var msg = this.timer.ElapsedMilliseconds;
            //log messages
            diagSvc.WriteTrace(0, new SPDiagnosticsCategory("Custom Branding", TraceSeverity.Monitorable, EventSeverity.Error), TraceSeverity.Monitorable, "StopWatch {0}---Elapsed milliseconds: {1}---Location:{2}", this.message, msg, this.webUrl);
        }
    }
}
