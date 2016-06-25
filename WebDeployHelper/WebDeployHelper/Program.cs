using System;
#region WebDeployHelper Description
//
//  WebDeployHelper Class
//  ------------------
//  Simply double click the .exe file to patch PowerPointLabs Website, 
//  so that it uploads the files onto the PowerPointLabs server.
//
//  HOW TO USE
//
//  For the first time use, you need to setup the followings:
//
//  0. Compile WebDeployHelper using Visual Studio. .NET 4.5 is required. The output program is under bin/debug or bin/release folder.
//
//  1. Fill in WebDeployHelper.conf
//  - Upload directory is the local directory to upload to the server
//  - Release type is dev or release
//  - SFTP address is the server to upload to
//  - SFTP username is the username used to login the server
//  - Dev path is the installation folder path on the server for dev version PowerPointLabs Website
//  - Release path is the installation folder path on the server for release version PowerPointLabs Website
//
//  2. Copy WebDeployHelper.exe, WebDeployHelper.conf, WinSCP.exe and WinSCPnet.dll from the output folder to the publish folder
//
//  3. Run WebDeployHelper.exe and follow the instructions.
//
//  For the next time
//
//  0. Run WebDeployHelper.exe and follow the instructions.
//
//  Have a nice day :)
//
//TODO: add testing
#endregion
namespace WebDeployHelper
{
    class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                # region Init

                var config = new ConfigReader()
                    .ReadConfig()
                    .ToDeployConfig();
                config.VerifyConfig();

                # endregion
                
                new DeployUploader(config)
                    .SftpUpload();
                Util.DisplayEndMessage();
            }
            catch
            {
                Util.IgnoreException();
            }
            finally
            {
                Console.ReadKey();
            }
        }
    }
}
