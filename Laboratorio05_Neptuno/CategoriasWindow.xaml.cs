using Laboratorio05;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Laboratorio05_Neptuno
{
    public partial class CategoriasWindow : Window
    {
        public CategoriasWindow()
        {
            InitializeComponent();
            ListarCategorias();
        }

        private void ListarCategorias()
        {
            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_ListarCategorias", con) { CommandType = CommandType.StoredProcedure };
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgCategorias.ItemsSource = dt.DefaultView;
            }
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_InsertarCategoria", con) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@NombreCategoria", txtNombre.Text);
                cmd.Parameters.AddWithValue("@Descripcion", txtDescripcion.Text);
                con.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Categoría agregada.");
                ListarCategorias();
            }
        }

        private void btnActualizar_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_ActualizarCategoria", con) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@CategoriaID", Convert.ToInt32(txtCategoriaID.Text));
                cmd.Parameters.AddWithValue("@NombreCategoria", txtNombre.Text);
                cmd.Parameters.AddWithValue("@Descripcion", txtDescripcion.Text);
                con.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Categoría actualizada.");
                ListarCategorias();
            }
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtCategoriaID.Text)) return;
            using (SqlConnection con = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_EliminarCategoria", con) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@CategoriaID", Convert.ToInt32(txtCategoriaID.Text));
                con.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Categoría dada de baja lógicamente.");
                ListarCategorias();
            }
        }

        private void dgCategorias_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgCategorias.SelectedItem is DataRowView row)
            {
                txtCategoriaID.Text = row["CategoriaID"].ToString();
                txtNombre.Text = row["NombreCategoria"].ToString();
                txtDescripcion.Text = row["Descripcion"].ToString();
            }
        }
    }
}