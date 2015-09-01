<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPageBootstrap.Master" AutoEventWireup="true" CodeBehind="General.aspx.cs" Inherits="AdministradorDeIglesiasV2.Website.Paginas.Reportes.General" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

    <div class="form-horizontal validate">
        <div class="form-group">
            <label class="col-sm-2 control-label">Célula:</label>
            <div class="col-sm-10">
                <asp:DropDownList
                    runat="server"
                    ID="cboCelula"
                    CssClass="form-control required"
                    DataValueField="CelulaId"
                    DataTextField="Descripcion">
                </asp:DropDownList>
            </div>
        </div>

        <div class="form-group">
            <div class="text-center">
                <button id="mostrarReporte" type="button" class="btn btn-primary">Mostrar Reporte</button>
            </div>
        </div>
    </div>

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
        <div class="tab-content" style="padding: 10px 20px;">
            <div role="tabpanel" class="tab-pane active" id="reporteGanar"></div>
            <div role="tabpanel" class="tab-pane" id="reporteConsolidar">
                <div class="alert alert-warning">Proximamente...</div>
            </div>
            <div role="tabpanel" class="tab-pane" id="reporteDiscipular">
                <div class="alert alert-warning">Proximamente...</div>
            </div>
            <div role="tabpanel" class="tab-pane" id="reporteEnviar">
                <div class="alert alert-warning">Proximamente...</div>
            </div>
        </div>

    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphFooter" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {

            var $celula = $("#cphMain_cboCelula");
            var $mostrarReporte = $("#mostrarReporte");

            var limpiarReporte = function () {
                $("#reporteGanar").html("");
            };

            var mostrarReporte = function () {
                $("body").mask();
                limpiarReporte();

                $.ajax({
                    type: "POST",
                    url: "General.aspx/ObtenerReporteGeneral",
                    data: '{celulaId: ' + $celula.val() + ' }',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success:  function (rtn) {
                        var datos = eval(rtn.d);

                        if (!datos.error) {

                            $.each(datos, function (index, datosPorCelula) {

                                var rows = datosPorCelula.resultadoAnual;

                                var $reportePorCelula = $('' +
                                '<div class="well">' +
                                '    <h4>' + datosPorCelula.nombre + '</h4>' +
                                '' +
                                '    <ul class="nav nav-pills" role="tablist">' +
                                '        <li role="presentation" class="active"><a href="#reporteGanarGrafica_' + index + '" role="tab" data-toggle="tab">Gráfica</a></li>' +
                                '        <li role="presentation"><a href="#reporteGanarTabla_' + index + '" role="tab" data-toggle="tab">Tabla</a></li>' +
                                '    </ul>' +
                                '' +
                                '    <div class="tab-content">' +
                                '        <div role="tabpanel" class="tab-pane active" id="reporteGanarGrafica_' + index + '"></div>' +
                                '        <div role="tabpanel" class="tab-pane table-responsive" id="reporteGanarTabla_' + index + '"></div>' +
                                '    </div>' +
                                '' +
                                '</div>');
                                $reportePorCelula.appendTo($("#reporteGanar"));

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

                                $table.wrap("<div class='table-responsive'></div>").appendTo("#reporteGanarTabla_" + index);

                                $("<div class='chart'></div>").appendTo("#reporteGanarGrafica_" + index).highcharts({
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

                            });
                        } else {
                            bootbox.alert(datos.error);
                        };
                    },
                    error: function (jqXHR, textStatus) {
                        bootbox.alert(textStatus);
                    },
                    complete: function () {
                        $("body").unmask();
                    }
                });

            };

            $celula.on("change", limpiarReporte);
            $mostrarReporte.on("click", mostrarReporte);

        });

    </script>
</asp:Content>
