using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Extensions;
using System.Text;
using UrlDesctopLinux.Models;
using System.IO;
using System.Reflection.Metadata;
using iTextSharp.text.pdf.parser;
using iTextSharp.text;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Xml.Linq;
using System;

namespace UrlDesctopLinux.Controllers
{
    public class FileManagerController : Controller
    {
        // Модель для работы с путями
        private UrlWorker urlWorker;
        // Логгер
        private readonly ILogger<FileManagerController> logger;
        // Инициализация контроллера
        public FileManagerController(ILogger<FileManagerController> _logger)
        {
            logger = _logger;
        }
        // Главня функция
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Берем путь с браузера
            urlWorker = new UrlWorker(HttpContext.Request.GetDisplayUrl());
            // Конвертируем его в путь из операционки
            string path = urlWorker.GetUrl();
            // Добавляем в логгер информацию что мы зашли на страницу по определенному пути в определенное время
            logger.LogInformation($"Зашли в {DateTime.Now} по пути: {path}");
            // Передаем класс Folder на страницу
            return View(new Folder(path));
        }
        // Функция для загрузки файлов(а)
        [HttpPost]
        public async Task<IActionResult> Index(List<IFormFile> file)
        {
            try
            {
                // Берем пусть с браузера
                urlWorker = new UrlWorker(HttpContext.Request.GetDisplayUrl());

                // Проверяем передали ли файл(ы)
                if (file == null)
                {
                    return NotFound("Error upload");
                }

                // Получаем путь куда будет сохроняться файл
                string path = urlWorker.GetUrl();
                // Перебираем массив с переданнми файлами(ом)
                foreach (var element in file)
                {
                    // Проверяем на какую операционку устанавливается файл
                    if (Folder.IsUnix)
                    {
                        // Добавляем к пути имя файла
                        path += element.FileName;
                    }
                    else
                    {
                        // Добавляем к пути имя файла
                        path += $"{element.FileName}";
                    }

                    // Асинхронно копируем файл
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await element.CopyToAsync(stream);
                    }
                }

                // Возвращаемся обратно по тому же пути
                return Redirect(HttpContext.Request.GetDisplayUrl());
            }
            catch
            {
                // Возвращем ошибку если что-то пошло не так
                return NotFound("Error upload");
            }
        }

        // Функция скачивания файлов с сервера
        [HttpPost]
        public FileResult Download([FromBody] List<string> listSelectedItem)
        {
            // Проверяем передали ли пустой массив с путями или нет
            if (listSelectedItem == null || listSelectedItem.Count == 0)
            {
                throw new Exception();
            }
            // Создаем класс, который содержит в себе информацию о скачиваемом архиве файлов(е)
            // В нее передаем название архива и массив с путями
            var ZipClass = Folder.CreateZip("MyZip.zip", listSelectedItem);

            // Возвращем пользователю созданный архив
            return File(ZipClass.SizeFile, ZipClass.ContentType, ZipClass.ZipName);
        }


        // Функция для создания директорий
        [HttpPost]
        public IResult CreateFolder()
        {
            try
            {
                // Получаем пусть с браузера
                urlWorker = new UrlWorker(HttpContext.Request.GetDisplayUrl());

                // Создаем по пути директорию
                Folder.CreateFolder(urlWorker.GetUrl());

                // Возврщаем код запроса 200
                return Results.Ok();
            }
            catch (Exception e)
            {
                // Возвращаем в ошибку если что-то пошло не так
                return Results.NotFound(e.Message);
            }

        }

        // Функция удаления файла или папки
        [HttpPost]
        public IResult Delete([FromBody] List<string> listSelectedItem)
        {
            try
            {
                // Перебираем массив с путями до файлов-директорий
                foreach (var element in listSelectedItem)
                {
                    // Удаляем и=
                    Folder.DeleteFileOrDirectory(element);  
                }

                // Возвращаем код запроса 200
                return Results.Ok();
            }
            catch(Exception e)
            {
                // Возвращаем ошибку если что-то пошло не так
                return Results.NotFound(e.Message);
            }
        }
    }
}