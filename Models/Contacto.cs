namespace pcrespoT3.Models
{
    public class Contacto
    {
        public const decimal PorcentajeIess = 0.0945m;

        public string TipoIdentificacion { get; set; } = string.Empty;
        public string Identificacion { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string Correo { get; set; } = string.Empty;
        public decimal Salario { get; set; }

        public decimal AporteIess => Math.Round(Salario * PorcentajeIess, 2);
        public decimal SalarioNeto => Salario - AporteIess;
    }
}
