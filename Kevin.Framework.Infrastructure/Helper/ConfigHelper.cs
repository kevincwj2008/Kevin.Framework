using System;
using System.Configuration;

namespace Kevin.Framework.Infrastructure
{
    public class ConfigHelper
    {
        public static string Get(string key)
        {
            return ConfigurationManager.AppSettings[key] ?? string.Empty;
        }

        //public static T Get<T>(string key)
        //{
        //    string value = ConfigurationManager.AppSettings[key];
        //    if (!string.IsNullOrEmpty(value))
        //    {
        //        try
        //        {
        //            defaultValue = (T)Convert.ChangeType(value, typeof(T));
        //        }
        //        catch
        //        {
        //        }
        //    }
        //    return defaultValue;
        //}

        public static void UpdateAppSettings(string key, string value)
        {
            var _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (!_config.HasFile)
                throw new ArgumentException("程序配置文件缺失！");
            KeyValueConfigurationElement _key = _config.AppSettings.Settings[key];
            if (_key == null)
                _config.AppSettings.Settings.Add(key, value);
            else
                _config.AppSettings.Settings[key].Value = value;
            _config.Save(ConfigurationSaveMode.Modified);
        }
    }
}
