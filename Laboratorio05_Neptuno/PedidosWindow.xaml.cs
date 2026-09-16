using Laboratorio05;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Laboratorio05_Neptuno
{
    public partial class PedidosWindow : Window
    {
        public PedidosWindow()
        {
            InitializeComponent();
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            if (dpInicio.SelectedDate == null || dpFin.SelectedDate == null)
            {
                MessageBox.Show("Por favor, seleccione la fecha de inicio y la fecha de fin.");
                return;
            }

            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_ReporteDetallePedidosPorFechas", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@FechaInicio", dpInicio.SelectedDate.Value);
                cmd.Parameters.AddWithValue("@FechaFin", dpFin.SelectedDate.Value);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgReporte.ItemsSource = dt.DefaultView;
            }
        }
    }
}