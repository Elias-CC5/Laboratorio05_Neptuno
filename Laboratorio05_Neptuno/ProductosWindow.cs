using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Laboratorio05
{
    public partial class ProductosWindow : Window
    {
        public ProductosWindow()
        {
            InitializeComponent();
            ListarProductos();
        }

        private void ListarProductos()
        {
            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_ListarProductos", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgProductos.ItemsSource = dt.DefaultView;
            }
        }

        // INSERTAR con ExecuteNonQuery
        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_InsertarProducto", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NombreProducto", txtNombre.Text);
                cmd.Parameters.AddWithValue("@PrecioUnidad", Convert.ToDecimal(txtPrecio.Text));
                cmd.Parameters.AddWithValue("@UnidadesEnExistencia", Convert.ToInt16(txtStock.Text));

                con.Open();
                int filasAfectadas = cmd.ExecuteNonQuery(); // ExecuteNonQuery para escritura
                if (filasAfectadas > 0)
                {
                    MessageBox.Show("Producto registrado correctamente.");
                    ListarProductos();
                    LimpiarFormulario();
                }
            }
        }
        private void btnActualizar_Click(object sender, RoutedEventArgs e)
        {
            // Validar que se haya seleccionado un producto (que el ID no esté vacío)
            if (string.IsNullOrWhiteSpace(txtProductoID.Text))
            {
                MessageBox.Show("Seleccione un producto de la lista antes de hacer clic en Actualizar.");
                return;
            }

            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_ActualizarProducto", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductoID", Convert.ToInt32(txtProductoID.Text));
                cmd.Parameters.AddWithValue("@NombreProducto", txtNombre.Text);
                cmd.Parameters.AddWithValue("@PrecioUnidad", Convert.ToDecimal(txtPrecio.Text));
                cmd.Parameters.AddWithValue("@UnidadesEnExistencia", Convert.ToInt16(txtStock.Text));

                con.Open();
                int filasAfectadas = cmd.ExecuteNonQuery();
                if (filasAfectadas > 0)
                {
                    MessageBox.Show("Producto actualizado correctamente.");
                    ListarProductos();
                    LimpiarFormulario();
                }
            }
        }

        // ELIMINACIÓN LÓGICA con ExecuteNonQuery (sp_EliminarProducto ejecuta UPDATE Activo = 0)
        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtProductoID.Text)) return;

            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_EliminarProducto", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductoID", Convert.ToInt32(txtProductoID.Text));

                con.Open();
                int filasAfectadas = cmd.ExecuteNonQuery(); // Invocación del SP de baja lógica
                if (filasAfectadas > 0)
                {
                    MessageBox.Show("Producto dado de baja lógicamente (Activo = 0).");
                    ListarProductos();
                    LimpiarFormulario();
                }
            }
        }

        private void dgProductos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgProductos.SelectedItem is DataRowView row)
            {
                txtProductoID.Text = row["ProductoID"].ToString();
                txtNombre.Text = row["NombreProducto"].ToString();
                txtPrecio.Text = row["PrecioUnidad"].ToString();
                txtStock.Text = row["UnidadesEnExistencia"].ToString();
            }
        }

        private void LimpiarFormulario()
        {
            txtProductoID.Clear();
            txtNombre.Clear();
            txtPrecio.Clear();
            txtStock.Clear();
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e) => LimpiarFormulario();
    }
}