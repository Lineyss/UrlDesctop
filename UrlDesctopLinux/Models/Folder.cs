using System.Drawing;
using System;
using System.IO;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using System.Diagnostics;
using ICSharpCode.SharpZipLib.Core;
using iTextSharp.text;

namespace UrlDesctopLinux.Models
{
    // Класс для работы с файлами и директориями
    public class Folder
    {
        // Класс для zip архива,в котором храняться скачиные файлы
        public class ZipFolder
        {
            // Размер архива в байтах
            public readonly byte[] SizeFile;
            // Тип архива
            public readonly string ContentType = "application/zip";
            // Название архива
            public readonly string ZipName;
            public ZipFolder(string ZipName, byte[] SizeFile)
            {
                // Инициализация переменных
                this.ZipName = ZipName;
                this.SizeFile = SizeFile;
            }
        }


        // Путь до файла
        public string PathFile { get; set; }
        // Название картинки
        public string ImageFileName { get; set; }
        // Является ли файл папкой
        public bool IsFoler { get; set; }
        // Является ли файл картинкой
        public bool IsImage { get; set; }
        // Текст файла
        public string TextFile { get; set; }
        // Массив всех файлов в пути
        public List<FileModel> Files { get; set; } = new List<FileModel>();
        // Какая операционная система используется
        public static readonly bool IsUnix = Environment.OSVersion.VersionString.Split(" ")[0] == "Unix";

        public Folder(string url)
        {
            // Получем системный путь
            PathFile = url;
            // Узнаем путь до директории или нет
            IsFoler = GetFiles();
            if(!IsFoler)
            {
                // Узнаем путь до картинки или нет
                IsImage = GetImage();
                if(!IsImage)
                {
                    // Получаем текст (если можем)
                    TextFile = GetTextInFileAsync();
                }
            }
        }

        // Метод для получения всех файлов и директорий из папки
        private bool GetFiles()
        {
            try
            {
                // Получение всех файлов и директорий
                foreach (var element in Directory.GetDirectories(PathFile))
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

                // Возврщаем true если не было ошибок
                return true;
            }
            catch
            {
                // Возвращаем false если были ошибки
                return false;
            }

        }

        // Метод для провекри ведет ли путь до картинки
        private bool GetImage()
        {
            try
            {
                // Получаем расширение файла
                string exeption = Path.GetExtension(PathFile).ToLower();
                // Проверяем какой формат был получен
                if (exeption == ".png" || exeption == ".web" || exeption == ".jpg" || exeption == ".gif" || exeption == ".svg")
                {
                    // Копирование картинки во временную папку
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

        // Метод для получения текста из файла
        private string GetTextInFileAsync()
        {
            try
            {
                // Объявляем класс который считывает текст с файла
                FileReader reader = new FileReader(PathFile);

                // Возвращаем полученный текст
                return reader.Read();
            }
            catch
            {
                // Возвращаем в случае ошибки
                return "Не удалось открыть файл";
            }
        }

        // Метод для копирования файла во временнную папку
        private void CopyFile()
        {
            // Получаем имя файла
            ImageFileName = Path.GetFileName(PathFile);

            // Получаем массив всех файлов во временной папке
            string[] ImageFiles = Directory.GetFiles(Directory.GetCurrentDirectory() + $@"\wwwroot\CopyFiles\");

            // Очищаем папку
            ClearFolder(ImageFiles);

            // Получаем полный путь до временного файла с файлом внутри
            string pathToImageDirectory = Directory.GetCurrentDirectory() + $@"\wwwroot\CopyFiles\{ImageFileName}";


            // Копируем файл во временную папку
            FileInfo fileInfo = new FileInfo(PathFile);
            fileInfo.CopyTo(pathToImageDirectory);
        }

        // Метод удаления файлов
        public static bool ClearFolder(string[] Files)
        {
            try
            {
                // Перебираем массив с файлами
                foreach (var element in Files)
                {
                    // Удаляем их
                    File.Delete(element);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        // Метод создания диреткории
        public static void CreateFolder(string path)
        {
            // Проверяем существует ли такая папка
            if (Directory.Exists(path))
            {
                throw new Exception("Папка уже существует");
            }
            else
            {
                // Емли нет то создаем ее
                Directory.CreateDirectory(path);
            }
        }

        // Удаление файла или директории
        public static void DeleteFileOrDirectory(string path)
        {
            // Проверка на файл или директорию
            if(File.Exists(path))
            {
                // Удаление файла
                File.Delete(path);
            }
            else
            {
                //  Удаление директории
                Directory.Delete(path, true);
            }
        }

        // Метод создания zip архивыа
        public static ZipFolder CreateZip(string ZipName, List<string> BootFiles)
        {
            // Получаем полный путь до временной папки с архивом
            string ZipPath = Directory.GetCurrentDirectory() + $@"\wwwroot\Downloads\{ZipName}";

            // Очистка временной папки
            Folder.ClearFolder(Directory.GetFiles(Directory.GetCurrentDirectory() + $@"\wwwroot\Downloads\"));

            // Создание архива
            using (ZipOutputStream zipOutputStream = new ZipOutputStream(System.IO.File.Create(ZipPath)))
            {
                // Установка уровня сжатия
                zipOutputStream.SetLevel(9);

                CreateEntryZip(BootFiles,zipOutputStream);

                // Завершение и закрытие потока
                zipOutputStream.Finish();
                zipOutputStream.Flush();
                zipOutputStream.Close();
            }

            byte[] finalResult = File.ReadAllBytes(ZipPath);
            if (finalResult == null)
            {
                throw new Exception();
            }

            return new ZipFolder(ZipName, finalResult);
        }

        // Метод создания элемента zip архива
        private static void CreateEntryZip(List<string> BootFiles, ZipOutputStream zipOutputStream, string? folderName = null)
        {
            foreach(var element in BootFiles)
            {
                // Проверка, является ли переденный элемент папкой или нет
                if (folderName == null)
                {
                    // Записываем имя папки в переменную
                    folderName = Path.GetFileName(element);
                    // Создаем элемент с таким же названием
                    ZipEntry entry = new ZipEntry(ZipEntry.CleanName(folderName + "/"));
                    entry.DateTime = DateTime.Now;
                    entry.IsUnicodeText = true;
                    // Добавляем элемент в архив
                    zipOutputStream.PutNextEntry(entry);
                    // Получаем массив всех файлов и директорий
                    List<string> files = Directory.GetFiles(element, "*", SearchOption.AllDirectories).ToList();

                    // Вызываем ее еще раз
                    CreateEntryZip(files, zipOutputStream, folderName);
                }
                else
                {
                    // Записываем название файла
                    string fileName = Path.GetFileName(element);
                    // Создаем элемент с таким же названием
                    ZipEntry entry = new ZipEntry(ZipEntry.CleanName(folderName + "/" + fileName));
                    entry.DateTime = DateTime.Now;
                    entry.IsUnicodeText = true;
                    // Добавляем элемент в архив
                    zipOutputStream.PutNextEntry(entry);

                    // Копируем файл в архим
                    CopyFile(element, zipOutputStream);
                }
            }
        }

        // Метод копирования файла в zip архив
        private static void CopyFile(string pathToFile, ZipOutputStream zipOutputStream)
        {
            // Выделение буферной памяти
            byte[] buffer = new byte[4096];

            // Чтение передаваемого в архив файла
            using (FileStream fileStream = File.OpenRead(pathToFile))
            {
                // Перенос файла в архив по байтово
                int sourceBytes;
                do
                {
                    sourceBytes = fileStream.Read(buffer, 0, buffer.Length);
                    zipOutputStream.Write(buffer, 0, sourceBytes);
                } while (sourceBytes > 0);
            }
        }
    }
}
