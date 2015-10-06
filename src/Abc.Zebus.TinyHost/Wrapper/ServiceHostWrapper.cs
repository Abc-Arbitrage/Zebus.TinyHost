using System.ServiceProcess;
using log4net;

namespace Abc.Zebus.TinyHost.Wrapper
{
    public class ServiceHostWrapper : ServiceBase, IHostWrapper
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ServiceHostWrapper));
        private readonly Host _host;

        public ServiceHostWrapper()
        {
            _host = new Host();
        }

        protected override void OnStart(string[] args)
        {
            _host.Start();
        }

        protected override void OnStop()
        {
            _host.Stop();
        }

        protected override void OnShutdown()
        {
            _host.Stop();
        }

        public void Start()
        {
            _log.Info("Starting up as a windows service application");
            Run(this);
        }
    }
}