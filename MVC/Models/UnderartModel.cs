using MySql.Data.MySqlClient;
using System.Data;

namespace ModelViewController.Models
{
    public class UnderartModel
    {
        private IConfiguration _configuration;
        private string connectionString;

        public UnderartModel(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration["ConnectionString"];
        }

        public DataTable GetAllUnderart()
        {
            MySqlConnection dbcon = new MySqlConnection(connectionString);
            dbcon.Open();
            MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM Underart;", dbcon);
            DataSet ds = new DataSet();
            adapter.Fill(ds, "result");
            DataTable underartTable = ds.Tables["result"];
            dbcon.Close();
            return underartTable;

        }
    }
}
