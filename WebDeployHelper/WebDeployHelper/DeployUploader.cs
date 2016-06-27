using System;

namespace WebDeployHelper
{
    class DeployUploader
    {
        #region SFTP upload

        private readonly DeployConfig _config;

        public DeployUploader(DeployConfig config)
        {
            _config = config;
        }

        public void SftpUpload()
        {
            try
            {
                Console.WriteLine("Connecting the server...");
                var uploader = new Uploader();
                uploader.SetConfig(_config);
                uploader.Upload();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error during SFTP uploading:");
                Util.DisplayWarning(e.Message, e);
            }
        }

        #endregion
    }
}
