using MySql.Data.MySqlClient;
using System.Data;

namespace ModelViewController.Models
{
    public class StankModel
    {

        private IConfiguration _configuration;
        private string connectionString;

        public StankModel(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration["ConnectionString"];
        }

        public DataTable GetAllStank()
        {
            MySqlConnection dbcon = new MySqlConnection(connectionString);
            dbcon.Open();
            MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM Stank;", dbcon);
            DataSet ds = new DataSet();
            adapter.Fill(ds, "result");
            DataTable stankTable = ds.Tables["result"];
            dbcon.Close();
            return stankTable;

        }
    }
}
