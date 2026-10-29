using pcrespoT3.Models;
using pcrespoT3.Services;

namespace pcrespoT3.Views
{
    public partial class vResumen : ContentPage
    {
        private readonly ContactoService _servicio;

        public vResumen(ContactoService servicio)
        {
            InitializeComponent();
            _servicio = servicio;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var contacto = _servicio.Actual;
            if (contacto is null)
                return;

            LblTipo.Text = contacto.TipoIdentificacion;
            LblIdentificacion.Text = contacto.Identificacion;
            LblNombres.Text = contacto.Nombres;
            LblApellidos.Text = contacto.Apellidos;
            LblFecha.Text = contacto.Fecha.ToString("dd/MM/yyyy");
            LblCorreo.Text = contacto.Correo;
            LblSalario.Text = ContactoService.Dinero(contacto.Salario);
            LblEtiquetaAporte.Text = $"Aporte IESS ({Contacto.PorcentajeIess * 100:0.##}%)";
            LblAporte.Text = ContactoService.Dinero(contacto.AporteIess);
            LblNeto.Text = ContactoService.Dinero(contacto.SalarioNeto);
            LblRuta.IsVisible = false;
        }

        private async void BtnExportar_Clicked(object? sender, EventArgs e)
        {
            var contacto = _servicio.Actual;
            if (contacto is null)
                return;

            try
            {
                var ruta = await _servicio.ExportarAsync(contacto);
                LblRuta.Text = $"Archivo guardado en:\n{ruta}";
                LblRuta.IsVisible = true;

                if (ContactoService.EsMovil)
                {
                    await Share.Default.RequestAsync(new ShareFileRequest
                    {
                        Title = "Exportar contacto",
                        File = new ShareFile(ruta)
                    });
                }
                else
                {
                    await DisplayAlertAsync("Exportación completa", $"Archivo guardado en:\n{ruta}", "Aceptar");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Error", $"No se pudo exportar el archivo: {ex.Message}", "Aceptar");
            }
        }

        private async void BtnVolver_Clicked(object? sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
