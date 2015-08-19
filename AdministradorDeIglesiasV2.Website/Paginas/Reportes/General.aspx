<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPageBootstrap.Master" AutoEventWireup="true" CodeBehind="General.aspx.cs" Inherits="AdministradorDeIglesiasV2.Website.Paginas.Reportes.General" %>

<%@ Register Assembly="ZagueEF.Core.Web.ExtNET" Namespace="ZagueEF.Core.Web.ExtNET.Controls" TagPrefix="Z" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

    <ext:Store ID="StoreCelulas" runat="server" IgnoreExtraFields="true">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" Mapping="CelulaId" />
                    <ext:RecordField Name="Descripcion" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>

    <ext:FormPanel ID="pnlFiltros" runat="server" AnchorHorizontal="right" Height="75px" Layout="Form" ButtonAlign="Center" PaddingSummary="5px 5px 0" BodyBorder="false" LabelWidth="200">
        <Defaults>
            <ext:Parameter Name="AllowBlank" Value="false" Mode="Raw" />
        </Defaults>
        <Items>
            <Z:ZComboBox
                FieldLabel="Celula"
                ID="cboCelula"
                runat="server"
                StoreID="StoreCelulas"
                Resizable="true"
                AllowBlank="false">
                <Listeners>
                    <Change Handler="limpiarReporte();" />
                </Listeners>
            </Z:ZComboBox>
        </Items>
        <Buttons>
            <ext:Button ID="cmdMostrarReporte" runat="server" Text="Mostrar Reporte">
                <Listeners>
                    <Click Handler="mostrarReporte();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:FormPanel>

    <!-- Tabs -->
    <div>

        <!-- Nav tabs -->
        <ul class="nav nav-tabs" role="tablist">
            <li role="presentation" class="active"><a href="#reporteGanar" role="tab" data-toggle="tab">Ganar</a></li>
            <li role="presentation"><a href="#reporteConsolidar" role="tab" data-toggle="tab">Consolidar</a></li>
            <li role="presentation"><a href="#reporteDiscipular" role="tab" data-toggle="tab">Discipular</a></li>
            <li role="presentation"><a href="#reporteEnviar" role="tab" data-toggle="tab">Enviar</a></li>
        </ul>

        <!-- Tab panes -->
        <div class="tab-content">
            <div role="tabpanel" class="tab-pane active" id="reporteGanar"></div>
            <div role="tabpanel" class="tab-pane" id="reporteConsolidar"><div class="alert alert-warning">Proximamente...</div></div>
            <div role="tabpanel" class="tab-pane" id="reporteDiscipular"><div class="alert alert-warning">Proximamente...</div></div>
            <div role="tabpanel" class="tab-pane" id="reporteEnviar"><div class="alert alert-warning">Proximamente...</div></div>
        </div>

    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphFooter" runat="server">
    <script type="text/javascript">

        Highcharts.setOptions({
            lang: {
                resetZoom: "Reiniciar zoom"
            }
        });

        function obtenerFormDeFiltros() {
            return Ext.getCmp('cphMain_pnlFiltros');
        }

        function limpiarReporte() {
            $("#reporteGanar").html("<div id='chartReporteGeneral'></div>");
        }

        function mostrarReporte() {
            if (obtenerFormDeFiltros().getForm().isValid()) {
                limpiarReporte();

                Ext.net.DirectMethods.ObtenerReporteGeneral($("#cphMain_cboCelula").prev().val(), {
                    success: function (rtn) {
                        var rows = eval(rtn);

                        var categories = [];
                        var series = [];

                        var $table = $("<table class='table table-striped table-hover' style='width:98%;'><thead></thead><tbody></tbody></table>");

                        var $trHead = $("<tr></tr>");
                        $trHead.append("<th># Semana</th>");
                        $.each(rows, function (index) {
                            var val = (index + 1);
                            $trHead.append("<th>" + val + "</th>");
                            categories.push(val);
                        });
                        $table.find("thead").append($trHead);

                        var $trActivas = $("<tr></tr>");
                        $trActivas.append("<td>Activas</td>");
                        series.push({ name: "Activas", data: [] });
                        $.each(rows, function (index) {
                            var val = rows[index].activas;
                            $trActivas.append("<td>" + val + "</td>");
                            series[0].data.push(val);
                        });
                        $table.find("tbody").append($trActivas);

                        var $trRealizadas = $("<tr></tr>");
                        $trRealizadas.append("<td>Realizadas</td>");
                        series.push({ name: "Realizadas", data: [] });
                        $.each(rows, function (index) {
                            var val = rows[index].realizadas;
                            $trRealizadas.append("<td>" + val + "</td>");
                            series[1].data.push(val);
                        });
                        $table.find("tbody").append($trRealizadas);

                        var $trNoRealizadas = $("<tr></tr>");
                        $trNoRealizadas.append("<td>No Realizadas</td>");
                        series.push({ name: "No Realizadas", data: [] });
                        $.each(rows, function (index) {
                            var val = rows[index].activas - rows[index].realizadas;
                            $trNoRealizadas.append("<td>" + val + "</td>");
                            series[2].data.push(val);
                        });
                        $table.find("tbody").append($trNoRealizadas);

                        var $trInvitados = $("<tr></tr>");
                        $trInvitados.append("<td>Invitados</td>");
                        series.push({ name: "Invitados", data: [] });
                        $.each(rows, function (index) {
                            var val = rows[index].invitados;
                            $trInvitados.append("<td>" + val + "</td>");
                            series[3].data.push(val);
                        });
                        $table.find("tbody").append($trInvitados);

                        var $trAsistencia = $("<tr></tr>");
                        $trAsistencia.append("<td>Asistencia</td>");
                        series.push({ name: "Asistencia", data: [] });
                        $.each(rows, function (index) {
                            var val = rows[index].asistencia;
                            $trAsistencia.append("<td>" + val + "</td>");
                            series[4].data.push(val);
                        });
                        $table.find("tbody").append($trAsistencia);

                        $("#reporteGanar").append($table.wrap("<div class='table-responsive'></div>"));

                        $("#chartReporteGeneral").highcharts({
                            chart: {
                                zoomType: "x"
                            },
                            credits: {
                                enabled: false
                            },
                            title: {
                                text: null
                            },
                            xAxis: {
                                title: {
                                    text: 'Semana'
                                },
                                categories: categories
                            },
                            yAxis: {
                                title: {
                                    text: null
                                }
                            },
                            series: series
                        });

                    }
                });
            }
        }

    </script>
</asp:Content>
