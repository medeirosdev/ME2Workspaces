using Me2Workspaces.ModulosME2.Database;
using Dapper;

namespace Me2Workspaces.ModulosME2.DBUser
{
    public class UserService
    {
        public ConnectionDB connectionDB { get; set; } = new ConnectionDB();

        public class AuthUserModel
        {
            public long Id { get; set; }
            public string Username { get; set; }

            public int TipoDaConta { get; set; }

            public bool ADMIN { get; set; }
            public int TipoDoContrato { get; set; }

            public string Email { get; set; }

            public string Token { get; set; }

            public string Senha { get; set; }

            public string InformacoesEmpresa { get; set; }
        }
        public async Task<bool> CreateUser(AuthUserModel user)
        {
            try
            {


                string query = @"INSERT INTO usuario (Username, TipoDaConta, ADMIN, TipoDoContrato, Email, Token, Senha, InformacoesEmpresa)
                     VALUES (@Username, @TipoDaConta, @ADMIN, @TipoDoContrato, @Email, @Token, @Senha, @InformacoesEmpresa)";

                using (var conn = await connectionDB.NewConnection())
                {
                    if (conn == null) return false;

                    var result = await conn.ExecuteAsync(query, user);
                    return result > 0;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateUser(AuthUserModel user)
        {
            try
            {


                string query = @"UPDATE usuario 
                     SET Username = @Username, TipoDaConta = @TipoDaConta, ADMIN = @ADMIN, TipoDoContrato = @TipoDoContrato, 
                         Email = @Email, Token = @Token, Senha = @Senha, InformacoesEmpresa = @InformacoesEmpresa
                     WHERE Id = @Id";

                using (var conn = await connectionDB.NewConnection())
                {
                    if (conn == null) return false;

                    var result = await conn.ExecuteAsync(query, user);
                    return result > 0;
                }

            }
            catch (Exception ex)
            {
              
                return false;
            }
        }

        public async Task<IEnumerable<AuthUserModel>> GetNonAdminUsers()
        {
            try
            {

            

            string query = @"SELECT Id, Username, TipoDaConta, ADMIN, TipoDoContrato, 
                            Email, Token, Senha, InformacoesEmpresa
                     FROM usuario
                     WHERE ADMIN = false";

            using (var conn = await connectionDB.NewConnection())
            {
                if (conn == null) return null;

                var users = await conn.QueryAsync<AuthUserModel>(query);
                return users;
            }

            }
            catch (Exception ex)
            {
              
                return [];
            }
        }


        public async Task<AuthUserModel> GetUserByIdAndPassword(int id, string senha)
        {
            string query = @"SELECT * FROM usuario WHERE Id = @Id AND Senha = @Senha";

            using (var conn = await connectionDB.NewConnection())
            {
                if (conn == null) return null;

                var user = await conn.QueryFirstOrDefaultAsync<AuthUserModel>(query, new { Id = id, Senha = senha });
                return user;
            }
        }

        public static async Task<AuthUserModel> GetUserByEmail(string email)
        {
            string query = @"SELECT * FROM usuario WHERE email = @email";
            ConnectionDB connectionDB = new();
            using (var conn = await connectionDB.NewConnection())
            {
                if (conn == null) return null;

                var user = await conn.QueryFirstOrDefaultAsync<AuthUserModel>(query, new { email = email });
                return user;
            }
        }

        public async Task<bool> UserExists(int id, string senha)
        {
            string query = @"SELECT COUNT(1) FROM usuario WHERE Id = @Id AND Senha = @Senha";

            using (var conn = await connectionDB.NewConnection())
            {
                if (conn == null) return false;

                var result = await conn.ExecuteScalarAsync<int>(query, new { Id = id, Senha = senha });
                return result > 0;
            }
        }

        public async Task<AuthUserModel> GetUserByEmailAndPassword(string email, string senha)
        {
            string query = @"SELECT * FROM usuario WHERE Email = @Email AND Senha = @Senha";

            using (var conn = await connectionDB.NewConnection())
            {
                if (conn == null) return null;

                var user = await conn.QueryFirstOrDefaultAsync<AuthUserModel>(query, new { Email = email, Senha = senha });
                return user;
            }
        }

        public async Task<bool> UserExistsByEmail(string email, string senha)
        {
            string query = @"SELECT COUNT(1) FROM usuario WHERE Email = @Email AND Senha = @Senha";

            using (var conn = await connectionDB.NewConnection())
            {
                if (conn == null) return false;

                var result = await conn.ExecuteScalarAsync<int>(query, new { Email = email, Senha = senha });
                return result > 0;
            }
        }



    }
}
