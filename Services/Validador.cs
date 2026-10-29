using System.Globalization;
using System.Text.RegularExpressions;

namespace pcrespoT3.Services
{
    public static class Validador
    {
        public const string TipoCi = "CI";
        public const string TipoRuc = "RUC";
        public const string TipoPasaporte = "Pasaporte";

        private static readonly Regex SoloNumeros = new(@"^\d+$", RegexOptions.Compiled);
        private static readonly Regex SoloLetras = new(@"^[\p{L}]+([ '\-][\p{L}]+)*$", RegexOptions.Compiled);
        private static readonly Regex FormatoCorreo = new(@"^[^@\s]+@[^@\s]+\.[^@\s]{2,}$", RegexOptions.Compiled);

        public static string? ValidarTipo(string? tipo)
        {
            return string.IsNullOrWhiteSpace(tipo) ? "Seleccione un tipo de identificación." : null;
        }

        public static string? ValidarIdentificacion(string? tipo, string? valor)
        {
            valor = valor?.Trim() ?? string.Empty;

            if (valor.Length == 0)
                return "La identificación es obligatoria.";

            switch (tipo)
            {
                case TipoCi:
                    if (!SoloNumeros.IsMatch(valor) || valor.Length != 10)
                        return "La cédula debe tener exactamente 10 números.";
                    break;
                case TipoRuc:
                    if (!SoloNumeros.IsMatch(valor) || valor.Length != 13)
                        return "El RUC debe tener exactamente 13 números.";
                    break;
                case TipoPasaporte:
                    if (valor.Length < 5 || valor.Length > 20)
                        return "El pasaporte debe tener entre 5 y 20 caracteres.";
                    break;
                default:
                    return "Seleccione un tipo de identificación.";
            }

            return null;
        }

        public static string? ValidarNombre(string? valor, string campo)
        {
            valor = valor?.Trim() ?? string.Empty;

            if (valor.Length == 0)
                return $"{campo} es obligatorio.";
            if (valor.Length < 2)
                return $"{campo} debe tener al menos 2 letras.";
            if (!SoloLetras.IsMatch(valor))
                return $"{campo} solo admite letras y espacios.";

            return null;
        }

        public static string? ValidarFecha(DateTime fecha)
        {
            if (fecha.Date > DateTime.Today)
                return "La fecha no puede ser futura.";
            if (fecha.Year < 1900)
                return "La fecha no es válida.";

            return null;
        }

        public static string? ValidarCorreo(string? valor)
        {
            valor = valor?.Trim() ?? string.Empty;

            if (valor.Length == 0)
                return "El correo es obligatorio.";
            if (!FormatoCorreo.IsMatch(valor))
                return "Ingrese un correo válido (ejemplo: nombre@dominio.com).";

            return null;
        }

        public static string? ValidarSalario(string? valor, out decimal salario)
        {
            salario = 0;
            valor = valor?.Trim().Replace(',', '.') ?? string.Empty;

            if (valor.Length == 0)
                return "El salario es obligatorio.";
            if (!decimal.TryParse(valor, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out salario))
                return "Ingrese un salario numérico válido.";
            if (salario <= 0)
                return "El salario debe ser mayor que cero.";
            if (salario > 1000000)
                return "El salario no puede superar 1.000.000.";

            return null;
        }
    }
}
