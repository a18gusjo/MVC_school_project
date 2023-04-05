using MySql.Data.MySqlClient;
using System.Data;

namespace ModelViewController.Models
{
    public class PensioneradModel
    {

        private IConfiguration _configuration;
        private string connectionString;

        public PensioneradModel(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration["ConnectionString"];
        }

        public DataTable GetAllPensionerad()
        {
            MySqlConnection dbcon = new MySqlConnection(connectionString);
            dbcon.Open();
            MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM PensioneradRen;", dbcon);
            DataSet ds = new DataSet();
            adapter.Fill(ds, "result");
            DataTable stankTable = ds.Tables["result"];
            dbcon.Close();
            return stankTable;

        }

        public void UppdateraPensionerad(string smak, string fabriknamn, string polsaburknr, string ren_nummer)
        {
            MySqlConnection dbcon = new MySqlConnection(connectionString);
            dbcon.Open();
            string UpdateString = "UPDATE PensioneradRen SET smak = @smak, fabriknamn = @fabriknamn,  polsaburknr = @polsaburknr WHERE ren_nummer = @ren_nummer;";
            MySqlCommand sqlCmd = new MySqlCommand(UpdateString, dbcon);
            sqlCmd.Parameters.AddWithValue("@smak", smak);
            sqlCmd.Parameters.AddWithValue("@fabriknamn", fabriknamn);
            sqlCmd.Parameters.AddWithValue("@polsaburknr", polsaburknr);
            sqlCmd.Parameters.AddWithValue("@ren_nummer", ren_nummer);
            int rows = sqlCmd.ExecuteNonQuery();
            dbcon.Close();
        }
    }
}
