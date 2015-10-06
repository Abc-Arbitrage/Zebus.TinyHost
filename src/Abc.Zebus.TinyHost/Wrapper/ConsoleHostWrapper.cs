using System;
using System.Threading;
using log4net;

namespace Abc.Zebus.TinyHost.Wrapper
{
    public class ConsoleHostWrapper : IHostWrapper
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ConsoleHostWrapper));

        private readonly Host _host = new Host();
        private readonly ManualResetEvent _terminationHandle;

        public ConsoleHostWrapper()
        {
            _terminationHandle = new ManualResetEvent(false);
        }

        public void Start()
        {
            HandleControlC();

            _logger.Info("Starting up as a console application");

            _host.Start();

            _logger.InfoFormat("Zebus host is running, press Ctrl+C to exit.");

            _terminationHandle.WaitOne();

            _host.Stop(); 
        }

        // This is a workaround to prevent VisualStudio from breaking in .NET 4.0 when hiting Ctrl+C
        private void HandleControlC()
        {
            Console.TreatControlCAsInput = true;
            var thread = new Thread(ListenForControlC) { IsBackground = true };
            thread.Start(this);
        }
         

        private  void ListenForControlC(object obj)
        {
            var isRunning = true;
            while (isRunning)
            {
                var keyInfo = Console.ReadKey(true);
                if ((keyInfo.Modifiers & ConsoleModifiers.Control) != 0 && keyInfo.Key.Equals(ConsoleKey.C))
                { 
                    _terminationHandle.Set();
                    isRunning = false;
                }
            }
        }
    }
}