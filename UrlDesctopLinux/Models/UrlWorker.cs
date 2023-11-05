using System;
using System.Reflection.Metadata;

namespace UrlDesctopLinux.Models
{
    // Класс для работы с браузерными путями и конвертация их в системные
    public class UrlWorker
    {
        // Пусть с котором нужно будет работать
        private string currentUrl;
        public UrlWorker(string Url)
        {
            // Инициализация пути
            currentUrl = Url;
        }

        // Статический метод для получения родительской директории
        public static string? GetParentDirectory(string Url)
        {
            // Проверяем передали ли пустую строку
            if(!String.IsNullOrWhiteSpace(Url))
            {
                // Возврщаем родительскую директорию
                return Directory.GetParent(Url)?.Name;
            }

            // Или ничего
            return null;
        }

        // Перегрузка метода получения родительской директории
        public string? GetParentDirectory()
        {
            return Directory.GetParent(currentUrl)?.Name;
        }

        // Метод для получения системного пути
        public string GetUrl()
        {
            // Проверяем операционную систему
            return Folder.IsUnix ? GetUrlLinux() : GetUrlWindows();
        }
        
        // Метод для получения системных путей в Windows
        private string GetUrlWindows()
        {
            // Получем массив пути
            List<string> array = RemoveUslessWords();
            // Проверяем заполненость массива
            if (array.Count == 0)
            {
                // Если пуст то переходим в системный диск
                array.Add("C:/");
            }
            else if(array[0] == "")
            {
                // Если первый элемент пустой, то меняем его на системный диск
                array[0] = "C:/";
            }
            // Иначе возвращаем полученный путь в виде строки
            return string.Join("/", array);
        }

        // Метод для получсения системных путей в Unix системах
        private string GetUrlLinux()
        {
            // Получаем массив пути
            List<string> array = RemoveUslessWords();
            // Проверяем заполненость массива
            if (array[0] == "" || array.Count == 0)
            {
                // Если пуст то переходим в главную директорию
                return "/";
            }
            // Конвертируем массив пути в строку
            currentUrl = string.Join("/",array); 

            // Проверяем до чего вудет путь и изменяем его
            if(Path.HasExtension(currentUrl))
            {
                return $"/{currentUrl}";
            }
            return $"/{currentUrl}/";
        }

        // Метод для удаления не нужных слов в пути
        private List<string> RemoveUslessWords()
        {
            // Сплитим путь в массив
            List<string> arr = currentUrl.Split("/").ToList();

            // Удаляем не нужные слова
            arr.RemoveAt(0);
            arr.RemoveAt(0);
            arr.RemoveAt(0);
            arr.RemoveAt(0);

            // Возвращаем массив
            return arr;
        }
    }
}
