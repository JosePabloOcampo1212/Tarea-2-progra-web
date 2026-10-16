using System;
using System.Linq;
using System.Web.Mvc;
using GestorTareas.Data;
using GestorTareas.Models;
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
        public JsonResult Listar(string filtro)
        {
            if (!Enum.TryParse(filtro, true, out FiltroTareas filtroElegido))
            {
                filtroElegido = FiltroTareas.Todas;
            }

            var datos = new
            {
                Tareas = Servicio.Listar(filtroElegido).Select(TareaDto.Desde).ToList(),
                Resumen = Servicio.ObtenerResumen()
            };

            return Json(ResultadoOperacion.Ok(null, datos), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Crear(string titulo, string descripcion)
        {
            return Json(Servicio.Crear(titulo, descripcion));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Editar(int id, string titulo, string descripcion)
        {
            return Json(Servicio.Editar(id, titulo, descripcion));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CambiarEstado(int id)
        {
            return Json(Servicio.CambiarEstado(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Eliminar(int id)
        {
            return Json(Servicio.Eliminar(id));
        }
    }
}
