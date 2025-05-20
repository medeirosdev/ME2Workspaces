using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MySqlConnector;

var config = new ConfigurationBuilder()
  .SetBasePath(Directory.GetCurrentDirectory())
  .AddJsonFile("appsettings.json", optional: false)
  .Build();

string connString = config.GetConnectionString("MySql");
var smtpCfg = config.GetSection("Smtp");

async Task SendNotificationsAsync()
{
    using var db = new MySqlConnection(connString);
    await db.OpenAsync();

    var today = DateTime.Today;

    var plus7 = today.AddDays(7);
    var toWarn7 = await db.QueryAsync<dynamic>(@"
    SELECT t.Id, t.Descricao, t.Prazo, i.Email, i.Nome
      FROM influencer_task t
      JOIN tabela_influencers_data i
        ON t.ID_Influencer = i.ID
     WHERE t.Feito = 0
       AND DATE(t.Prazo) = @Plus7",
      new { Plus7 = plus7 });

    // 2) Tarefas com prazo em 1 dia
    var plus1 = today.AddDays(1);
    var toWarn1 = await db.QueryAsync<dynamic>(@"
    SELECT t.Id, t.Descricao, t.Prazo, i.Email, i.Nome
      FROM influencer_task t
      JOIN tabela_influencers_data i
        ON t.ID_Influencer = i.ID
     WHERE t.Feito = 0
       AND DATE(t.Prazo) = @Plus1",
      new { Plus1 = plus1 });

    // 3) Influencers com mais de 5 tarefas pendentes
    var over5 = await db.QueryAsync<dynamic>(@"
    SELECT i.ID as InfluencerId, i.Email, i.Nome, COUNT(*) as Pendentes
      FROM influencer_task t
      JOIN tabela_influencers_data i
        ON t.ID_Influencer = i.ID
     WHERE t.Feito = 0
     GROUP BY i.ID, i.Email, i.Nome
    HAVING COUNT(*) > 5");

    // Função para enviar e-mail
    async Task SendEmail(string to, string subject, string body)
    {
        var msg = new MimeMessage();
        msg.From.Add(new MailboxAddress("Me2 Notifier", smtpCfg["User"]));
        msg.To.Add(MailboxAddress.Parse(to));
        msg.Subject = subject;
        msg.Body = new TextPart("plain") { Text = body };

        using var client = new SmtpClient();
        await client.ConnectAsync(smtpCfg["Host"], int.Parse(smtpCfg["Port"]), SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(smtpCfg["User"], smtpCfg["Pass"]);
        await client.SendAsync(msg);
        await client.DisconnectAsync(true);
    }

    foreach (var x in toWarn7)
    {
        var body = $@"
Olá {x.Nome},

sua tarefa #{x.Id}: {x.Descricao}
vence em 7 dias, em {((DateTime)x.Prazo):dd/MM/yyyy}.

Por favor, verifique e marque como concluída quando terminar.";

        await SendEmail(x.Email, "Tarefa vence em 7 dias", body);
    }

    foreach (var x in toWarn1)
    {
        var body = $@"
Olá {x.Nome},

sua tarefa #{x.Id}: {x.Descricao}
vence amanhã ({((DateTime)x.Prazo):dd/MM/yyyy}).

Não deixe vencer sem concluir.";

        await SendEmail(x.Email, "Tarefa vence amanhã", body);
    }

    // Dispara aviso de mais de 5 pendentes
    foreach (var x in over5)
    {
        var body = $@"
Olá {x.Nome},

você tem {x.Pendentes} tarefas pendentes.
Dê uma olhada no painel e organize seu workflow.";

        await SendEmail(x.Email, "Mais de 5 tarefas pendentes", body);
    }
}

async Task MainLoop()
{
    while (true)
    {
        try
        {
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm} — iniciando checagem");
            await SendNotificationsAsync();
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm} — fim da checagem");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
        }

        // aguarda 24 horas
        await Task.Delay(TimeSpan.FromHours(24));
    }
}

// entry point
await MainLoop();
