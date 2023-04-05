using MySql.Data.MySqlClient;
using System.Data;

namespace ModelViewController.Models
{
    public class TjänstModel
    {
        private IConfiguration _configuration;
        private string connectionString;

        public TjänstModel(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration["ConnectionString"];
        }

        public DataTable GetAllTjänst()
        {
            MySqlConnection dbcon = new MySqlConnection(connectionString);
            dbcon.Open();
            MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM TjänsteRen;", dbcon);
            DataSet ds = new DataSet();
            adapter.Fill(ds, "result");
            DataTable stankTable = ds.Tables["result"];
            dbcon.Close();
            return stankTable;

        }
        public void UppdateraTjänst(string lon, string ren_nr)
        {
            MySqlConnection dbcon = new MySqlConnection(connectionString);
            dbcon.Open();
            string UpdateString = "UPDATE TjänsteRen SET lon = @lon WHERE ren_nr = @ren_nr;";
            MySqlCommand sqlCmd = new MySqlCommand(UpdateString, dbcon);
            sqlCmd.Parameters.AddWithValue("@lon", lon);
            sqlCmd.Parameters.AddWithValue("@ren_nr", ren_nr);
            int rows = sqlCmd.ExecuteNonQuery();
            dbcon.Close();
        }

    }
}
