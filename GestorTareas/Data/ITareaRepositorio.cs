using System.Collections.Generic;
using GestorTareas.Models;

namespace GestorTareas.Data
{
    public interface ITareaRepositorio
    {
        List<Tarea> ObtenerTodas();

        Tarea ObtenerPorId(int id);

        void Agregar(Tarea tarea);

        void Actualizar(Tarea tarea);

        bool Eliminar(int id);
    }
}
