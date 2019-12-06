using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WithSalt.Common.Json;

namespace DNCS.Config
{
    public class ConfigManager
    {
        private static ConfigManager _manager = null;

        /// <summary>
        /// 自动从文件加载配置文件
        /// </summary>
        /// <param name="configFileName">配置文件名称，位于程序根目录，默认使用appsettings.json</param>
        /// <returns></returns>
        public static bool LoadConfig(string configFileName = "")
        {
            try
            {
                string configPath = $"{AppContext.BaseDirectory}appsettings.json";
                if (!string.IsNullOrEmpty(configFileName))
                {
                    configPath = $"{AppContext.BaseDirectory}{configFileName}";
                }
                if (!File.Exists(configPath))
                {
                    return false;
                }
                string loadConfigString = File.ReadAllText(configPath);
                if (string.IsNullOrEmpty(loadConfigString))
                {

                    return false;
                }
                ConfigManager config = JsonHelper.DeserializeJsonToObject<ConfigManager>(loadConfigString);
                //Set now value
                Now = config;
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Load config failed. {ex.Message}");
            }
        }

        public static ConfigManager Now
        {
            get
            {
                return _manager;
            }
            set
            {
                if (value != null)
                {
                    _manager = value;
                }
                else
                {
                    throw new Exception("Config setting object can not null.");
                }
            }
        }


        public ConnectionStringsNode ConnectionStrings { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public LoggingNode Logging { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AllowedHosts { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public AppSettingsNode AppSettings { get; set; }
    }
}
