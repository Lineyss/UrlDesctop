using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Extensions;
using System.Text;
using UrlDesctopLinux.Models;
using System.IO;
using System.Reflection.Metadata;
using iTextSharp.text.pdf.parser;
using iTextSharp.text;

namespace UrlDesctopLinux.Controllers
{
    public class FileManagerController : Controller
    {
        private UrlWorker urlWorker;
        private readonly ILogger<FileManagerController> logger;
        public FileManagerController(ILogger<FileManagerController> _logger)
        {
            logger = _logger;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            urlWorker = new UrlWorker(HttpContext.Request.GetDisplayUrl());
            string path = urlWorker.GetUrl();
            logger.LogInformation($"Зашли в {DateTime.Now} по пути: {path}");
            return View(new Folder(path));
        }
        [HttpPost]
        public async Task<IActionResult> Index (IFormFile file)
        {
            try
            {
                urlWorker = new UrlWorker(HttpContext.Request.GetDisplayUrl());

                if (file == null)
                {
                    return NotFound("Error upload");
                }

                string path = urlWorker.GetUrl();
                if (Folder.IsUnix)
                {
                    path += file.FileName;
                }
                else
                {
                    path += $"{file.FileName}";
                }
                using(var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return Redirect(HttpContext.Request.GetDisplayUrl());
            }
            catch
            {
                return NotFound("Error upload");
            }
        }
        [HttpPost]
        public IResult CreateFolder()
        {
            try
            {
                urlWorker = new UrlWorker(HttpContext.Request.GetDisplayUrl());

                string parentDirectory = urlWorker.GetParentDirectory();

                Folder.CreateFolder(urlWorker.GetUrl());

                return Results.Ok();
            }
            catch (Exception e)
            {
                return Results.NotFound(e.Message);
            }

        }

        [HttpPost]
        public IResult Delete([FromBody] List<string> listSelectedItem)
        {
            string parentDirectory = "";
            try
            {
                foreach (var element in listSelectedItem)
                {
                    parentDirectory = UrlWorker.GetParentDirectory(element);
                    Folder.DeleteFileOrDirectory(element);  
                }

                return Results.Ok();
            }
            catch(Exception e)
            {
                return Results.NotFound(e.Message);
            }
        }
    }
}