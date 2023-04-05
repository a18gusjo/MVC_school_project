using MySql.Data.MySqlClient;
using System.Data;
using System.Xml.Linq;

namespace ModelViewController.Models
{
    public class SlädeModel
    {
        private IConfiguration _configuration;
        private string connectionString;

        public SlädeModel(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration["ConnectionString"];
        }

        public DataTable GetAllSläde()
        {
            MySqlConnection dbcon = new MySqlConnection(connectionString);
            dbcon.Open();
            MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT nr, namn, tillvärkare, steglängd, reg_typ FROM Släde;", dbcon);
            DataSet ds = new DataSet();
            adapter.Fill(ds, "result");
            DataTable slädeTable = ds.Tables["result"];
            dbcon.Close();
            return slädeTable;

        }

        public DataTable GetAllReg()
        {
            MySqlConnection dbcon = new MySqlConnection(connectionString);
            dbcon.Open();
            MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM Registrering;", dbcon);
            DataSet ds = new DataSet();
            adapter.Fill(ds, "result");
            DataTable regTable = ds.Tables["result"];
            dbcon.Close();
            return regTable;

        }
        
        public DataTable GetAllExpress()
        {
            MySqlConnection dbcon = new MySqlConnection(connectionString);
            dbcon.Open();
            MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM Expressläde;", dbcon);
            DataSet ds = new DataSet();
            adapter.Fill(ds, "result");
            DataTable expressTable = ds.Tables["result"];
            dbcon.Close();
            return expressTable;

        }


        public DataTable GetAllLast()
        {
            MySqlConnection dbcon = new MySqlConnection(connectionString);
            dbcon.Open();
            MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM LastSläde;", dbcon);
            DataSet ds = new DataSet();
            adapter.Fill(ds, "result");
            DataTable lastTable = ds.Tables["result"];
            dbcon.Close();
            return lastTable;

        }

        public DataTable SökSläde(string namn)
        {
            MySqlConnection dbcon = new MySqlConnection(connectionString);
            dbcon.Open();
            MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM Släde WHERE namn LIKE @namn;", dbcon);
            adapter.SelectCommand.Parameters.AddWithValue("@namn", $"%{namn}%");
            DataSet ds = new DataSet();
            adapter.Fill(ds, "result");
            DataTable slädeTable = ds.Tables["result"];
            dbcon.Close();
            return slädeTable;
        
        }

        public void UppdateraSläde(string tillvärkare, string steglängd, int reg_typ, string nr)
        {
            MySqlConnection dbcon = new MySqlConnection(connectionString);
            dbcon.Open();
            string UpdateString = "UPDATE Släde SET tillvärkare = @tillvärkare, steglängd = @steglängd, reg_typ = @reg_typ WHERE nr = @nr;";
            MySqlCommand sqlCmd = new MySqlCommand(UpdateString, dbcon);
            sqlCmd.Parameters.AddWithValue("@tillvärkare", tillvärkare);
            sqlCmd.Parameters.AddWithValue("@steglängd", steglängd);
            sqlCmd.Parameters.AddWithValue("@reg_typ", reg_typ);
            sqlCmd.Parameters.AddWithValue("@nr", nr);
            int rows = sqlCmd.ExecuteNonQuery();
            dbcon.Close();
        }

        public void InsertSläde(string nr, string namn)
        {
            MySqlConnection dbcon = new MySqlConnection(connectionString);
            dbcon.Open();
            string InsertString = "INSERT INTO Släde(nr, namn) VALUES(@nr, @namn);";
            MySqlCommand sqlCmd = new MySqlCommand(InsertString, dbcon);
            sqlCmd.Parameters.AddWithValue("@nr", nr);
            sqlCmd.Parameters.AddWithValue("@namn", namn);
            int rows = sqlCmd.ExecuteNonQuery();
            dbcon.Close();
        }


        public void InsertExpress(string släde_nr, string raketantal, string bromsverkan)
        {
            MySqlConnection dbcon = new MySqlConnection(connectionString);
            dbcon.Open();
            string InsertString = "INSERT INTO Expressläde(släde_nr, raketantal, bromsverkan) VALUES(@släde_nr, @raketantal , @bromsverkan);";
            MySqlCommand sqlCmd = new MySqlCommand(InsertString, dbcon);
            sqlCmd.Parameters.AddWithValue("@släde_nr", släde_nr);
            sqlCmd.Parameters.AddWithValue("@raketantal", raketantal);
            sqlCmd.Parameters.AddWithValue("@bromsverkan", bromsverkan);
            int rows = sqlCmd.ExecuteNonQuery();
            dbcon.Close();
        }

        public void InsertLast(string släde_nr, double extrakapacitet, string klimattyp)
        {
            MySqlConnection dbcon = new MySqlConnection(connectionString);
            dbcon.Open();
            string InsertString = "INSERT INTO LastSläde(släde_nr, extrakapacitet, klimattyp) VALUES(@släde_nr, @extrakapacitet , @klimattyp);";
            MySqlCommand sqlCmd = new MySqlCommand(InsertString, dbcon);
            sqlCmd.Parameters.AddWithValue("@släde_nr", släde_nr);
            sqlCmd.Parameters.AddWithValue("@extrakapacitet", extrakapacitet);
            sqlCmd.Parameters.AddWithValue("@klimattyp", klimattyp);
            int rows = sqlCmd.ExecuteNonQuery();
            dbcon.Close();
        }

        public void FåKapacitet(string släde_nr)
        {
            MySqlConnection dbcon = new MySqlConnection(connectionString);
            dbcon.Open();
            string ProcedureString = "CALL FåKapacitet(@släde_nr);";
            MySqlCommand sqlCmd = new MySqlCommand(ProcedureString, dbcon);
            sqlCmd.Parameters.AddWithValue("(@släde_nr", släde_nr);
            int rows = sqlCmd.ExecuteNonQuery();

            dbcon.Close();
            GeKapacitet(släde_nr);

        }

        public void DeleteSläde(string nr)
        {
            MySqlConnection dbcon = new MySqlConnection(connectionString);
            dbcon.Open();
            string DeleteString = "DELETE FROM Släde WHERE nr = @nr;";
            MySqlCommand sqlCmd = new MySqlCommand(DeleteString, dbcon);
            sqlCmd.Parameters.AddWithValue("@nr", nr);
            int rows = sqlCmd.ExecuteNonQuery();
            dbcon.Close();
        }


        public DataTable GeKapacitet(string släde_nr)
        {


            MySqlConnection dbcon = new MySqlConnection(connectionString);
           
            MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM TotalKapacitet;", dbcon);
            adapter.SelectCommand.Parameters.AddWithValue("@släde_nr", släde_nr);
            DataSet ds = new DataSet();
            adapter.Fill(ds, "result");
            DataTable slädeTable = ds.Tables["result"];
            dbcon.Close();
            return slädeTable;
        

        }
    }
}
