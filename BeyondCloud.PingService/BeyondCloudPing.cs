using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Configuration;
using System.IO;

namespace BeyondCloud.PingService
{
    public partial class BeyondCloudPing : ServiceBase
    {
        private readonly AppSettingsReader _appSettingsReader = new System.Configuration.AppSettingsReader();
        private double _pingInterval;
        private string _serverName;
        private string _serverFolder;

        public BeyondCloudPing()
        {
            InitializeComponent();

            #region Event Log
            eventLog = new EventLog();
            if (!EventLog.SourceExists("BeyondCloudPing"))
            {
                EventLog.CreateEventSource("BeyondCloudPing", "BeyondCloudPingLog");
            }
            eventLog.Source = "BeyondCloudPing";
            eventLog.Log = "BeyondCloudPingLog";
            #endregion
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                _pingInterval = double.Parse(_appSettingsReader.GetValue("PingInterval", typeof(string)).ToString());
                _serverName = _appSettingsReader.GetValue("ServerName", typeof(string)).ToString();
                _serverFolder = _appSettingsReader.GetValue("ServerFolder", typeof(string)).ToString();

                // Set up a timer that triggers every minute.
                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Interval = _pingInterval;
                timer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimer);
                timer.Start();
            }
            catch(Exception ex)
            {
                eventLog.WriteEntry($"An error occurred starting service: {ex.Message}", EventLogEntryType.Error);
                throw;
            }
        }

        protected override void OnStop()
        {
            eventLog.WriteEntry("Stopped");
        }

        private void OnTimer(object sender, ElapsedEventArgs e)
        {
            try
            {
                eventLog.WriteEntry("Ping");
                var path = $"{Path.DirectorySeparatorChar}{Path.DirectorySeparatorChar}{_serverName}{Path.DirectorySeparatorChar}{_serverFolder}";
                eventLog.WriteEntry($"Path: {path}");
                var di = new DirectoryInfo(path);

                if (di.Exists)
                {
                    var result = di.GetDirectories();
                    eventLog.WriteEntry($"{_serverFolder} directories: {result.Length}");
                }
                else
                    eventLog.WriteEntry("Path doesnt exist", EventLogEntryType.Error);
            }
            catch(Exception ex)
            {
                eventLog.WriteEntry($"An error occurred in service: {ex.Message}", EventLogEntryType.Error);
                throw;
            }
        }
    }
}
