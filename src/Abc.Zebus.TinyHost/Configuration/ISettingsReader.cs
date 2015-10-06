namespace Abc.Zebus.TinyHost.Configuration
{
    public interface ISettingsReader
    {
        T Read<T>(string parameterName);
    }
}