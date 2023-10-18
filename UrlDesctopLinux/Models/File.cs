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
            PathToImage = Path.HasExtension(pathToFile)? "/image/folder.svg" : "/image/files.svg";
            CreateTime = File.GetCreationTime(pathToFile);
        }
    }
}
