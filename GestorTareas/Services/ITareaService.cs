using System.Collections.Generic;
using GestorTareas.Models;

namespace GestorTareas.Services
{
    public interface ITareaService
    {
        IList<Tarea> Listar();
    }
}
