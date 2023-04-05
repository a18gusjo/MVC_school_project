using MySql.Data.MySqlClient;
using System.Data;

namespace ModelViewController.Models
{
    public class RenspannModel
    {

        private IConfiguration _configuration;
        private string connectionString;

        public RenspannModel(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration["ConnectionString"];
        }

        public DataTable GetAllRenSpann()
        {
            MySqlConnection dbcon = new MySqlConnection(connectionString);
            dbcon.Open();
            MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM Renspann;", dbcon);
            DataSet ds = new DataSet();
            adapter.Fill(ds, "result");
            DataTable renspannTable = ds.Tables["result"];
            dbcon.Close();
            return renspannTable;
        }
        public void InsertRenspann(string namn, int kapacitet)
        {
            MySqlConnection dbcon = new MySqlConnection(connectionString);
            dbcon.Open();
            string InsertString = "INSERT INTO Renspann(namn, kapacitet) VALUES(@namn, @kapacitet);";
            MySqlCommand sqlCmd = new MySqlCommand(InsertString, dbcon);
            sqlCmd.Parameters.AddWithValue("@namn", namn);
            sqlCmd.Parameters.AddWithValue("@kapacitet", kapacitet);

            int rows = sqlCmd.ExecuteNonQuery();
            dbcon.Close();
        }

        public void DeleteRenspann(string namn)
        {
            MySqlConnection dbcon = new MySqlConnection(connectionString);
            dbcon.Open();
            string InsertString = "DELETE FROM Renspann WHERE namn = @namn;";
            MySqlCommand sqlCmd = new MySqlCommand(InsertString, dbcon);
            sqlCmd.Parameters.AddWithValue("@namn", namn);
            int rows = sqlCmd.ExecuteNonQuery();
            dbcon.Close();
        }

    }
}
