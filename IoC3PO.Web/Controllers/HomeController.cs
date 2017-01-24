using System.Web.Mvc;

namespace IoC3PO.Web.Controllers
{
    public interface IDroid
    {
        string Name { get; set; }
        string Type { get; set; }
    }

    public class Artoo : IDroid {
        public string Name { get; set; }
        public string Type { get; set; }
    }

    public interface ISandCrawler
    {
        IDroid GetDroid();
    }

    public class SandCrawler : ISandCrawler
    {
        private readonly IDroid _droid;

        public SandCrawler(IDroid droid)
        {
            _droid = droid;
            _droid.Name = "R2-D2";
            _droid.Type = "R2";
        }

        public IDroid GetDroid()
        {
            return _droid;
        }
    }

    public class HomeController : Controller
    {
        private readonly ISandCrawler _crawler;

        public HomeController(ISandCrawler crawler)
        {
            _crawler = crawler;
        }
        public ActionResult Index()
        {
            var droid = _crawler.GetDroid();
            ViewBag.DroidYoureLookingFor = droid;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}