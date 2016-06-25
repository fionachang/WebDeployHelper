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
        public string ConfigDevPath;
        public string ConfigReleasePath;

        public void VerifyConfig()
        {
            if (ConfigReleaseType != "release" && ConfigReleaseType != "dev")
            {
                Util.DisplayWarning(TextCollection.Const.ErrorInvalidConfig + " Release type not correct.", new Exception());
            }
        }
    }
}
