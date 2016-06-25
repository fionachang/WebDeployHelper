using System;
using WinSCP;

namespace WebDeployHelper
{
    class Uploader
    {
        private const bool IsToRemoveAfterUpload = false;

        protected DeployConfig Config;

        public void SetConfig(DeployConfig config)
        {
            Config = config;
        }

        public void UploadLocalDirectory(Session session, string remotePath, TransferOptions transferOptions)
        {
            // Construct folder with permissions first
            try
            {
                session.SynchronizeDirectories(SynchronizationMode.Remote, Config.ConfigDirUpload, remotePath, IsToRemoveAfterUpload,
                    options: transferOptions);
            }
            catch (InvalidOperationException)
            {
                if (session.Opened)
                {
                    Util.IgnoreException();
                }
                else
                {
                    throw;
                }
            }

            var synchronizeResult = session.SynchronizeDirectories(SynchronizationMode.Remote, Config.ConfigDirUpload, remotePath,
                IsToRemoveAfterUpload, options: transferOptions);
            synchronizeResult.Check();
        }

        public TransferOptions SetupTransferOptions()
        {
            var transferOptions = new TransferOptions { TransferMode = TransferMode.Binary };
            var permissions = new FilePermissions { Octal = "644" };
            transferOptions.FilePermissions = permissions;
            return transferOptions;
        }

        public string SetupRemotePath()
        {
            var versionToUpload = Config.ConfigReleaseType;
            switch (versionToUpload)
            {
                case TextCollection.Const.VarDev:
                    return Config.ConfigDevPath;
                case TextCollection.Const.VarRelease:
                    return Config.ConfigReleasePath;
                default:
                    Util.DisplayWarning("Incorrect release type!", new Exception());
                    //dummy return, won't reach
                    return Config.ConfigDevPath;
            }
        }
        
        public SessionOptions GetSessionOptions()
        {
            string password = null;
            while (password == null || password.Trim() == "")
            {
                Console.Write(TextCollection.Const.InfoEnterPassword);
                password = Util.ReadPassword();
            }

            var sessionOptions = new SessionOptions
            {
                Protocol = Protocol.Sftp,
                HostName = Config.ConfigSftpAddress,
                UserName = Config.ConfigSftpUser,
                Password = password,
                PortNumber = 22, //TODO: make it configurable
                GiveUpSecurityAndAcceptAnySshHostKey = true
            };
            return sessionOptions;
        }
    }
}
