using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ImageGallery.Controllers
{
    public class ImageGalleryController : Controller
    {
        private readonly string rootPath = Path.Combine
            (Directory.GetCurrentDirectory(),"wwwroot//uploads");
        public IActionResult Index()
        {
            //create directory
            bool dirExists = Directory.Exists (rootPath);

            if (!dirExists)
            {
                Directory.CreateDirectory(rootPath);
            }
			List<string> imageNames = Directory.GetFiles(rootPath).Select(Path.GetFileName).ToList();
            return View(imageNames);
        }
        [HttpPost]
        public async Task<IActionResult> Index(IFormFile file)
        {
            if (file != null)
            {
                //get file name
                var path = Path.Combine(rootPath, Guid.NewGuid() +
                    Path.GetExtension(file.FileName));

                //upload file directory
                using (var fs = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(fs);
                }
                return RedirectToAction(nameof(Index));
                //return RedirectToAction("Index");
            }
            return View();
        }
    }
}
