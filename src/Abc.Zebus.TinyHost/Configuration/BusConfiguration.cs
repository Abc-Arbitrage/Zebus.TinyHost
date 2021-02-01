using System;

namespace Abc.Zebus.TinyHost.Configuration
{
    public class BusConfiguration : IBusConfiguration
    {
        private readonly ISettingsReader _settingsReader;

        public BusConfiguration(ISettingsReader settingsReader)
        {
            _settingsReader = settingsReader;
        }
         
        public string[] DirectoryServiceEndPoints => _settingsReader.Read<string[]>("Bus.Directory.EndPoints");
        public TimeSpan RegistrationTimeout => _settingsReader.Read<TimeSpan>("Bus.Directory.RegistrationTimeout");
        public TimeSpan StartReplayTimeout => _settingsReader.Read<TimeSpan>("Bus.Persistence.StartReplayTimeout");
        public bool IsPersistent => _settingsReader.Read<bool>("Bus.IsPersistent");
        public bool IsDirectoryPickedRandomly => _settingsReader.Read<bool>("Bus.Directory.PickRandom");
        public bool IsErrorPublicationEnabled => _settingsReader.Read<bool>("Bus.IsErrorPublicationEnabled");
        public int MessagesBatchSize => _settingsReader.Read<int>("Bus.MessagesBatchSize");
    }
}
