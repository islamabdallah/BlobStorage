using BlobStorageDemo2.Models;
using BlobStorageDemo2.Services.contracts;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BlobStorageDemo2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IUploadService _uploadService;

        public HomeController(ILogger<HomeController> logger,
            IUploadService uploadService)
        {
            _logger = logger;
            _uploadService = uploadService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddFile(IFormFile tesFile)
        {
          var result =   _uploadService.UploadAsync(tesFile.OpenReadStream(), tesFile.FileName, tesFile.ContentType);
            return View(result);
        }


        public string getBlobURL()
        {
            try
            {
                string fileURL =_uploadService.GetBlobSASTOkenByFile("").Result;
                return fileURL;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}