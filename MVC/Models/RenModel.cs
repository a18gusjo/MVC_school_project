using MySql.Data.MySqlClient;
using System.Data;
using System.Data.SqlTypes;
using System.Xml.Linq;

namespace ModelViewController.Models
{
    public class RenModel
    {
        private IConfiguration _configuration;
        private string connectionString;

        public RenModel(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration["ConnectionString"];
        }

        public  DataTable GetAllRen()
        {
            MySqlConnection dbcon = new MySqlConnection(connectionString);
            dbcon.Open();
            MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT nr, klan, underart, stank, tjänststatus FROM Ren;", dbcon);
            DataSet ds = new DataSet();
            adapter.Fill(ds, "result");
            DataTable renspannTable = ds.Tables["result"];
            dbcon.Close();
            return renspannTable;

        }

        public void InsertRen(string nr, string klan, string underart, long stank)
        {
            MySqlConnection dbcon = new MySqlConnection(connectionString);
            dbcon.Open();
            string InsertString = "INSERT INTO Ren(nr, klan, underart, stank) VALUES(@nr, @klan , @underart ,@stank);";
            MySqlCommand sqlCmd = new MySqlCommand(InsertString, dbcon);
            sqlCmd.Parameters.AddWithValue("@nr", nr);
            sqlCmd.Parameters.AddWithValue("@klan", klan);
            sqlCmd.Parameters.AddWithValue("@underart", underart);
            sqlCmd.Parameters.AddWithValue("@stank", stank);
            int rows = sqlCmd.ExecuteNonQuery();
            dbcon.Close();
        }

        public void PensioneraRen(string nr)
        {
            MySqlConnection dbcon = new MySqlConnection(connectionString);
            dbcon.Open();
            string ProcedureString = "CALL PensioneraRen(@nr, 'Köttkvarnen');";
            MySqlCommand sqlCmd = new MySqlCommand(ProcedureString, dbcon);
            sqlCmd.Parameters.AddWithValue("@nr", nr);
            int rows = sqlCmd.ExecuteNonQuery();
            dbcon.Close();
        } 

    }
}
