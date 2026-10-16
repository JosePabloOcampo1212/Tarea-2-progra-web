using System.Linq;
using System.Web.Mvc;
using GestorTareas.Data;
using GestorTareas.Models.Dtos;
using GestorTareas.Services;

namespace GestorTareas.Controllers
{
    public class TareasController : Controller
    {
        private ITareaService _servicio;

        private ITareaService Servicio
        {
            get
            {
                if (_servicio == null)
                {
                    _servicio = new TareaService(new TareaSessionRepositorio(Session));
                }
                return _servicio;
            }
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult Listar()
        {
            var datos = new
            {
                Tareas = Servicio.Listar().Select(TareaDto.Desde).ToList()
            };

            return Json(ResultadoOperacion.Ok(null, datos), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Crear(string titulo, string descripcion)
        {
            return Json(Servicio.Crear(titulo, descripcion));
        }
    }
}
