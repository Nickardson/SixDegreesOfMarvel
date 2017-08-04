using System.Web.Mvc;

namespace SixDegreesOfMarvel.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult ChainFinder()
        {
            return View();
        }
    }
}
