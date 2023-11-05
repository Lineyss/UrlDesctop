using System.IO;

namespace UrlDesctopLinux.Models
{
    // Модель которая выводится на страницу
    public class FileModel
    {
        // Название файла
        public string FileName { get; set; }
        // Путь до файла
        public string FilePath { get; set; }
        // Путь до картинки
        public string PathToImage { get; set; }
        // Время создания файла
        public DateTime CreateTime { get; set; }


        public FileModel(string pathToFile)
        {
            // Инициализация путь и имени файла
            FilePath = pathToFile;
            FileName = Path.GetFileName(pathToFile);

            // Получение пути до картинки
            IsFoler();

            // Получение времени создания файла
            CreateTime = File.GetCreationTime(pathToFile);
        }

        private void IsFoler()
        {
            // Проверка если путь до папки
            if ((File.GetAttributes(FilePath) & FileAttributes.Directory) == FileAttributes.Directory)
            {
                // Выдаем путь до картинки папкм
                PathToImage = "/image/folder.svg";
            }
            else
            {
                // Выдаем путь до кратинки файла
                PathToImage = "/image/files.svg";
            }
        }
    }
}
