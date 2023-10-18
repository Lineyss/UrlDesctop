using System.Drawing;
using System;
using System.IO;
using System.Text;

namespace UrlDesctopLinux.Models
{
    public class Folder
    {
        public string PathFile { get; set; }
        public string ImageFileName { get; set; }
        public bool IsFoler { get; set; } = true;
        public bool IsImage { get; set; }
        public string TextFile { get; set; }
        public List<FileModel> Files { get; set; } = new List<FileModel>();

        public static readonly bool IsUnix = Environment.OSVersion.VersionString.Split(" ")[0] == "Unix";

        public Folder(string url)
        {
            PathFile = url;
            if(!GetFiles())
            {
                IsFoler = false;
                if (!GetImage())
                {      
                    GetTextInFileAsync();
                }
            }
        }

        private bool GetFiles()
        {
            try
            {
                foreach(var element in Directory.GetDirectories(PathFile))
                {
                    Files.Add(new FileModel(element));
                }

                foreach (var element in Directory.GetFiles(PathFile))
                {
                    Files.Add(new FileModel(element));
                }

                if (IsUnix)
                {
                    for(int i = 0;i< Files.Count;i++)
                    {
                        /*Files[i] = Files[i][1..];*/
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }

        }

        private bool GetImage()
        {
            try
            {
                string exeption = Path.GetExtension(PathFile).ToLower();
                if (exeption == ".png" || exeption == ".web" || exeption == ".jpg" || exeption == ".gif" || exeption == ".svg")
                {
                    IsImage = true;
                    CopyFile();
                    return true;
                }

                throw new Exception();
            }
            catch
            {
                return false;
            }
        }

        private async void GetTextInFileAsync()
        {
            try
            {
                FileReader reader = new FileReader(PathFile);

                TextFile = reader.Read();
            }
            catch
            {
                TextFile = "Не удалось открыть файл";
            }
        }

        private void CopyFile()
        {
            ImageFileName = Path.GetFileName(PathFile);

            string[] ImageFiles = Directory.GetFiles(Directory.GetCurrentDirectory() + $@"\wwwroot\CopyFiles\");

            if (ImageFiles != null)
            {
                foreach (var element in ImageFiles)
                {
                    File.Delete(element);
                }
            }

            string pathToImageDirectory = Directory.GetCurrentDirectory() +  $@"\wwwroot\CopyFiles\{ImageFileName}";


            FileInfo fileInfo = new FileInfo(PathFile);
            fileInfo.CopyTo(pathToImageDirectory);
        }
    }
}
