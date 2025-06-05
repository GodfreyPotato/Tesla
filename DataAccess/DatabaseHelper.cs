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
            using (MySqlConnection con = new MySqlConnection(_conStr))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
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
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public int executeWParameters(string query, Dictionary<string, object> parameters = null)
        {
            using (MySqlConnection con = new MySqlConnection(_conStr))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            cmd.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }
                    return cmd.ExecuteNonQuery();
                }
            }
        }


        public object scalar(string query)
        {
            object result = null;
            using (var conn = new MySqlConnection(_conStr))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    result = cmd.ExecuteScalar();
                }
            }
            return result;
        }
    }
}
