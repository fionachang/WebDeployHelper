using System;
using System.IO;

namespace WebDeployHelper
{
    class ConfigReader
    {
        #region Read Config

        private readonly string _currentDirectory;
        private readonly string _configDirectory;
        
        private static string _configDirUpload;
        private static string _configReleaseType;
        private static string _configSftpAddress;
        private static string _configSftpUser;
        private static string _configDevPath;
        private static string _configReleasePath;

        public ConfigReader()
        {
            _configDirectory = DeployConfig.DirConfig;
        }

        public ConfigReader(string configDirectory, string vstoDirectory)
        {
            _configDirectory = configDirectory;
        }

        public ConfigReader ReadConfig()
        {
            //Sequence matters
            InitConfigVariables();
            InitDeployInfo();
            return this;
        }

        public override string ToString()
        {
            return
                "ConfigDirUpload: " + _configDirUpload + "\r\n" +
                "ConfigReleaseType: " + _configReleaseType + "\r\n" +
                "ConfigSftpAddress: " + _configSftpAddress + "\r\n" +
                "ConfigSftpUser: " + _configSftpUser + "\r\n" +
                "ConfigDevPath: " + _configDevPath + "\r\n" +
                "ConfigReleasePath: " + _configReleasePath + "\r\n";
        }

        public DeployConfig ToDeployConfig()
        {
            var config = new DeployConfig
            {
                ConfigDirUpload = _configDirUpload,
                ConfigReleaseType = _configReleaseType,
                ConfigSftpAddress = _configSftpAddress,
                ConfigSftpUser = _configSftpUser,
                ConfigDevPath = _configDevPath,
                ConfigReleasePath = _configReleasePath
            };
            return config;
        }

        private void InitConfigVariables()
        {
            string[] configContent = {};
            try
            {
                configContent = File.ReadAllLines(_configDirectory);
            }
            catch (Exception e)
            {
                Util.DisplayWarning(TextCollection.Const.ErrorNoConfig, e);
            }

            //index here refers to the line number in DeployHelper.conf
            _configDirUpload = configContent[1];
            _configReleaseType = configContent[3];
            _configSftpAddress = configContent[5];
            _configSftpUser = configContent[7];
            _configDevPath = configContent[9];
            _configReleasePath = configContent[11];
        }

        private void InitDeployInfo()
        {
            PrintInfo("You are going to deploy PowerPointLabs Website\r\n");
            Console.Write("Settings info:\n");
            PrintInfo("Upload Directory: ", _configDirUpload);
            PrintInfo("Release Type: ", _configReleaseType);
            PrintInfo("Dev Path: ", _configDevPath);
            PrintInfo("Release Path: ", _configReleasePath);
        }

        private void PrintInfo(string text, string highlightedText = "")
        {
            Console.Write(text);
            Util.ConsoleWriteWithColor(highlightedText, ConsoleColor.Yellow);
            Console.WriteLine("");
        }
        #endregion
    }
}
