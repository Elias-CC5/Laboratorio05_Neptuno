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
                MessageBox.Show("Por favor, seleccione ambas fechas.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DateTime fechaInicio = dpInicio.SelectedDate.Value.Date;
            DateTime fechaFin = dpFin.SelectedDate.Value.Date.AddDays(1).AddSeconds(-1);

            if (fechaInicio > fechaFin)
            {
                MessageBox.Show("La fecha de inicio debe ser menor o igual a la fecha de fin.", "Rango Inválido", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (SqlConnection con = Conexion.ObtenerConexion())
                {
                    SqlCommand cmd = new SqlCommand("sp_ReporteDetallePedidosPorFechas", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", fechaFin);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgReporte.ItemsSource = dt.DefaultView;

                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No se encontraron pedidos registrados en ese rango de fechas.", "Sin resultados", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al consultar el reporte: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    }
