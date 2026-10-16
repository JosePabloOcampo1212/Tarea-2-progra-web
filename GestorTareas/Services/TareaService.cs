using System;
using System.Collections.Generic;
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

        public IList<Tarea> Listar()
        {
            return _repositorio.ObtenerTodas();
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

        private static string Validar(string titulo, string descripcion)
        {
            if (string.IsNullOrWhiteSpace(titulo))
            {
                return "El título es obligatorio.";
            }

            if (string.IsNullOrWhiteSpace(descripcion))
            {
                return "La descripción es obligatoria.";
            }

            if (titulo.Trim().Length > MaximoTitulo)
            {
                return "El título no puede superar " + MaximoTitulo + " caracteres.";
            }

            if (descripcion.Trim().Length > MaximoDescripcion)
            {
                return "La descripción no puede superar " + MaximoDescripcion + " caracteres.";
            }

            return null;
        }
    }
}
