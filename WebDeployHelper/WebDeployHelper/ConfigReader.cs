using System;
using System.IO;

namespace WebDeployHelper
{
    class ConfigReader
    {
        #region Read Config
        
        private readonly string _configDirectory;
        
        private static string _configDirUpload;
        private static string _configReleaseType;
        private static string _configSftpAddress;
        private static string _configSftpUser;
        private static string _configDevPath;
        private static string _configReleasePath;

        private static string _remotePath;

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
            InitRemotePath();
            InitDeployInfo();
            return this;
        }

        public DeployConfig ToDeployConfig()
        {
            var config = new DeployConfig
            {
                ConfigDirUpload = _configDirUpload,
                ConfigReleaseType = _configReleaseType,
                ConfigSftpAddress = _configSftpAddress,
                ConfigSftpUser = _configSftpUser,
                RemotePath = _remotePath
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
                Util.DisplayWarning(TextCollection.ErrorNoConfig, e);
            }

            //index here refers to the line number in DeployHelper.conf
            _configDirUpload = configContent[1];
            _configReleaseType = configContent[3];
            _configSftpAddress = configContent[5];
            _configSftpUser = configContent[7];
            _configDevPath = configContent[9];
            _configReleasePath = configContent[11];
        }

        private void InitRemotePath()
        {
            switch (_configReleaseType)
            {
                case TextCollection.VarRelease:
                    _remotePath = _configReleasePath;
                    break;
                case TextCollection.VarDev:
                    _remotePath = _configDevPath;
                    break;
                default:
                    Util.DisplayWarning(TextCollection.ErrorInvalidReleaseType, new Exception());
                    break;
            }
        }

        private void InitDeployInfo()
        {
            PrintInfo("You are going to deploy PowerPointLabs Website");
            PrintInfo("");
            PrintInfo("Settings info:");
            PrintInfo("Upload Directory: ", _configDirUpload);
            PrintInfo("Release Type: ", _configReleaseType);
            PrintInfo("Remote Path: ", _remotePath);
            PrintInfo("");
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
