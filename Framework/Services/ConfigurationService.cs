using Framework.DTOs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Framework.Services
{
    public sealed class ConfigurationService
    {
        private static ConfigurationService _instance;
        public ConfigurationService() => Root = InitializeConfiguration();
        public static ConfigurationService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ConfigurationService();
                }
                return _instance;
            }
        }
        public IConfigurationRoot Root { get; }
        
        public string GetUrlSettings()
        {
            var result = ConfigurationService.Instance.Root.GetSection("urlSettings").GetSection("url").Value;
            if (result == null)
            {
                throw new Exception(typeof(UrlSettings).ToString());
            }
            return result;
        }
        //public WebSettings GetWebSettings()
        //=> ConfigurationService.Instance.Root.GetSection("webSettings").Get<WebSettings>();
        private IConfigurationRoot InitializeConfiguration()
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                var assemblyConfigurationAttribute = typeof(ConfigurationService).Assembly.GetCustomAttribute<AssemblyConfigurationAttribute>();
                var buildConfigurationName = assemblyConfigurationAttribute?.Configuration;
                return SetConfiguration(buildConfigurationName);
            }
            return SetConfiguration();
            //var filesInExecutionDir = Directory.GetFiles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            //var settingsFile =
            //filesInExecutionDir.FirstOrDefault(x => x.Contains("appsettings.QA") && x.EndsWith(".json"));
            //var builder = new ConfigurationBuilder();
            //if (settingsFile != null)
            //{
            //    builder.AddJsonFile(settingsFile, optional: true, reloadOnChange: true);
            //}
            //return builder.Build();
        }
        public IConfigurationRoot SetConfiguration(string config)
        {
            var filesInExecutionDir = Directory.GetFiles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            var settingsFile =
            filesInExecutionDir.FirstOrDefault(x => x.Contains($"appsettings.{config}") && x.EndsWith(".json"));
            var builder = new ConfigurationBuilder();
            if (settingsFile != null)
            {
                builder.AddJsonFile(settingsFile, optional: true, reloadOnChange: true);
            }
            return builder.Build();
        }
        public IConfigurationRoot SetConfiguration()
        {
            var filesInExecutionDir = Directory.GetFiles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            var settingsFile =
            filesInExecutionDir.FirstOrDefault(x => x.Contains($"appsettings") && x.EndsWith(".json"));
            var builder = new ConfigurationBuilder();
            if (settingsFile != null)
            {
                builder.AddJsonFile(settingsFile, optional: true, reloadOnChange: true);
            }
            return builder.Build();
        }
    }
}
