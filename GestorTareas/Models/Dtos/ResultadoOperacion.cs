namespace GestorTareas.Models.Dtos
{
    public class ResultadoOperacion
    {
        public bool Exito { get; set; }

        public string Mensaje { get; set; }

        public object Datos { get; set; }

        public static ResultadoOperacion Ok(string mensaje, object datos = null)
        {
            return new ResultadoOperacion { Exito = true, Mensaje = mensaje, Datos = datos };
        }

        public static ResultadoOperacion Error(string mensaje)
        {
            return new ResultadoOperacion { Exito = false, Mensaje = mensaje };
        }
    }
}
