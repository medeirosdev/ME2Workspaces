using Me2Workspaces.ModulosME2.Me2InstagramCheck;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;

namespace ME2Workspaces.ModulosME2.ME2GestaoInterna
{
    public class ClsGestaoInterna
    {
        public enum TaskStatus
        {
            ABERTO,
            EM_PROGRESSO,
            REVISANDO,
            CONCLUIDO
        }

        public enum PriorityLevel
        {
            BAIXA,
            MEDIA,
            ALTA
        }

        public class Workspace
        {
            public long Id { get; set; }
            public string? Nome { get; set; }
            public string? Descricao { get; set; }
            public long CriadoPor { get; set; }
            public DateTimeOffset CriadoEm { get; set; }
            public DateTime? DataInicio { get; set; }
            public DateTime? DataFim { get; set; }
        }

        public class TaskGroup
        {
            public long Id { get; set; }
            public long? WorkspaceId { get; set; }
            public string? Nome { get; set; }
            public string? Descricao { get; set; }
            public long CriadoPor { get; set; }
            public DateTimeOffset CriadoEm { get; set; }
            public DateTime? DataInicio { get; set; }
            public DateTime? DataFim { get; set; }
            public PriorityLevel Prioridade { get; set; }
        }

        public class TaskInterno
        {
            public long Id { get; set; }
            public long? TaskGroupId { get; set; }
            public string? Titulo { get; set; }
            public string? Descricao { get; set; }
            public long CriadoPor { get; set; }
            public DateTimeOffset CriadoEm { get; set; }
            public DateTime? DataInicio { get; set; }
            public DateTime? DataVencimento { get; set; }
            public bool Feito { get; set; }
            public long[] Responsaveis { get; set; } = Array.Empty<long>();
            public PriorityLevel Prioridade { get; set; }
            public TaskStatus Status { get; set; }
        }
    }
}


//--1.Definição dos tipos enum para status e prioridade
//CREATE TYPE task_status AS ENUM (
//    'ABERTO',
//    'EM PROGRESSO',
//    'REVISANDO',
//    'CONCLUIDO'
//);

//CREATE TYPE priority_level AS ENUM (
//    'BAIXA',
//    'MEDIA',
//    'ALTA'
//);

//--2.Tabela de workspaces
//CREATE TABLE workspace (
//    id               BIGSERIAL       PRIMARY KEY,
//    nome             VARCHAR(100)    DEFAULT '',
//    descricao        TEXT            DEFAULT '',
//    criado_por       BIGINT          DEFAULT 0,
//    criado_em        TIMESTAMPTZ     DEFAULT NOW(),
//    data_inicio      DATE            DEFAULT NULL,
//    data_fim         DATE            DEFAULT NULL,
//    CONSTRAINT chk_workspace_datas CHECK (
//        data_inicio IS NULL OR 
//        data_fim    IS NULL OR 
//        data_inicio <= data_fim
//    )
//);

//--3.Tabela de grupos de tarefas (com prioridade)
//CREATE TABLE task_group (
//    id               BIGSERIAL       PRIMARY KEY,
//    workspace_id     BIGINT          DEFAULT NULL
//                           REFERENCES workspace(id) ON DELETE CASCADE,
//    nome             VARCHAR(100)    DEFAULT '',
//    descricao        TEXT            DEFAULT '',
//    criado_por       BIGINT          DEFAULT 0,
//    criado_em        TIMESTAMPTZ     DEFAULT NOW(),
//    data_inicio      DATE            DEFAULT NULL,
//    data_fim         DATE            DEFAULT NULL,
//    prioridade       priority_level  DEFAULT 'MEDIA',
//    CONSTRAINT chk_task_group_datas CHECK (
//        data_inicio IS NULL OR 
//        data_fim    IS NULL OR 
//        data_inicio <= data_fim
//    )
//);

//--4.Tabela de tarefas individuais (com status e prioridade)
//CREATE TABLE task (
//    id               BIGSERIAL       PRIMARY KEY,
//    task_group_id    BIGINT          DEFAULT NULL
//                           REFERENCES task_group(id) ON DELETE CASCADE,
//    titulo           VARCHAR(150)    DEFAULT '',
//    descricao        TEXT            DEFAULT '',
//    criado_por       BIGINT          DEFAULT 0,
//    criado_em        TIMESTAMPTZ     DEFAULT NOW(),
//    data_inicio      DATE            DEFAULT NULL,
//    data_vencimento  DATE            DEFAULT NULL,
//    feito            BOOLEAN         DEFAULT FALSE,
//    responsaveis     BIGINT[] DEFAULT '{}',
//    prioridade priority_level  DEFAULT 'MEDIA',
//    status           task_status     DEFAULT 'ABERTO',
//    CONSTRAINT chk_task_datas CHECK (
//        data_inicio IS NULL OR 
//        data_inicio <= data_vencimento
//    )
//);