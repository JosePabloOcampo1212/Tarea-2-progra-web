using System;
using System.Collections.Generic;
using GestorTareas.Data;
using GestorTareas.Models;

namespace GestorTareas.Services
{
    public class TareaService : ITareaService
    {
        private readonly ITareaRepositorio _repositorio;

        public TareaService(ITareaRepositorio repositorio)
        {
            _repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public IList<Tarea> Listar()
        {
            return _repositorio.ObtenerTodas();
        }
    }
}
