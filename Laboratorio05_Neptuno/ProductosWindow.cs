using Laboratorio05;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Laboratorio05_Neptuno
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
            try
            {
                using (SqlConnection con = Conexion.ObtenerConexion())
                {
                    SqlCommand cmd = new SqlCommand("sp_ListarProductos", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgProductos.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar productos: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // INSERTAR con parámetros completos
        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtPrecio.Text) ||
                string.IsNullOrWhiteSpace(txtStock.Text))
            {
                MessageBox.Show("Por favor complete Nombre, Precio y Stock.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(txtPrecio.Text, out decimal precio) ||
                !short.TryParse(txtStock.Text, out short stock))
            {
                MessageBox.Show("Ingrese valores numéricos válidos en Precio y Stock.", "Error de formato", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                using (SqlConnection con = Conexion.ObtenerConexion())
                {
                    SqlCommand cmd = new SqlCommand("sp_InsertarProducto", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@NombreProducto", txtNombre.Text);
                    cmd.Parameters.AddWithValue("@ProveedorID", string.IsNullOrWhiteSpace(txtProveedorID.Text) ? (object)DBNull.Value : Convert.ToInt32(txtProveedorID.Text));
                    cmd.Parameters.AddWithValue("@CategoriaID", string.IsNullOrWhiteSpace(txtCategoriaID.Text) ? (object)DBNull.Value : Convert.ToInt32(txtCategoriaID.Text));
                    cmd.Parameters.AddWithValue("@CantidadPorUnidad", string.IsNullOrWhiteSpace(txtCantidadPorUnidad.Text) ? (object)DBNull.Value : txtCantidadPorUnidad.Text);
                    cmd.Parameters.AddWithValue("@PrecioUnidad", precio);
                    cmd.Parameters.AddWithValue("@UnidadesEnExistencia", stock);

                    con.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                    {
                        MessageBox.Show("Producto registrado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                        ListarProductos();
                        LimpiarFormulario();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al insertar producto: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ACTUALIZAR con parámetros completos
        private void btnActualizar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProductoID.Text))
            {
                MessageBox.Show("Seleccione un producto para actualizar.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(txtPrecio.Text, out decimal precio) ||
                !short.TryParse(txtStock.Text, out short stock))
            {
                MessageBox.Show("Ingrese valores numéricos válidos en Precio y Stock.", "Error de formato", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                using (SqlConnection con = Conexion.ObtenerConexion())
                {
                    SqlCommand cmd = new SqlCommand("sp_ActualizarProducto", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@ProductoID", Convert.ToInt32(txtProductoID.Text));
                    cmd.Parameters.AddWithValue("@NombreProducto", txtNombre.Text);
                    cmd.Parameters.AddWithValue("@ProveedorID", string.IsNullOrWhiteSpace(txtProveedorID.Text) ? (object)DBNull.Value : Convert.ToInt32(txtProveedorID.Text));
                    cmd.Parameters.AddWithValue("@CategoriaID", string.IsNullOrWhiteSpace(txtCategoriaID.Text) ? (object)DBNull.Value : Convert.ToInt32(txtCategoriaID.Text));
                    cmd.Parameters.AddWithValue("@CantidadPorUnidad", string.IsNullOrWhiteSpace(txtCantidadPorUnidad.Text) ? (object)DBNull.Value : txtCantidadPorUnidad.Text);
                    cmd.Parameters.AddWithValue("@PrecioUnidad", precio);
                    cmd.Parameters.AddWithValue("@UnidadesEnExistencia", stock);

                    con.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                    {
                        MessageBox.Show("Producto actualizado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                        ListarProductos();
                        LimpiarFormulario();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar producto: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ELIMINACIÓN LÓGICA
        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProductoID.Text))
            {
                MessageBox.Show("Seleccione un producto para dar de baja.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (SqlConnection con = Conexion.ObtenerConexion())
                {
                    SqlCommand cmd = new SqlCommand("sp_EliminarProducto", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@ProductoID", Convert.ToInt32(txtProductoID.Text));

                    con.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                    {
                        MessageBox.Show("Producto dado de baja lógicamente (Activo = 0).", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                        ListarProductos();
                        LimpiarFormulario();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar producto: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Selección de fila en el DataGrid
        private void dgProductos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgProductos.SelectedItem is DataRowView row)
            {
                txtProductoID.Text = row["ProductoID"].ToString();
                txtNombre.Text = row["NombreProducto"].ToString();
                txtProveedorID.Text = row["ProveedorID"].ToString();
                txtCategoriaID.Text = row["CategoriaID"].ToString();
                txtCantidadPorUnidad.Text = row["CantidadPorUnidad"].ToString();
                txtPrecio.Text = row["PrecioUnidad"].ToString();
                txtStock.Text = row["UnidadesEnExistencia"].ToString();
            }
        }

        private void LimpiarFormulario()
        {
            txtProductoID.Clear();
            txtNombre.Clear();
            txtProveedorID.Clear();
            txtCategoriaID.Clear();
            txtCantidadPorUnidad.Clear();
            txtPrecio.Clear();
            txtStock.Clear();
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e) => LimpiarFormulario();
    }
}