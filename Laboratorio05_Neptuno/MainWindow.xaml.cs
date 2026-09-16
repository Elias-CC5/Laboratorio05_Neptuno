using Laboratorio05;
using System.Windows;

namespace Laboratorio05_Neptuno
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnProductos_Click(object sender, RoutedEventArgs e)
        {
            ProductosWindow win = new ProductosWindow();
            win.ShowDialog();
        }

        private void btnCategorias_Click(object sender, RoutedEventArgs e)
        {
            CategoriasWindow win = new CategoriasWindow();
            win.ShowDialog();
        }

        private void btnProveedores_Click(object sender, RoutedEventArgs e)
        {
            ProveedoresWindow win = new ProveedoresWindow();
            win.ShowDialog();
        }

        private void btnPedidos_Click(object sender, RoutedEventArgs e)
        {
            PedidosWindow win = new PedidosWindow();
            win.ShowDialog();
        }
    }
}