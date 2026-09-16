using Laboratorio05;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Laboratorio05_Neptuno
{
    public partial class ProveedoresWindow : Window
    {
        public ProveedoresWindow()
        {
            InitializeComponent();
            BuscarProveedores();
        }

        private void BuscarProveedores()
        {
            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_BuscarProveedores", con) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@NombreContacto", string.IsNullOrEmpty(txtBuscarContacto.Text) ? (object)DBNull.Value : txtBuscarContacto.Text);
                cmd.Parameters.AddWithValue("@Ciudad", string.IsNullOrEmpty(txtBuscarCiudad.Text) ? (object)DBNull.Value : txtBuscarCiudad.Text);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgProveedores.ItemsSource = dt.DefaultView;
            }
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e) => BuscarProveedores();

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (dgProveedores.SelectedItem is DataRowView row)
            {
                int id = Convert.ToInt32(row["ProveedorID"]);
                using (SqlConnection con = Conexion.ObtenerConexion())
                {
                    SqlCommand cmd = new SqlCommand("sp_EliminarProveedor", con) { CommandType = CommandType.StoredProcedure };
                    cmd.Parameters.AddWithValue("@ProveedorID", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Proveedor dado de baja lógicamente.");
                    BuscarProveedores();
                }
            }
        }
    }
}