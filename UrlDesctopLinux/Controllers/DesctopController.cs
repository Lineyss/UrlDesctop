using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Extensions;
using System.Text;
using UrlDesctopLinux.Models;
using System.IO;
using System.Reflection.Metadata;

namespace UrlDesctopLinux.Controllers
{
    public class DesctopController : Controller
    {
        private readonly ILogger<DesctopController> logger;
        public DesctopController(ILogger<DesctopController> _logger)
        {
            logger = _logger;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            UrlWorker urlWorker = new UrlWorker(HttpContext.Request.GetDisplayUrl());
            string path = urlWorker.GetUrl();
            logger.LogInformation($"Зашли в {DateTime.Now} по пути: {path}");
            return View(new Folder(path));
        }
        [HttpPost]
        public async Task<IActionResult> Index (IFormFile file)
        {
            try
            {
                UrlWorker urlWorker = new UrlWorker(HttpContext.Request.GetDisplayUrl());

                if (file == null)
                {
                    return NotFound("Error upload");
                }

                string path = urlWorker.GetUrl();
                path += $"\\{file.FileName}";
                using(var stream = new FileStream(path,FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return Redirect((HttpContext.Request.GetDisplayUrl()));
            }
            catch
            {
                return NotFound("Error upload");
            }
        }
    }
}
