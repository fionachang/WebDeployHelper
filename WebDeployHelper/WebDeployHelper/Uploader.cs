using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;

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

        public void Upload()
        {
            var winscp = new Process();
            winscp.StartInfo.FileName = "winscp.com";
            winscp.StartInfo.Arguments = "/xmllog=\"" + TextCollection.FileXmlLogName + "\"";
            winscp.StartInfo.UseShellExecute = false;
            winscp.StartInfo.CreateNoWindow = true;
            winscp.StartInfo.RedirectStandardInput = true;
            winscp.Start();

            var writer = winscp.StandardInput;

            WriteUploadCommands(writer);

            writer.Close();
            winscp.WaitForExit();

            CheckForError();
        }

        private void WriteUploadCommands(StreamWriter writer)
        {
            writer.WriteLine("open sftp://" + Config.ConfigSftpUser + "@" + Config.ConfigSftpAddress + ":22 -hostkey=\"*\"");

            string password = null;
            while (password == null || password.Trim() == "")
            {
                Console.Write(TextCollection.InfoEnterPassword);
                password = Util.ReadPassword();
            }
            writer.WriteLine(password);
            password = null;

            writer.WriteLine("option confirm off");
            writer.WriteLine("put -permissions=" + TextCollection.PermissionsFile + " -transfer=binary \"" + Config.ConfigDirUpload
                + "\" \"" + Config.RemotePath + "\"");

            var directoryInfos = new DirectoryInfo(Config.ConfigDirUpload).EnumerateDirectories("*", SearchOption.AllDirectories);

            foreach (var directoryInfo in directoryInfos)
            {
                string remoteDirectoryPath = Config.RemotePath + directoryInfo.FullName.Replace(Config.ConfigDirUpload, "").Replace("\\", "/");

                writer.WriteLine("chmod " + TextCollection.PermissionsDirectory + " \"" + remoteDirectoryPath + "\"");
            }
            
            writer.WriteLine("exit");
        }

        private void CheckForError()
        {
            var log = new XPathDocument(TextCollection.FileXmlLogName);
            var xmlNamespace = new XmlNamespaceManager(new NameTable());
            xmlNamespace.AddNamespace("w", "http://winscp.net/schema/session/1.0");
            var navigator = log.CreateNavigator();
            var iterator = navigator.Select("//w:message", xmlNamespace);

            if (iterator.Count > 0)
            {
                var errorMessage = new StringBuilder();
                foreach (XPathNavigator message in iterator)
                {
                    errorMessage.AppendLine(message.Value);
                }

                throw new Exception(errorMessage.ToString());
            }
            else
            {
                Console.WriteLine(TextCollection.DoneUploaded);
            }
        }
    }
}
