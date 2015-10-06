using System.Linq;
using Abc.Zebus.TinyHost.Wrapper;

namespace Abc.Zebus.TinyHost
{
    public static class HostWrapperFactory
    {
        public static IHostWrapper GetWrapper(string[] args)
        {
            if (args.Contains("service"))
                return new ServiceHostWrapper();
            return new ConsoleHostWrapper();
        }
    }
}