<%@ Page Title="Simulador" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs"
    Inherits="Skynetz.Web._Default" Culture="pt-BR" UICulture="pt-BR" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upSimulacao" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <main class="app-main">
                <header class="page-heading">
                    <p class="eyebrow">SKYNETZ <span aria-hidden="true">/</span> FALEMAIS</p>
                    <h1>Simule sua ligação</h1>
                    <p class="page-intro">Compare os valores antes de ligar. Escolha a rota, informe a duração e veja como o FaleMais se aplica.</p>
                </header>

                <section class="simulator-card" aria-labelledby="simulacaoTitle">
                    <div class="section-heading">
                        <div>
                            <span class="section-index" aria-hidden="true">01</span>
                            <h2 id="simulacaoTitle">Nova simulação</h2>
                        </div>
                        <p>Todos os campos são necessários</p>
                    </div>

                    <div class="form-grid">
                        <div class="field-group">
                            <asp:Label ID="lblOrigem" runat="server"
                                AssociatedControlID="ddlOrigem"
                                CssClass="form-label" Text="DDD de origem" />
                            <asp:DropDownList ID="ddlOrigem" runat="server"
                                CssClass="form-select" />
                        </div>

                        <div class="field-group">
                            <asp:Label ID="lblDestino" runat="server"
                                AssociatedControlID="ddlDestino"
                                CssClass="form-label" Text="DDD de destino" />
                            <asp:DropDownList ID="ddlDestino" runat="server"
                                CssClass="form-select" />
                        </div>

                        <div class="field-group">
                            <asp:Label ID="lblMinutos" runat="server"
                                AssociatedControlID="txtMinutos"
                                CssClass="form-label" Text="Duração da ligação" />
                            <div class="input-with-suffix">
                                <asp:TextBox ID="txtMinutos" runat="server"
                                    CssClass="form-control"
                                    TextMode="Number" min="0" step="1"
                                    inputmode="numeric" />
                                <span class="input-suffix" aria-hidden="true">min</span>
                            </div>
                        </div>

                        <div class="field-group">
                            <asp:Label ID="lblPlano" runat="server"
                                AssociatedControlID="ddlPlano"
                                CssClass="form-label" Text="Plano FaleMais" />
                            <asp:DropDownList ID="ddlPlano" runat="server"
                                CssClass="form-select" />
                        </div>
                    </div>

                    <div class="form-footer">
                        <span class="form-note">O valor de aquisição do plano não está incluído.</span>
                        <asp:Button ID="btnCalcular" runat="server"
                            Text="Calcular ligação"
                            CssClass="btn btn-primary calculate-button"
                            OnClick="btnCalcular_Click" />
                    </div>

                    <asp:Label ID="lblMensagem" runat="server"
                        CssClass="validation-message"
                        role="alert" aria-live="polite" />
                </section>

                <asp:Panel ID="pnlResultado" runat="server"
                    CssClass="results-section" Visible="false">
                    <div class="section-heading results-heading">
                        <div>
                            <span class="section-index" aria-hidden="true">02</span>
                            <h2>Comparativo da ligação</h2>
                        </div>
                        <p>Valores estimados para esta rota</p>
                    </div>

                    <div class="comparison-grid">
                        <article class="comparison-card comparison-card--plan">
                            <div class="comparison-label">
                                <span class="comparison-mark" aria-hidden="true">F</span>
                                <span>Com FaleMais</span>
                            </div>
                            <div class="comparison-value">
                                <asp:Label ID="lblValorComPlano" runat="server" />
                            </div>
                            <p class="comparison-caption">Cobrança apenas dos minutos excedentes</p>
                        </article>

                        <article class="comparison-card comparison-card--regular">
                            <div class="comparison-label">
                                <span class="comparison-mark comparison-mark--muted" aria-hidden="true">—</span>
                                <span>Sem FaleMais</span>
                            </div>
                            <div class="comparison-value comparison-value--regular">
                                <asp:Label ID="lblValorSemPlano" runat="server" />
                            </div>
                            <p class="comparison-caption">Tarifa normal por minuto</p>
                        </article>
                    </div>

                    <div id="economiaResultado" class="savings-summary" role="status" aria-live="polite" hidden>
                        <span class="savings-icon" aria-hidden="true">
                            <svg viewBox="0 0 24 24" focusable="false">
                                <path d="M5 12.5 9.5 17 19 7.5" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" />
                            </svg>
                        </span>
                        <span>
                            <strong id="economiaTitulo"></strong>
                            <span id="economiaValor" class="savings-amount"></span>
                        </span>
                    </div>

                    <p class="minutes-note">
                        <span class="minutes-note__label">Minutos excedentes</span>
                        <strong><asp:Label ID="lblMinutosExcedentes" runat="server" /></strong>
                    </p>
                </asp:Panel>

                <section class="history-section" aria-labelledby="historicoTitle">
                    <div class="section-heading history-heading">
                        <div>
                            <span class="section-index" aria-hidden="true">03</span>
                            <h2 id="historicoTitle">Histórico de simulações</h2>
                        </div>
                        <p>As 20 simulações mais recentes</p>
                    </div>

                    <div class="history-table-wrap">
                        <asp:GridView ID="gvHistorico" runat="server"
                            AutoGenerateColumns="false"
                            CssClass="table history-grid"
                            GridLines="None"
                            UseAccessibleHeader="true"
                            EmptyDataText="Ainda não há simulações registradas."
                            OnRowDataBound="gvHistorico_RowDataBound">

                            <EmptyDataTemplate>
                                <div class="empty-state" role="status">
                                    <span class="empty-state__icon" aria-hidden="true">
                                        <svg viewBox="0 0 48 48" focusable="false">
                                            <path d="M10 13.5h28M14 13.5l1.7 23h16.6l1.7-23M19 20v10m10-10v10M18 9h12l2 4.5H16L18 9Z" fill="none" stroke="currentColor" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round" />
                                        </svg>
                                    </span>
                                    <strong>Nenhuma simulação por aqui ainda</strong>
                                    <span>Faça um cálculo e ele aparecerá neste histórico.</span>
                                </div>
                            </EmptyDataTemplate>

                            <Columns>
                                <asp:BoundField DataField="CriadaEmUtc"
                                    HeaderText="Data/hora (Brasília)"
                                    DataFormatString="{0:dd/MM/yyyy HH:mm}" />

                                <asp:TemplateField HeaderText="Rota">
                                    <ItemTemplate>
                                        <span class="route-code"><%#: Eval("DddOrigem") %></span>
                                        <span class="route-arrow" aria-label="para">→</span>
                                        <span class="route-code"><%#: Eval("DddDestino") %></span>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="MinutosLigacao"
                                    HeaderText="Duração"
                                    DataFormatString="{0} min" />

                                <asp:TemplateField HeaderText="Plano">
                                    <ItemTemplate>
                                        <span class="plan-badge"><%#: Eval("NomePlano") %></span>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Excedentes">
                                    <ItemTemplate>
                                        <span class='<%# System.Convert.ToInt32(Eval("MinutosExcedentes")) > 0 ? "minutes-badge minutes-badge--extra" : "minutes-badge minutes-badge--none" %>'>
                                            <%#: Eval("MinutosExcedentes") %> min
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="ValorComPlano"
                                    HeaderText="Com FaleMais"
                                    DataFormatString="{0:C}" />

                                <asp:BoundField DataField="ValorSemPlano"
                                    HeaderText="Sem FaleMais"
                                    DataFormatString="{0:C}" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </section>
            </main>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
        (function () {
            var comPlanoId = '<%= lblValorComPlano.ClientID %>';
            var semPlanoId = '<%= lblValorSemPlano.ClientID %>';

            function parseReal(text) {
                var normalized = (text || "")
                    .replace(/[^0-9,.-]/g, "")
                    .replace(/\./g, "")
                    .replace(",", ".");
                return parseFloat(normalized);
            }

            function formatReal(value) {
                if (window.Intl && Intl.NumberFormat) {
                    return new Intl.NumberFormat("pt-BR", {
                        style: "currency",
                        currency: "BRL"
                    }).format(value);
                }
                return "R$ " + value.toFixed(2).replace(".", ",");
            }

            function atualizarEconomia() {
                var comPlano = document.getElementById(comPlanoId);
                var semPlano = document.getElementById(semPlanoId);
                var painel = document.getElementById("economiaResultado");
                var titulo = document.getElementById("economiaTitulo");
                var valor = document.getElementById("economiaValor");

                if (!comPlano || !semPlano || !painel || !comPlano.textContent || !semPlano.textContent) {
                    return;
                }

                var valorComPlano = parseReal(comPlano.textContent || comPlano.innerText);
                var valorSemPlano = parseReal(semPlano.textContent || semPlano.innerText);

                if (isNaN(valorComPlano) || isNaN(valorSemPlano)) {
                    return;
                }

                var diferenca = valorSemPlano - valorComPlano;
                painel.className = "savings-summary";

                if (diferenca > 0.005) {
                    painel.className += " savings-summary--positive";
                    titulo.textContent = "Economia com FaleMais";
                    valor.textContent = formatReal(diferenca);
                } else if (diferenca < -0.005) {
                    painel.className += " savings-summary--negative";
                    titulo.textContent = "Sem economia nesta simulação";
                    valor.textContent = "Com FaleMais, o custo fica " + formatReal(Math.abs(diferenca)) + " maior.";
                } else {
                    painel.className += " savings-summary--neutral";
                    titulo.textContent = "Mesmo valor com ou sem plano";
                    valor.textContent = "";
                }

                painel.hidden = false;
            }

            if (window.Sys && Sys.Application) {
                Sys.Application.add_load(atualizarEconomia);
            } else if (document.addEventListener) {
                document.addEventListener("DOMContentLoaded", atualizarEconomia);
            }
        })();
    </script>
</asp:Content>
