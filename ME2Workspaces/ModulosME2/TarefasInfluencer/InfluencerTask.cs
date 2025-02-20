namespace ME2Workspaces.ModulosME2.TarefasInfluencer
{
    // Enum para definir os tipos de tarefa
    public enum TipoTarefa
    {
        Stories,      // Valor 0
        Publicacao,   // Valor 1
        Reels         // Valor 2
    }

    // Modelo de tarefa para o influenciador da campanha
    public class InfluencerTask
    {
        /// <summary>
        /// Identificador único da tarefa.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Identificador da campanha à qual a tarefa pertence.
        /// </summary>
        public long ID_Campanha { get; set; }

        /// <summary>
        /// Identificador do influenciador associado à tarefa.
        /// </summary>
        public long ID_Influencer { get; set; }

        /// <summary>
        /// Descrição da tarefa.
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Tipo da tarefa (Stories, Publicacao, Reels).
        /// </summary>
        public TipoTarefa Tipo_Tarefa { get; set; }

        /// <summary>
        /// Prazo para conclusão da tarefa.
        /// </summary>
        public DateTime? Prazo { get; set; }

        /// <summary>
        /// Data de criação da tarefa.
        /// </summary>
        public DateTime DataCriacao { get; set; }

        /// <summary>
        /// Indica se a tarefa foi concluída (true) ou não (false).
        /// </summary>
        public bool Feito { get; set; }
    }
}
