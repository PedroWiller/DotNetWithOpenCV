using Bride_Frankenstein.Services;
using FrankenStain.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using t = Bride_Frankenstein;

namespace FrankenStain.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            new  t.Program().Main2();
            var repo = new BaseRepository();
            
            var data  = repo.Find<BaseModel>("SELECT * FROM NovoTest");

            return View(data);
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
