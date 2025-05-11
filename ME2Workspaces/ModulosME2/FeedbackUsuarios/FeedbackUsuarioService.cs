using Dapper;
using Me2Workspaces.ModulosME2.Database;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ME2Workspaces.ModulosME2.FeedbackUsuarios
{
    public class FeedbackUsuarioService
    {
        // Cria a conexão utilizando a classe ConnectionDB
        private readonly ConnectionDB _connectionDB = new ConnectionDB();


        public async Task<long> CreateFeedback(FeedbackUsuario feedback)
        {
            using (var connection = await _connectionDB.NewConnection())
            {
                var query = @"
                    INSERT INTO feedback_usuario (NomeUsuario, EmailUsuario, Feedback, Resolvido, DataCriacao)
                    VALUES (@NomeUsuario, @EmailUsuario, @Feedback, @Resolvido, @DataCriacao);
                    SELECT LAST_INSERT_ID();"; // Ajuste para o seu SGBD, se necessário
                return await connection.QueryFirstOrDefaultAsync<long>(query, feedback);
            }
        }

        public async Task<FeedbackUsuario> GetFeedbackById(long id)
        {
            using (var connection = await _connectionDB.NewConnection())
            {
                var query = "SELECT * FROM feedback_usuario WHERE Id_Feedback = @Id";
                return await connection.QueryFirstOrDefaultAsync<FeedbackUsuario>(query, new { Id = id });
            }
        }

        public async Task<IEnumerable<FeedbackUsuario>> GetAllFeedbacks()
        {
            using (var connection = await _connectionDB.NewConnection())
            {
                var query = "SELECT * FROM feedback_usuario";
                return await connection.QueryAsync<FeedbackUsuario>(query);
            }
        }

        public async Task<bool> UpdateFeedback(FeedbackUsuario feedback)
        {
            using (var connection = await _connectionDB.NewConnection())
            {
                var query = @"
                    UPDATE feedback_usuario SET
                        NomeUsuario = @NomeUsuario,
                        EmailUsuario = @EmailUsuario,
                        Feedback = @Feedback,
                        Resolvido = @Resolvido,
                        DataResolucao = @DataResolucao
                    WHERE Id_Feedback = @Id_Feedback;"; // Adicionado WHERE para especificar qual feedback atualizar
                var rowsAffected = await connection.ExecuteAsync(query, feedback);
                return rowsAffected > 0;
            }
        }

        public async Task<bool> DeleteFeedback(long id)
        {
            using (var connection = await _connectionDB.NewConnection())
            {
                var query = "DELETE FROM feedback_usuario WHERE Id_Feedback = @Id";
                var rowsAffected = await connection.ExecuteAsync(query, new { Id = id });
                return rowsAffected > 0;
            }
        }

        public async Task<bool> MarkAsResolved(long id, DateTime resolutionDate)
        {
            using (var connection = await _connectionDB.NewConnection())
            {
                var query = "UPDATE feedback_usuario SET Resolvido = 1, DataResolucao = @ResolutionDate WHERE Id_Feedback = @Id";
                var parameters = new { Id = id, ResolutionDate = resolutionDate };
                var rowsAffected = await connection.ExecuteAsync(query, parameters);
                return rowsAffected > 0;
            }
        }

        public async Task<bool> MarkAsUnresolved(long id)
        {
            using (var connection = await _connectionDB.NewConnection())
            {
                var query = "UPDATE feedback_usuario SET Resolvido = 0, DataResolucao = NULL WHERE Id_Feedback = @Id";
                var parameters = new { Id = id };
                var rowsAffected = await connection.ExecuteAsync(query, parameters);
                return rowsAffected > 0;
            }
        }
    }
}

