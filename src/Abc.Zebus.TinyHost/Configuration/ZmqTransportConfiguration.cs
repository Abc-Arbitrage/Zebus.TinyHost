using System;
using Abc.Zebus.Transport;

namespace Abc.Zebus.TinyHost.Configuration
{
    public class ZmqTransportConfiguration : IZmqTransportConfiguration
    {
        private readonly ISettingsReader _settingsReader;

        public ZmqTransportConfiguration(ISettingsReader settingsReader)
        {
            _settingsReader = settingsReader;
        }

        public string InboundEndPoint => _settingsReader.Read<string>("Bus.InboundEndPoint");
        public TimeSpan WaitForEndOfStreamAckTimeout => _settingsReader.Read<TimeSpan>("Bus.WaitForEndOfStreamAckTimeout");
    }
}