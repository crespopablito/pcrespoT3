using pcrespoT3.Models;
using pcrespoT3.Services;

namespace pcrespoT3.Views
{
    public partial class vHome : ContentPage
    {
        private readonly ContactoService _servicio;

        public vHome(ContactoService servicio)
        {
            InitializeComponent();
            _servicio = servicio;

            PkTipo.ItemsSource = new List<string>
            {
                Validador.TipoCi,
                Validador.TipoRuc,
                Validador.TipoPasaporte
            };

            DpFecha.MaximumDate = DateTime.Today;
            DpFecha.Date = DateTime.Today;
            TxtIdentificacion.IsEnabled = false;
        }

        private string? TipoSeleccionado => PkTipo.SelectedItem as string;

        private DateTime FechaSeleccionada => DpFecha.Date is DateTime fecha ? fecha : DateTime.Today;

        private static bool MostrarError(Label etiqueta, string? mensaje)
        {
            etiqueta.Text = mensaje ?? string.Empty;
            etiqueta.IsVisible = mensaje is not null;
            return mensaje is null;
        }

        private void PkTipo_SelectedIndexChanged(object? sender, EventArgs e)
        {
            var tipo = TipoSeleccionado;

            TxtIdentificacion.Text = string.Empty;
            TxtIdentificacion.IsEnabled = tipo is not null;

            switch (tipo)
            {
                case Validador.TipoCi:
                    TxtIdentificacion.Keyboard = Keyboard.Numeric;
                    TxtIdentificacion.MaxLength = 10;
                    TxtIdentificacion.Placeholder = "10 números";
                    break;
                case Validador.TipoRuc:
                    TxtIdentificacion.Keyboard = Keyboard.Numeric;
                    TxtIdentificacion.MaxLength = 13;
                    TxtIdentificacion.Placeholder = "13 números";
                    break;
                case Validador.TipoPasaporte:
                    TxtIdentificacion.Keyboard = Keyboard.Text;
                    TxtIdentificacion.MaxLength = 20;
                    TxtIdentificacion.Placeholder = "Número de pasaporte";
                    break;
            }

            MostrarError(LblErrorTipo, Validador.ValidarTipo(tipo));
            MostrarError(LblErrorIdentificacion, null);
        }

        private void TxtIdentificacion_TextChanged(object? sender, TextChangedEventArgs e)
        {
            var tipo = TipoSeleccionado;
            var texto = e.NewTextValue ?? string.Empty;

            if (tipo == Validador.TipoCi || tipo == Validador.TipoRuc)
            {
                var soloNumeros = new string(texto.Where(char.IsDigit).ToArray());
                if (soloNumeros != texto)
                {
                    TxtIdentificacion.Text = soloNumeros;
                    return;
                }
            }

            if (LblErrorIdentificacion.IsVisible)
                MostrarError(LblErrorIdentificacion, Validador.ValidarIdentificacion(tipo, texto));
        }

        private void TxtIdentificacion_Unfocused(object? sender, FocusEventArgs e)
        {
            if (TipoSeleccionado is not null)
                MostrarError(LblErrorIdentificacion, Validador.ValidarIdentificacion(TipoSeleccionado, TxtIdentificacion.Text));
        }

        private void TxtNombres_Unfocused(object? sender, FocusEventArgs e)
        {
            MostrarError(LblErrorNombres, Validador.ValidarNombre(TxtNombres.Text, "Los nombres"));
        }

        private void TxtApellidos_Unfocused(object? sender, FocusEventArgs e)
        {
            MostrarError(LblErrorApellidos, Validador.ValidarNombre(TxtApellidos.Text, "Los apellidos"));
        }

        private void DpFecha_DateSelected(object? sender, DateChangedEventArgs e)
        {
            MostrarError(LblErrorFecha, Validador.ValidarFecha(e.NewDate is DateTime nueva ? nueva : DateTime.Today));
        }

        private void TxtCorreo_Unfocused(object? sender, FocusEventArgs e)
        {
            MostrarError(LblErrorCorreo, Validador.ValidarCorreo(TxtCorreo.Text));
        }

        private void TxtSalario_Unfocused(object? sender, FocusEventArgs e)
        {
            MostrarError(LblErrorSalario, Validador.ValidarSalario(TxtSalario.Text, out _));
        }

        private bool ValidarFormulario(out decimal salario)
        {
            var valido = MostrarError(LblErrorTipo, Validador.ValidarTipo(TipoSeleccionado));
            valido &= MostrarError(LblErrorIdentificacion, Validador.ValidarIdentificacion(TipoSeleccionado, TxtIdentificacion.Text));
            valido &= MostrarError(LblErrorNombres, Validador.ValidarNombre(TxtNombres.Text, "Los nombres"));
            valido &= MostrarError(LblErrorApellidos, Validador.ValidarNombre(TxtApellidos.Text, "Los apellidos"));
            valido &= MostrarError(LblErrorFecha, Validador.ValidarFecha(FechaSeleccionada));
            valido &= MostrarError(LblErrorCorreo, Validador.ValidarCorreo(TxtCorreo.Text));
            valido &= MostrarError(LblErrorSalario, Validador.ValidarSalario(TxtSalario.Text, out salario));
            return valido;
        }

        private async void BtnGuardar_Clicked(object? sender, EventArgs e)
        {
            if (!ValidarFormulario(out var salario))
            {
                await DisplayAlertAsync("Datos inválidos", "Corrija los campos marcados en rojo.", "Aceptar");
                return;
            }

            _servicio.Actual = new Contacto
            {
                TipoIdentificacion = TipoSeleccionado!,
                Identificacion = (TxtIdentificacion.Text ?? string.Empty).Trim(),
                Nombres = (TxtNombres.Text ?? string.Empty).Trim(),
                Apellidos = (TxtApellidos.Text ?? string.Empty).Trim(),
                Fecha = FechaSeleccionada,
                Correo = (TxtCorreo.Text ?? string.Empty).Trim(),
                Salario = salario
            };

            await Shell.Current.GoToAsync(nameof(vResumen));
        }

        private void BtnLimpiar_Clicked(object? sender, EventArgs e)
        {
            PkTipo.SelectedIndex = -1;
            TxtIdentificacion.Text = string.Empty;
            TxtIdentificacion.IsEnabled = false;
            TxtIdentificacion.Placeholder = "Seleccione primero el tipo";
            TxtNombres.Text = string.Empty;
            TxtApellidos.Text = string.Empty;
            DpFecha.Date = DateTime.Today;
            TxtCorreo.Text = string.Empty;
            TxtSalario.Text = string.Empty;

            MostrarError(LblErrorTipo, null);
            MostrarError(LblErrorIdentificacion, null);
            MostrarError(LblErrorNombres, null);
            MostrarError(LblErrorApellidos, null);
            MostrarError(LblErrorFecha, null);
            MostrarError(LblErrorCorreo, null);
            MostrarError(LblErrorSalario, null);
        }
    }
}
