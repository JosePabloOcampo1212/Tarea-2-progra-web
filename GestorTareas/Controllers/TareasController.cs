using System.Web.Mvc;

namespace GestorTareas.Controllers
{
    public class TareasController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}
