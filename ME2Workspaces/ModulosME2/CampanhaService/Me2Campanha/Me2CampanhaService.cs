using Me2Workspaces.ModulosME2.Database;
using Dapper;
using MySql.Data.MySqlClient;

namespace Me2Workspaces.ModulosME2.CampanhaService.Me2Campanha
{
    public class Me2CampanhaService
    {
        public ConnectionDB connectionDB { get; set; } = new ConnectionDB();

        // Create
        public async Task<int> CreateCampanha(Me2CampanhaModelo campanha)
        {
            using (var conn = await connectionDB.NewConnection())
            {
                if (conn == null) return -1;

                var query = @"INSERT INTO me2campanha (empresaid, datainicio, datatermino, totalinvestimento, nomecampanha, descricaocampanha, categoriacampanha1, categoriacampanha2, categoriacampanha3, descricaoadicional, descricaoempresas_associadas) 
                          VALUES (@EmpresaId, @DataInicio, @DataTermino, @TotalInvestimento, @NomeCampanha, @DescricaoCampanha, @CategoriaCampanha1, @CategoriaCampanha2, @CategoriaCampanha3, @DescricaoAdicional, @DescricaoEmpresasAssociadas);
                          SELECT LAST_INSERT_ID();";

                return await conn.ExecuteScalarAsync<int>(query, campanha);
            }
        }

        // Read by Id
        public async Task<Me2CampanhaModelo> GetCampanhaById(long id)
        {
            using (var conn = await connectionDB.NewConnection())
            {
                if (conn == null) return null;

                var query = "SELECT * FROM me2campanha WHERE id = @Id";
                return await conn.QueryFirstOrDefaultAsync<Me2CampanhaModelo>(query, new { Id = id });
            }
        }

        // Update
        public async Task<bool> UpdateCampanha(Me2CampanhaModelo campanha)
        {
            using (var conn = await connectionDB.NewConnection())
            {
                if (conn == null) return false;

                var query = @"UPDATE me2campanha 
                          SET empresaid = @EmpresaId, 
                              datainicio = @DataInicio, 
                              datatermino = @DataTermino, 
                              totalinvestimento = @TotalInvestimento, 
                              nomecampanha = @NomeCampanha, 
                              descricaocampanha = @DescricaoCampanha, 
                              categoriacampanha1 = @CategoriaCampanha1, 
                              categoriacampanha2 = @CategoriaCampanha2, 
                              categoriacampanha3 = @CategoriaCampanha3, 
                              descricaoadicional = @DescricaoAdicional, 
                              descricaoempresas_associadas = @DescricaoEmpresasAssociadas  
                          WHERE id = @Id";

                var result = await conn.ExecuteAsync(query, campanha);
                return result > 0;
            }
        }

        // Delete
        public async Task<bool> DeleteCampanha(int id)
        {
            using (var conn = await connectionDB.NewConnection())
            {
                if (conn == null) return false;

                var query = "DELETE FROM me2campanha WHERE id = @Id";
                var result = await conn.ExecuteAsync(query, new { Id = id });
                return result > 0;
            }
        }

        public async Task<int> ObterNumeroCampanhas(long usuarioId)
        {
            using var connection = await connectionDB.NewConnection();
            if (connection == null) return 0;

            string sql = "SELECT COUNT(*) FROM me2campanha WHERE empresaid = @UsuarioId;";
            return await connection.ExecuteScalarAsync<int>(sql, new { UsuarioId = usuarioId });
        }

        // List all campaigns by company Id
        public async Task<List<Me2CampanhaModelo>> GetCampanhasByEmpresaId(long empresaId)
        {
            using (var conn = await connectionDB.NewConnection())
            {
                if (conn == null) return new List<Me2CampanhaModelo>();

                try
                {
                    var query = "SELECT * FROM me2campanha WHERE empresaid = @EmpresaId";
                    var result = await conn.QueryAsync<Me2CampanhaModelo>(query, new { EmpresaId = empresaId });
                    return result?.ToList() ?? new List<Me2CampanhaModelo>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao buscar campanhas: {ex.Message}");
                    return new List<Me2CampanhaModelo>();
                }
            }
        }
    }

    public class Me2CampanhaModelo
    {
        public long Id { get; set; }
        public long EmpresaId { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataTermino { get; set; }
        public decimal TotalInvestimento { get; set; }
        public string NomeCampanha { get; set; }
        public string DescricaoCampanha { get; set; }
        public int CategoriaCampanha1 { get; set; }
        public int CategoriaCampanha2 { get; set; }
        public int CategoriaCampanha3 { get; set; }
        public string DescricaoAdicional { get; set; }
        public string DescricaoEmpresasAssociadas { get; set; }
    }
}
