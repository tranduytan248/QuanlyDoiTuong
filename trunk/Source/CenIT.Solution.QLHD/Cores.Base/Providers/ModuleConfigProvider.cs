using Cores.Base.Helpers;

namespace Cores.Base.Providers
{
    public static class ModuleConfigProvider
    {
        public static string GetConfigForModule(string configFilePath, string configKey)
        {
            if (string.IsNullOrEmpty(configFilePath) || string.IsNullOrEmpty(configKey)) return string.Empty;

            var dataConfigs = ConfigHelper.GetSettingsByPath(configFilePath, "ModuleSettings");
            if (dataConfigs == null || dataConfigs.Count == 0) return string.Empty;
            var configValue = dataConfigs[configKey]?.ToString();
            return configValue;
        }
    }
}