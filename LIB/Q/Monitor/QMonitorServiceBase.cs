﻿using System.ServiceProcess;

namespace LIB.Q.Monitor
{
    static class QMonitorServiceBase
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                //new QMonitorService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
