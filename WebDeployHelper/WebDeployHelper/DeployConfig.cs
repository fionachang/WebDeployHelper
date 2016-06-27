using System;

namespace WebDeployHelper
{
    class DeployConfig
    {
        public static readonly string DirConfig = Environment.CurrentDirectory + @"\WebDeployHelper.conf";

        public string ConfigDirUpload;
        public string ConfigReleaseType;
        public string ConfigSftpAddress;
        public string ConfigSftpUser;

        public string RemotePath;
    }
}
