using System.IO;

namespace UrlDesctopLinux.Models
{
    public class FileModel
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string PathToImage { get; set; }
        public DateTime CreateTime { get; set; }
        public long FileSize { get; set; }
        public FileModel(string pathToFile)
        {
            FilePath = pathToFile;
            FileName = Path.GetFileName(pathToFile);

            IsFoler();

            CreateTime = File.GetCreationTime(pathToFile);
        }

        private void IsFoler()
        {
            if ((File.GetAttributes(FilePath) & FileAttributes.Directory) == FileAttributes.Directory)
            {
                PathToImage = "/image/folder.svg";
            }
            else
            {
                PathToImage = "/image/files.svg";
            }
        }
    }
}
