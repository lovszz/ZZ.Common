using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ZZ.Common.ThirdParty.Log4Net;

namespace ZZ.Common.Configurations
{
    public class Configuration
    {
        public Configuration()
        {

        }
        public  Configuration UseLog4Net(FileInfo configFile)
        {
            var repository=LogManager.CreateRepository("Log4NetRepository");
            log4net.Config.XmlConfigurator.Configure(repository, configFile);
            log4net.Config.BasicConfigurator.Configure(repository);
            Log4Netmanager log4Netmanager = new Log4Netmanager(LogManager.GetLogger(repository.Name, "Log4Netmanager"));
            return this;
        }
    }
}
