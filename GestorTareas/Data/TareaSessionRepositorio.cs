using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GestorTareas.Models;

namespace GestorTareas.Data
{
    public class TareaSessionRepositorio : ITareaRepositorio
    {
        private const string ClaveTareas = "GestorTareas.Lista";

        private const string ClaveSiguienteId = "GestorTareas.SiguienteId";

        private readonly HttpSessionStateBase _session;

        public TareaSessionRepositorio(HttpSessionStateBase session)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
        }

        public List<Tarea> ObtenerTodas()
        {
            return Lista()
                .OrderByDescending(t => t.FechaCreacion)
                .ToList();
        }

        public Tarea ObtenerPorId(int id)
        {
            return Lista().FirstOrDefault(t => t.Id == id);
        }

        public void Agregar(Tarea tarea)
        {
            List<Tarea> lista = Lista();
            tarea.Id = SiguienteId();
            lista.Add(tarea);
            Guardar(lista);
        }

        public void Actualizar(Tarea tarea)
        {
            List<Tarea> lista = Lista();
            int indice = lista.FindIndex(t => t.Id == tarea.Id);

            if (indice >= 0)
            {
                lista[indice] = tarea;
                Guardar(lista);
            }
        }

        public bool Eliminar(int id)
        {
            List<Tarea> lista = Lista();
            int eliminadas = lista.RemoveAll(t => t.Id == id);

            if (eliminadas > 0)
            {
                Guardar(lista);
            }

            return eliminadas > 0;
        }

        private List<Tarea> Lista()
        {
            var lista = _session[ClaveTareas] as List<Tarea>;

            if (lista == null)
            {
                lista = new List<Tarea>();
                _session[ClaveTareas] = lista;
            }

            return lista;
        }

        private void Guardar(List<Tarea> lista)
        {
            _session[ClaveTareas] = lista;
        }

        private int SiguienteId()
        {
            int siguiente = _session[ClaveSiguienteId] is int valor ? valor : 1;
            _session[ClaveSiguienteId] = siguiente + 1;
            return siguiente;
        }
    }
}
