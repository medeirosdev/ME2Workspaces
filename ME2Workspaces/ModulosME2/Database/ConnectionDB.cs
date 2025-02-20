using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using Dapper;

namespace Me2Workspaces.ModulosME2.Database
{
    public class ConnectionDB
    {
        // Configurações da conexão
        public static string server { get; set; } = "srv723.hstgr.io";
        public static string database { get; set; }  = "u307563222_me2";
        public static string user { get; set; } = "u307563222_me2";
        public static string password { get; set; } = "me2me2me2167A";
        public static string port = "3306";

        string connectionString { get; set; } = $"Server={server};Database={database};User ID={user};Password={password};Port={port};";


        public async Task<MySqlConnection> NewConnection()
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(connectionString);
                await conn.OpenAsync(); 
                return conn;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return null;
            }
        }

        public class Tokens
        {
            public string token { get; set; }
        }

        public async Task<Tokens?> GetTokenByID(long id)
        {
            MySqlConnection conn;
            ConnectionDB connService = new ConnectionDB();

            conn = await connService.NewConnection();

            if (conn == null) { return null; }

            try
            {
                string query = @"SELECT * FROM u307563222_me2.tokens WHERE id = " + id;

                conn = await connService.NewConnection();

                var result = await conn.QueryAsync<Tokens>(query);

                if (result != null)
                {
                    return result.SingleOrDefault();
                }

                return null;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                await conn.CloseAsync();
                await conn.DisposeAsync();
            }
        }

    }


}
