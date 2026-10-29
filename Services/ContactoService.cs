using System.Globalization;
using System.Text;
using pcrespoT3.Models;

namespace pcrespoT3.Services
{
    public class ContactoService
    {
        public Contacto? Actual { get; set; }

        public static bool EsMovil => DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS;

        public static string Dinero(decimal valor)
        {
            return $"${valor.ToString("N2", CultureInfo.InvariantCulture)}";
        }

        private static string CarpetaDestino()
        {
            return EsMovil
                ? FileSystem.AppDataDirectory
                : Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        public string GenerarTexto(Contacto contacto)
        {
            var sb = new StringBuilder();
            sb.AppendLine("DATOS DEL CONTACTO");
            sb.AppendLine("------------------");
            sb.AppendLine($"Tipo de identificacion: {contacto.TipoIdentificacion}");
            sb.AppendLine($"Identificacion: {contacto.Identificacion}");
            sb.AppendLine($"Nombres: {contacto.Nombres}");
            sb.AppendLine($"Apellidos: {contacto.Apellidos}");
            sb.AppendLine($"Fecha: {contacto.Fecha:dd/MM/yyyy}");
            sb.AppendLine($"Correo: {contacto.Correo}");
            sb.AppendLine($"Salario: {Dinero(contacto.Salario)}");
            sb.AppendLine($"Aporte IESS ({Contacto.PorcentajeIess * 100:0.##}%): {Dinero(contacto.AporteIess)}");
            sb.AppendLine($"Salario neto: {Dinero(contacto.SalarioNeto)}");
            return sb.ToString();
        }

        public async Task<string> ExportarAsync(Contacto contacto)
        {
            var nombreArchivo = $"contacto_{contacto.Identificacion}_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
            var ruta = Path.Combine(CarpetaDestino(), nombreArchivo);
            await File.WriteAllTextAsync(ruta, GenerarTexto(contacto), Encoding.UTF8);
            return ruta;
        }
    }
}
