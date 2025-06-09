using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using Me2Workspaces.ModulosME2.Database;
using static ME2Workspaces.ModulosME2.ME2GestaoInterna.ClsGestaoInterna;

namespace ME2Workspaces.ModulosME2.ME2GestaoInterna
{
    public class GestaoInternaServico
    {
        private readonly ConnectionDB connectionDB = new ConnectionDB();

        #region Workspace CRUD

        public async Task<long> CreateWorkspace(ClsGestaoInterna.Workspace workspace)
        {
            const string sql = @"
                INSERT INTO workspace (nome, descricao, criado_por, criado_em, data_inicio, data_fim)
                VALUES (@Nome, @Descricao, @CriadoPor, @CriadoEm, @DataInicio, @DataFim);
                SELECT LAST_INSERT_ID();";

            using var connection = await connectionDB.NewConnection();
            return await connection.ExecuteScalarAsync<long>(sql, workspace);
        }

        public async Task<ClsGestaoInterna.Workspace?> GetWorkspaceById(long id)
        {
            const string sql = "SELECT * FROM workspace WHERE id = @Id;";
            using var connection = await connectionDB.NewConnection();
            return await connection.QueryFirstOrDefaultAsync<ClsGestaoInterna.Workspace>(sql, new { Id = id });
        }

        public async Task<IEnumerable<ClsGestaoInterna.Workspace>> GetAllWorkspaces()
        {
            const string sql = "SELECT * FROM workspace;";
            using var connection = await connectionDB.NewConnection();
            return await connection.QueryAsync<ClsGestaoInterna.Workspace>(sql);
        }

        public async Task<bool> UpdateWorkspace(ClsGestaoInterna.Workspace workspace)
        {
            const string sql = @"
                UPDATE workspace SET
                    nome = @Nome,
                    descricao = @Descricao,
                    criado_por = @CriadoPor,
                    criado_em = @CriadoEm,
                    data_inicio = @DataInicio,
                    data_fim = @DataFim
                WHERE id = @Id;";

            using var connection = await connectionDB.NewConnection();
            var affected = await connection.ExecuteAsync(sql, workspace);
            return affected > 0;
        }

        public async Task<bool> DeleteWorkspace(long id)
        {
            const string sql = "DELETE FROM workspace WHERE id = @Id;";
            using var connection = await connectionDB.NewConnection();
            var affected = await connection.ExecuteAsync(sql, new { Id = id });
            return affected > 0;
        }

        #endregion

        #region TaskGroup CRUD

        public async Task<long> CreateTaskGroup(ClsGestaoInterna.TaskGroup group)
        {
            const string sql = @"
                INSERT INTO task_group (workspace_id, nome, descricao, criado_por, criado_em, data_inicio, data_fim, prioridade)
                VALUES (@WorkspaceId, @Nome, @Descricao, @CriadoPor, @CriadoEm, @DataInicio, @DataFim, @Prioridade);
                SELECT LAST_INSERT_ID();";

            using var connection = await connectionDB.NewConnection();
            return await connection.ExecuteScalarAsync<long>(sql, group);
        }

        public async Task<ClsGestaoInterna.TaskGroup?> GetTaskGroupById(long id)
        {
            const string sql = "SELECT * FROM task_group WHERE id = @Id;";
            using var connection = await connectionDB.NewConnection();
            return await connection.QueryFirstOrDefaultAsync<ClsGestaoInterna.TaskGroup>(sql, new { Id = id });
        }

        public async Task<IEnumerable<ClsGestaoInterna.TaskGroup>> GetGroupsByWorkspace(long workspaceId)
        {
            const string sql = "SELECT * FROM task_group WHERE workspace_id = @WorkspaceId;";
            using var connection = await connectionDB.NewConnection();
            return await connection.QueryAsync<ClsGestaoInterna.TaskGroup>(sql, new { WorkspaceId = workspaceId });
        }

        public async Task<bool> UpdateTaskGroup(ClsGestaoInterna.TaskGroup group)
        {
            const string sql = @"
                UPDATE task_group SET
                    workspace_id = @WorkspaceId,
                    nome = @Nome,
                    descricao = @Descricao,
                    criado_por = @CriadoPor,
                    criado_em = @CriadoEm,
                    data_inicio = @DataInicio,
                    data_fim = @DataFim,
                    prioridade = @Prioridade
                WHERE id = @Id;";

            using var connection = await connectionDB.NewConnection();
            var affected = await connection.ExecuteAsync(sql, group);
            return affected > 0;
        }

        public async Task<bool> DeleteTaskGroup(long id)
        {
            const string sql = "DELETE FROM task_group WHERE id = @Id;";
            using var connection = await connectionDB.NewConnection();
            var affected = await connection.ExecuteAsync(sql, new { Id = id });
            return affected > 0;
        }

        #endregion

        #region TaskInterno CRUD

        public async Task<long> CreateTaskInterno(ClsGestaoInterna.TaskInterno task)
        {
            const string sql = @"
                INSERT INTO task (
                    task_group_id,
                    titulo,
                    descricao,
                    criado_por,
                    criado_em,
                    data_inicio,
                    data_vencimento,
                    feito,
                    responsaveis,
                    prioridade,
                    status
                ) VALUES (
                    @TaskGroupId,
                    @Titulo,
                    @Descricao,
                    @CriadoPor,
                    @CriadoEm,
                    @DataInicio,
                    @DataVencimento,
                    @Feito,
                    @Responsaveis,
                    @Prioridade,
                    @Status
                );
                SELECT LAST_INSERT_ID();";

            using var connection = await connectionDB.NewConnection();
            return await connection.ExecuteScalarAsync<long>(sql, task);
        }

        public async Task<ClsGestaoInterna.TaskInterno?> GetTaskInternoById(long id)
        {
            const string sql = "SELECT * FROM task WHERE id = @Id;";
            using var connection = await connectionDB.NewConnection();
            return await connection.QueryFirstOrDefaultAsync<ClsGestaoInterna.TaskInterno>(sql, new { Id = id });
        }

        public async Task<IEnumerable<ClsGestaoInterna.TaskInterno>> GetTasksByGroup(long groupId)
        {
            const string sql = "SELECT * FROM task WHERE task_group_id = @GroupId;";
            using var connection = await connectionDB.NewConnection();
            return await connection.QueryAsync<ClsGestaoInterna.TaskInterno>(sql, new { GroupId = groupId });
        }

        public async Task<bool> UpdateTaskInterno(ClsGestaoInterna.TaskInterno task)
        {
            const string sql = @"
                UPDATE task SET
                    task_group_id = @TaskGroupId,
                    titulo = @Titulo,
                    descricao = @Descricao,
                    criado_por = @CriadoPor,
                    criado_em = @CriadoEm,
                    data_inicio = @DataInicio,
                    data_vencimento = @DataVencimento,
                    feito = @Feito,
                    responsaveis = @Responsaveis,
                    prioridade = @Prioridade,
                    status = @Status
                WHERE id = @Id;";

            using var connection = await connectionDB.NewConnection();
            var affected = await connection.ExecuteAsync(sql, task);
            return affected > 0;
        }

        public async Task<bool> DeleteTaskInterno(long id)
        {
            const string sql = "DELETE FROM task WHERE id = @Id;";
            using var connection = await connectionDB.NewConnection();
            var affected = await connection.ExecuteAsync(sql, new { Id = id });
            return affected > 0;
        }

        #endregion
    }


}

