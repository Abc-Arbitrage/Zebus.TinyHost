using System;
using System.Diagnostics;
using System.Linq;
using Abc.Zebus.Hosting;
using Abc.Zebus.Initialization;
using Abc.Zebus.TinyHost.Configuration; 
using Abc.Zebus.Transport;
using log4net;
using StructureMap;
using StructureMap.Graph;

namespace Abc.Zebus.TinyHost
{
    internal class Host
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(Host));
        private Container _container;
        private IBus _bus;

        public Host()
        {
            ContainerFactory = CreateAndConfigureContainer;
        }

        private static Container CreateAndConfigureContainer()
        {
            var container = new Container();

            container.Configure(c => c.AddRegistry<ZebusRegistry>());
            container.Configure(x => x.ForSingletonOf<ISettingsReader>().Use(new AppSettingsReader()));
            container.Configure(cfg =>
            {
                cfg.ForSingletonOf<IBusConfiguration>().Use<BusConfiguration>();
                cfg.ForSingletonOf<IZmqTransportConfiguration>().Use<ZmqTransportConfiguration>();
            });

            container.Configure(cfg =>
            {
                cfg.Scan(x =>
                {
                    AddAssembliesToScan(x);
                    x.LookForRegistries();
                    x.With(new HostInitializerRegistrationConvention());
                });
            });
            return container;
        }

        internal Func<Container> ContainerFactory { get; set; }

        public void Start()
        {
            try
            {
                _logger.Info("Zebus Host is starting...");
                _logger.Info($"User: {Environment.UserName}");
                _logger.Info($"PID: {Process.GetCurrentProcess().Id}");

                _container = ContainerFactory.Invoke();

                _bus = _container.GetInstance<IBus>();

                _container.CallActionOnInitializers(x => x.ConfigureContainer(_container));

                ConfigureBus();

                _container.CallActionOnInitializers(x => x.BeforeStart());

                _logger.Info("Starting bus");
                _bus.Start();

                _container.CallActionOnInitializers(x => x.AfterStart());

                _logger.Info("Zebus Host started"); 
            }
            catch (Exception ex)
            {
                _logger.Error(ex);

                if (_bus != null && _bus.IsRunning)
                    StopBus();

                throw;
            }
        }

        private void ConfigureBus()
        {
            var settingReader = _container.GetInstance<ISettingsReader>();

            var peerId = settingReader.Read<string>("Bus.PeerId");
            var environment = settingReader.Read<string>("Bus.Environment");

            _bus.Configure(new PeerId(peerId), environment);
        }

        public void Stop()
        {
            try
            {
                _logger.Info("Zebus Host is stopping...");
                _container.CallActionOnInitializers(x => x.BeforeStop(), invertPriority: true);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
            finally
            {
                StopBus();
                CallAfterStop();
                _container.Dispose();
            }
        }

        private void CallAfterStop()
        {
            try
            {
                _container.CallActionOnInitializers(x => x.AfterStop(), invertPriority: true);
                _logger.Info("Zebus Host stopped");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        private void StopBus()
        {
            _logger.Info("Stopping bus");
            try
            {
                _bus.Stop();
            }
            catch (Exception ex)
            {
                _logger.ErrorFormat("Unable to stop bus: {0}", ex);
            }
        }

        private static void AddAssembliesToScan(IAssemblyScanner assemblyScanner)
        {
            var assemblies = AssemblyScanner.GetAssembliesInFolder(AppDomain.CurrentDomain.BaseDirectory, fileName => !fileName.StartsWith("libzmq-"))
                                            .Where(a => a != typeof(ZebusRegistry).Assembly);

            foreach (var assembly in assemblies)
            {
                assemblyScanner.Assembly(assembly);
            }
        }
  
    }
}