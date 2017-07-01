using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Configuration;
using NPA.Common;

namespace InspectionWriterWebApi.Utilities
{

    public class FileUtils : ApiController
    {
        public static async Task<int> MoveFiles()
        {
            int itemCount = 0;
            string sourceImagePath = WebConfigurationManager.AppSettings["ImageFileBasePath"];
            string baseImagePath = WebConfigurationManager.AppSettings["BaseImagePath"];
            foreach (string file in System.IO.Directory.EnumerateFiles(sourceImagePath, "*.*", SearchOption.TopDirectoryOnly))
            {
                string fileName = "";
                try
                {
                    if (Path.GetFileName(file).ToLower() == "thumbs.db")
                    {
                        continue;
                    }
                    fileName = Path.GetFileName(file);
                    File.Copy(file, Path.Combine(baseImagePath, fileName), true);
                    MoveFile(file, Path.Combine(sourceImagePath, "archive", fileName));
                    itemCount++;
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Could not find file")) { return 0; }
                    else if(ex.Message.Contains("it is being used by another process"))
                    {
                        await Task.Delay(180000);
                        await MoveFiles();
                        return 0;
                    }
                    else
                    {
                        //string errorRecipients = NPA.Core.ConfigurationMgr.GetSetting("DevTeam", "wsteed@npauctions.com");
                        string errorRecipients = "wsteed@npauctions.com";

                        var sourceId = new Guid(WebConfigurationManager.AppSettings["EmailNotificationSourceId"]);
                        var typeId = new Guid("C99B35B4-4388-4B68-8F16-4CFB34336B4D"); //email

                        //Email.SendNotification(null, sourceId, typeId, errorRecipients, "FileCopyAgent@npauctions.com",
                        //    null, null, "Error copying file", "Error copying " + file + ":" + Environment.NewLine + ex.Message, null);
                        LogUtil.WriteLogEntry("PictureAgent", "Error moving file " + fileName + "." 
                            + Environment.NewLine + ex.ToString(), System.Diagnostics.EventLogEntryType.Error);
                    }
                    
                }
            }
            return itemCount;
        }

        private static void MoveFile(string source, string destination)
        {
            File.Move(source, destination);
        }

    }
}