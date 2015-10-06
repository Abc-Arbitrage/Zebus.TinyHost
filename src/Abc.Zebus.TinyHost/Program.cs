using System;
using System.IO;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using IODirectory = System.IO.Directory;

namespace Abc.Zebus.TinyHost
{
    static class Program
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(Program));

        static Program()
        {
            IODirectory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            TaskScheduler.UnobservedTaskException += (sender, args) =>
            {
                _logger.ErrorFormat("Task unhandled exception: {0}", args.Exception);
                args.SetObserved();
            };

            AppDomain.CurrentDomain.UnhandledException += (sender, args) => _logger.Error($"AppDomain unhandled exception: {args.ExceptionObject}, IsTerminating: {args.IsTerminating}");
        }

        static void Main(string[] args)
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config");
            XmlConfigurator.ConfigureAndWatch(new FileInfo(filePath));

            _logger.Info($"Starting with args: {string.Join(",", args)}");

            try
            {
                var host = HostWrapperFactory.GetWrapper(args);
                host.Start();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        } 
    }
}
