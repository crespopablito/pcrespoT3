using pcrespoT3.Views;

namespace pcrespoT3
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(vResumen), typeof(vResumen));
        }
    }
}
