using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Skynetz.Application.Simulacoes.Commands;
using Skynetz.Web.App_Start;

namespace Skynetz.Web
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarOpcoes();
                CarregarHistorico();
            }
        }

        private void CarregarOpcoes()
        {
            var opcoes = ApplicationComposition
                .CriarCasoDeUsoObterOpcoesSimulacao()
                .Executar();

            ddlOrigem.DataSource = opcoes.DddsOrigem;
            ddlOrigem.DataBind();
            ddlOrigem.Items.Insert(0, new ListItem("Selecione...", ""));

            ddlDestino.DataSource = opcoes.DddsDestino;
            ddlDestino.DataBind();
            ddlDestino.Items.Insert(0, new ListItem("Selecione...", ""));

            ddlPlano.DataSource = opcoes.Planos;
            ddlPlano.DataTextField = "Nome";
            ddlPlano.DataValueField = "Nome";
            ddlPlano.DataBind();
            ddlPlano.Items.Insert(0, new ListItem("Selecione...", ""));
        }

        private void CarregarHistorico()
        {
            gvHistorico.DataSource = ApplicationComposition
                .CriarCasoDeUsoListarHistorico()
                .Executar(20);

            gvHistorico.DataBind();
        }

        protected void btnCalcular_Click(object sender, EventArgs e)
        {
            pnlResultado.Visible = false;
            lblMensagem.Text = "";

            if (string.IsNullOrWhiteSpace(ddlOrigem.SelectedValue) ||
                string.IsNullOrWhiteSpace(ddlDestino.SelectedValue) ||
                string.IsNullOrWhiteSpace(ddlPlano.SelectedValue))
            {
                lblMensagem.Text = "Selecione origem, destino e plano.";
                return;
            }

            int minutos;
            if (!int.TryParse(txtMinutos.Text, out minutos) || minutos < 0)
            {
                lblMensagem.Text =
                    "Informe um número inteiro de minutos igual ou maior que zero.";
                return;
            }

            try
            {
                var command = new CalcularSimulacaoCommand(
                    ddlOrigem.SelectedValue,
                    ddlDestino.SelectedValue,
                    minutos,
                    ddlPlano.SelectedValue);

                var resultado = ApplicationComposition
                    .CriarCasoDeUsoCalcularSimulacao()
                    .Executar(command);

                var culturaBrasileira =
                    CultureInfo.GetCultureInfo("pt-BR");

                lblValorComPlano.Text =
                    resultado.ValorComPlano.ToString("C", culturaBrasileira);

                lblValorSemPlano.Text =
                    resultado.ValorSemPlano.ToString("C", culturaBrasileira);

                lblMinutosExcedentes.Text =
                    resultado.MinutosExcedentes.ToString();

                pnlResultado.Visible = true;

                CarregarHistorico();
            }
            catch (InvalidOperationException ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void gvHistorico_RowDataBound(
            object sender,
            GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;

            var simulacao =
                (Skynetz.Application.Simulacoes.DTOs.SimulacaoHistoricoDto)e.Row.DataItem;

            // O SQL Server/EF pode materializar o DateTime sem Kind.
            var dataUtc = DateTime.SpecifyKind(
                simulacao.CriadaEmUtc,
                DateTimeKind.Utc);

            var fusoBrasilia =
                TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");

            var dataBrasilia =
                TimeZoneInfo.ConvertTimeFromUtc(dataUtc, fusoBrasilia);

            e.Row.Cells[0].Text = dataBrasilia.ToString(
                "dd/MM/yyyy HH:mm",
                CultureInfo.GetCultureInfo("pt-BR"));
        }
    }
}