namespace GestorTareas.Models.Dtos
{
    public class TareaDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public bool Completada { get; set; }

        public string FechaCreacion { get; set; }

        public static TareaDto Desde(Tarea tarea)
        {
            return new TareaDto
            {
                Id = tarea.Id,
                Titulo = tarea.Titulo,
                Descripcion = tarea.Descripcion,
                Completada = tarea.Completada,
                FechaCreacion = tarea.FechaCreacion.ToString("dd/MM/yyyy HH:mm")
            };
        }
    }
}
