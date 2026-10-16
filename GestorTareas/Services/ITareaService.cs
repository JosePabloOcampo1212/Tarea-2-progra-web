using System.Collections.Generic;
using GestorTareas.Models;
using GestorTareas.Models.Dtos;

namespace GestorTareas.Services
{
    public interface ITareaService
    {
        IList<Tarea> Listar(FiltroTareas filtro);

        ResumenTareas ObtenerResumen();

        ResultadoOperacion Crear(string titulo, string descripcion);

        ResultadoOperacion Editar(int id, string titulo, string descripcion);

        ResultadoOperacion CambiarEstado(int id);

        ResultadoOperacion Eliminar(int id);
    }
}
