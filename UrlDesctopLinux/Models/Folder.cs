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
        public bool IsImage { get; set; } = false;
        public string TextFile { get; set; }
        public List<string>? Files { get; set; } = null;

        private readonly bool IsUnix = Environment.OSVersion.VersionString.Split(" ")[0] == "Unix";

        public Folder(string url)
        {
            PathFile = url;
            if(!GetFiles())
            {
                if(!GetImage())
                {
                    GetTextInFile();
                }
            }
        }

        private bool GetFiles()
        {
            try
            {
                Files = Directory.GetDirectories(PathFile).ToList();
                Files = Files.Concat(Directory.GetFiles(PathFile)).ToList();

                if(IsUnix)
                {
                    for(int i = 0;i< Files.Count;i++)
                    {
                        Files[i] = Files[i][1..];
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
                    CopyFile(PathFile);
                    return true;
                }

                throw new Exception();
            }
            catch
            {
                return false;
            }
        }

        private void GetTextInFile()
        {
            try
            {
                TextFile = File.ReadAllText(PathFile);
            }
            catch
            {
                TextFile = "Не удалось открыть файл";
            }
        }

        private void CopyFile(string url)
        {
            ImageFileName = Path.GetFileName(url);

            string[] ImageFiles = Directory.GetFiles(Directory.GetCurrentDirectory() + $@"\wwwroot\CopyFiles\");

            if (ImageFiles != null)
            {
                foreach (var element in ImageFiles)
                {
                    File.Delete(element);
                }
            }

            string pathToImageDirectory = Directory.GetCurrentDirectory() +  $@"\wwwroot\CopyFiles\{ImageFileName}";


            FileInfo fileInfo = new FileInfo(url);
            fileInfo.CopyTo(pathToImageDirectory);
        }
    }
}
