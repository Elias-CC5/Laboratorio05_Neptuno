using System.Configuration;
using System.Data.SqlClient;

namespace Laboratorio05
{
    public class Conexion
    {
        public static SqlConnection ObtenerConexion()
        {
            string cadena = ConfigurationManager.ConnectionStrings["NeptunoCon"].ConnectionString;
            return new SqlConnection(cadena);
        }
    }
}