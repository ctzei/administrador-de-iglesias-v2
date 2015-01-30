<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="Boletas.aspx.cs" Inherits="AdministradorDeIglesiasV2.Website.Paginas.Reportes.Consolidacion.Boletas" %>

<%@ Register Assembly="ZagueEF.Core.Web.ExtNET" Namespace="ZagueEF.Core.Web.ExtNET.Controls" TagPrefix="Z" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ MasterType VirtualPath="~/MainMasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <link rel="stylesheet" href='<%= System.Web.Configuration.WebConfigurationManager.AppSettings["jQueryUiCssUrl"] %>' type="text/css">
    <link rel="stylesheet" href="//cdnjs.cloudflare.com/ajax/libs/pivottable/1.3.0/pivot.min.css" type="text/css">
    <link rel="stylesheet" href="//cdnjs.cloudflare.com/ajax/libs/datatables/1.10.4/css/jquery.dataTables.min.css" type="text/css">

    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <script src='<%= System.Web.Configuration.WebConfigurationManager.AppSettings["jQueryJsUrl"] %>' type="text/javascript"></script>
    <script src='<%= System.Web.Configuration.WebConfigurationManager.AppSettings["jQueryUiJsUrl"] %>' type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/Recursos/js/pivot.js")%>" type="text/javascript"></script>
    <script src="//cdnjs.cloudflare.com/ajax/libs/pivottable/1.3.0/gchart_renderers.min.js" type="text/javascript"></script>
    <script src="//cdnjs.cloudflare.com/ajax/libs/datatables/1.10.4/js/jquery.dataTables.min.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

    <ext:Viewport ID="ViewportAsistencias" runat="server" Layout="Anchor">
        <Items>
            <ext:FormPanel ID="pnlFiltros" runat="server" AnchorHorizontal="right" Height="75px" Layout="Form" ButtonAlign="Center" PaddingSummary="5px 5px 0" BodyBorder="false" LabelWidth="200">
                <Defaults>
                    <ext:Parameter Name="AllowBlank" Value="false" Mode="Raw" />
                </Defaults>
                <Items>
                    <Z:ZCompositeField ID="CompositeField1" runat="server" FieldLabel="Fechas del Reporte (Inicio/Fin)">
                        <Items>
                            <Z:ZDateField
                                FieldLabel="Inicio "
                                ID="dtpFechaInicial"
                                runat="server"
                                AllowBlank="false"
                                Flex="1">
                                <Listeners>
                                    <Change Handler="limpiarReporte();" />
                                </Listeners>
                            </Z:ZDateField>
                            <Z:ZDateField
                                FieldLabel="Fin"
                                ID="dtpFechaFinal"
                                runat="server"
                                AllowBlank="false"
                                Flex="1">
                                <Listeners>
                                    <Change Handler="limpiarReporte();" />
                                </Listeners>
                            </Z:ZDateField>
                        </Items>
                    </Z:ZCompositeField>
                </Items>
                <Buttons>
                    <ext:Button ID="cmdMostrarReporte" runat="server" Text="Mostrar Reporte">
                        <Listeners>
                            <Click Handler="mostrarReporte();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:FormPanel>

            <ext:TabStrip ID="tabsReporte" runat="server" AutoWidth="true" Hidden="true">
                <Items>
                    <ext:TabStripItem runat="server" Title="Tabla Pivote" ActionItemID="tablaPivote" />
                    <ext:TabStripItem runat="server" Title="Table Plana" ActionItemID="tablaPlana" />
                </Items>
            </ext:TabStrip>

        </Items>
    </ext:Viewport>

    <div id="tablaPivote" class="tab-content"></div>
    <div id="tablaPlana" class="tab-content"></div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphFooter" runat="server">

    <style type="text/css">
        body {
            overflow: scroll !important;
        }

        .tab-content {
            padding: 20px;
            min-height: 400px;
            display: none;
        }

        #tablaPivote table {
            margin: 0 auto;
        }

        #tablaPivote ul {
            margin-top: 5px;
        }

        #reportesPredefinidos > li {
            background-color: #eee;
            border: 1px solid #ddd;
            display: inline-block;
            margin-bottom: 3px;
            margin-right: 3px;
            padding: 15px;
        }
    </style>

    <script type="text/javascript">

        google.load("visualization", "1", { packages: ["corechart", "charteditor"] });

        function limpiarReporte() {
            $(".tab-content").html("");
            $("#tablaPivote").append("<div class='display' />");
            $("#tablaPlana").append("<table class='display' />");
        }

        function obtenerFormDeFiltros() {
            return Ext.getCmp('cphMain_pnlFiltros');
        }

        function obtenerFecha(id) {
            var fecha = Ext.getCmp(id).getValue();
            return fecha.getFullYear() + "-" + (fecha.getMonth() + 1) + "-" + fecha.getDate() + "T00:00:00.000";
        }

        function mostrarReporte() {
            if (obtenerFormDeFiltros().getForm().isValid()) {
                limpiarReporte();

                var fechaInicial = obtenerFecha('cphMain_dtpFechaInicial');
                var fechaFinal = obtenerFecha('cphMain_dtpFechaFinal');

                //Mostramos los elementos...
                Ext.getCmp('cphMain_tabsReporte').show();
                Ext.get('tablaPivote').show();
                Ext.get('tablaPlana').show();

                Ext.net.DirectMethods.ObtenerBoletas(fechaInicial, fechaFinal, {
                    success: function (rtn) {
                        var rows = eval(rtn);

                        if (rows && rows.length > 0) {

                            var renderers = $.extend($.pivotUtilities.renderers, $.pivotUtilities.gchart_renderers);
                            var tablaPivote = $("#tablaPivote > .display");

                            var generarReporteDeEstatusPorMes = function () {
                                tablaPivote.pivotUI(rows, {
                                    renderers: renderers,
                                    rows: ["Estatus", "SubEstatus"],
                                    cols: ["Año", "Mes"],
                                    rendererName: "Heatmap"
                                }, true);
                            };

                            var generarReporteDeEstatusPorMesEnPorcientos = function () {
                                tablaPivote.pivotUI(rows, {
                                    renderers: renderers,
                                    rows: ["Estatus", "SubEstatus"],
                                    cols: ["Año", "Mes"],
                                    rendererName: "Heatmap",
                                    aggregatorName: "Count as Fraction of Columns"
                                }, true);
                            };

                            var generarReporteAcumuladoAnualDeEstatus = function () {
                                tablaPivote.pivotUI(rows, {
                                    renderers: renderers,
                                    rows: ["Estatus"],
                                    aggregator: "Count as Fraction of Total",
                                    rendererName: "Heatmap"
                                }, true);
                            };

                            generarReporteDeEstatusPorRed = function () {
                                tablaPivote.pivotUI(rows, {
                                    renderers: renderers,
                                    rows: ["Estatus", "SubEstatus"],
                                    cols: ["Red"],
                                    rendererName: "Heatmap"
                                }, true);
                            };

                            generarReporteDeEstatusPorConsolidador = function () {
                                tablaPivote.pivotUI(rows, {
                                    renderers: renderers,
                                    rows: ["Miembro"],
                                    cols: ["Estatus", "SubEstatus"],
                                    rendererName: "Heatmap"
                                }, true);
                            };

                            // Generamos el reporte predeterminado
                            generarReporteDeEstatusPorMes();

                            // Agregamos los reportes predefinidos
                            var reportes = $("<div><h1>Reportes Predefinidos: </h1><ul id='reportesPredefinidos' /></div>");
                            var reportesUl = reportes.find("ul");

                            var reporteDeEstatusPorMes = $("<li><a href='javascript:void(0);'>Estatus por Mes</a></li>");
                            reporteDeEstatusPorMes.on("click", generarReporteDeEstatusPorMes);
                            reportesUl.append(reporteDeEstatusPorMes);

                            var reporteDeEstatusPorMesEnPorcientos = $("<li><a href='javascript:void(0);'>Estatus por Mes (%)</a></li>");
                            reporteDeEstatusPorMesEnPorcientos.on("click", generarReporteDeEstatusPorMesEnPorcientos);
                            reportesUl.append(reporteDeEstatusPorMesEnPorcientos);

                            var reporteAcumuladoAnualDeEstatus = $("<li><a href='javascript:void(0);'>Acumulado Anual de Estatus</a></li>");
                            reporteAcumuladoAnualDeEstatus.on("click", generarReporteAcumuladoAnualDeEstatus);
                            reportesUl.append(reporteAcumuladoAnualDeEstatus);

                            var reporteDeEstatusPorRed = $("<li><a href='javascript:void(0);'>Estatus por Red</a></li>");
                            reporteDeEstatusPorRed.on("click", generarReporteDeEstatusPorRed);
                            reportesUl.append(reporteDeEstatusPorRed);

                            var reporteDeEstatusPorConsolidador = $("<li><a href='javascript:void(0);'>Estatus por Consolidador</a></li>");
                            reporteDeEstatusPorConsolidador.on("click", generarReporteDeEstatusPorConsolidador);
                            reportesUl.append(reporteDeEstatusPorConsolidador);

                            tablaPivote.parent().prepend(reportes);

                            // Generamos la files de la tabla HTML
                            var table = $('#tablaPlana > .display');

                            // Agregmos el thead y tbody
                            table.append($("<thead />")).append($("<tbody />"));
                            var thead = table.find("thead");
                            var tbody = table.find("tbody");

                            // Agregamos las columnas
                            var tr = "<tr>";
                            for (col in rows[0]) {
                                tr += "<td>" + col + "</td>";
                            }
                            tr += "</tr>";
                            thead.append($(tr));

                            // Agregamos las filas
                            for (var i = 0; i < rows.length; i++) {
                                row = rows[i];
                                var tr = "<tr>";
                                for (col in row) {
                                    tr += "<td>" + row[col] + "</td>";
                                }
                                tr += "</tr>";
                                tbody.append($(tr));
                            }

                            // Convertimos la tabla HTML a dataTable
                            table.dataTable({
                                destroy: true,
                                iDisplayLength: 25,
                                aLengthMenu: [
                                  [25, 50, 100, 200, -1],
                                  [25, 50, 100, 200, "All"]
                                ]
                            });

                        }
                    }
                });

            }
        }

    </script>
</asp:Content>
