namespace ME2Workspaces.ModulosME2.FeedbackUsuarios
{
    public class FeedbackUsuario
    {
        public long Id_Feedback { get; set; }
        public string NomeUsuario { get; set; }
        public string EmailUsuario { get; set; }
        public string Feedback { get; set; }
        public bool Resolvido { get; set; }
        public DateTime? DataResolucao { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
