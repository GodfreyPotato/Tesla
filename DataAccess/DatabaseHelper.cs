using MySql.Data.MySqlClient;
using System.Data;

namespace practiceQuiz.DataAccess
{
    public class DatabaseHelper
    {
        
        private readonly string _conStr;
         public DatabaseHelper()
        {
            _conStr = "server=localhost; username=root; password=''; database=tesla";
        }

        public DataTable read(string query)
        {
            DataTable dt = new DataTable();
            using(MySqlConnection con = new MySqlConnection(_conStr))
            {
                con.Open();
                using(MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    using(MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        public int execute(string query)
        {
            using (MySqlConnection con = new MySqlConnection(_conStr))
            {
                con.Open();
                using(MySqlCommand cmd = new MySqlCommand(query, con)) {

                    return cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
