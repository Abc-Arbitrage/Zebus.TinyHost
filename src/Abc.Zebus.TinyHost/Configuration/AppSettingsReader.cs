using System.Configuration;
using log4net;

namespace Abc.Zebus.TinyHost.Configuration
{
    public class AppSettingsReader : ISettingsReader
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(AppSettingsReader));

        public T Read<T>(string parameterName)
        {
            return (T)ConfigurationValueConverter.ConvertValue(GetValue(parameterName), typeof(T));
        }

        private string GetValue(string settingKey)
        {
            var appSetting = ConfigurationManager.AppSettings[settingKey];
            if (appSetting != null)
            {
                _logger.Debug($"Using App.config settings: Key: {settingKey} Value: {appSetting} !");
                return appSetting;
            }

            var connectionStringSettings = ConfigurationManager.ConnectionStrings[settingKey];
            if (connectionStringSettings != null)
            {
                _logger.Debug($"Using App.config settings: Key: {settingKey} Value: {connectionStringSettings.ConnectionString} !");
                return connectionStringSettings.ConnectionString;
            }

            return null;
        }
    }
}