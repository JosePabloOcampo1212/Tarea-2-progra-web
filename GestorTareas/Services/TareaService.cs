using System;
using System.Collections.Generic;
using System.Linq;
using GestorTareas.Data;
using GestorTareas.Models;
using GestorTareas.Models.Dtos;

namespace GestorTareas.Services
{
    public class TareaService : ITareaService
    {
        public const int MaximoTitulo = 100;

        public const int MaximoDescripcion = 500;

        private const string MensajeNoExiste = "La tarea no existe o ya fue eliminada.";

        private readonly ITareaRepositorio _repositorio;

        public TareaService(ITareaRepositorio repositorio)
        {
            _repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public IList<Tarea> Listar(FiltroTareas filtro)
        {
            List<Tarea> tareas = _repositorio.ObtenerTodas();

            switch (filtro)
            {
                case FiltroTareas.Pendientes:
                    return tareas.Where(t => !t.Completada).ToList();

                case FiltroTareas.Completadas:
                    return tareas.Where(t => t.Completada).ToList();

                default:
                    return tareas;
            }
        }

        public ResumenTareas ObtenerResumen()
        {
            List<Tarea> tareas = _repositorio.ObtenerTodas();
            int completadas = tareas.Count(t => t.Completada);

            return new ResumenTareas
            {
                Total = tareas.Count,
                Completadas = completadas,
                Pendientes = tareas.Count - completadas
            };
        }

        public ResultadoOperacion Crear(string titulo, string descripcion)
        {
            string error = Validar(titulo, descripcion);
            if (error != null)
            {
                return ResultadoOperacion.Error(error);
            }

            var tarea = new Tarea
            {
                Titulo = titulo.Trim(),
                Descripcion = descripcion.Trim(),
                Completada = false,
                FechaCreacion = DateTime.Now
            };

            _repositorio.Agregar(tarea);

            return ResultadoOperacion.Ok("Tarea creada.", TareaDto.Desde(tarea));
        }

        public ResultadoOperacion Editar(int id, string titulo, string descripcion)
        {
            Tarea tarea = _repositorio.ObtenerPorId(id);
            if (tarea == null)
            {
                return ResultadoOperacion.Error(MensajeNoExiste);
            }

            string error = Validar(titulo, descripcion);
            if (error != null)
            {
                return ResultadoOperacion.Error(error);
            }

            tarea.Titulo = titulo.Trim();
            tarea.Descripcion = descripcion.Trim();
            _repositorio.Actualizar(tarea);

            return ResultadoOperacion.Ok("Tarea actualizada.", TareaDto.Desde(tarea));
        }

        public ResultadoOperacion CambiarEstado(int id)
        {
            Tarea tarea = _repositorio.ObtenerPorId(id);
            if (tarea == null)
            {
                return ResultadoOperacion.Error(MensajeNoExiste);
            }

            tarea.Completada = !tarea.Completada;
            _repositorio.Actualizar(tarea);

            string mensaje = tarea.Completada
                ? "Tarea marcada como completada."
                : "Tarea marcada como pendiente.";

            return ResultadoOperacion.Ok(mensaje, TareaDto.Desde(tarea));
        }

        public ResultadoOperacion Eliminar(int id)
        {
            bool eliminada = _repositorio.Eliminar(id);

            return eliminada
                ? ResultadoOperacion.Ok("Tarea eliminada.")
                : ResultadoOperacion.Error(MensajeNoExiste);
        }

        private static string Validar(string titulo, string descripcion)
        {
            if (string.IsNullOrWhiteSpace(titulo))
            {
                return "El titulo es obligatorio.";
            }

            if (string.IsNullOrWhiteSpace(descripcion))
            {
                return "La descripcion es obligatoria.";
            }

            if (titulo.Trim().Length > MaximoTitulo)
            {
                return "El titulo no puede superar " + MaximoTitulo + " caracteres.";
            }

            if (descripcion.Trim().Length > MaximoDescripcion)
            {
                return "La descripcion no puede superar " + MaximoDescripcion + " caracteres.";
            }

            return null;
        }
    }
}
