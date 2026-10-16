using System.Collections.Generic;
using GestorTareas.Models;
using GestorTareas.Models.Dtos;

namespace GestorTareas.Services
{
    public interface ITareaService
    {
        IList<Tarea> Listar();

        ResultadoOperacion Crear(string titulo, string descripcion);
    }
}
