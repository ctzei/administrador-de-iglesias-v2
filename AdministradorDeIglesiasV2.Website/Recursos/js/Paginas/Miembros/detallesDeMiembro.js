var detallesDeMiembro = {
    timeChart: null,
    pieChart: null,
    coloresDeLaGraficas: ['#6E99B9', '#C94B25', '#676767', '#B2BC4C'],
    urlDeFotos: ResolveUrl('~/Paginas/Miembros/FotoDeMiembro.aspx')
}

function cargarFoto(idDeImg, miembroId) {
    Ext.getCmp(idDeImg).setImageUrl(detallesDeMiembro.urlDeFotos + '?miembroId=' + miembroId);
}

function crearGraficasDeAsistencias(datos, nombre) {
    if (datos) {
        datos = eval(datos);

        if (datos.length > 0) {
            var msPorSemana = 7 * 24 * 3600 * 1000,
                indicesDeAsistencias = [],
                asistenciasTotales = 0,
                faltasTotales = 0,
                cancelacionesTotales = 0,
                total = 0,
                indiceDeAsistencia = 0,
                fechaInicial = getUTCDateFromMSSQL(datos[0].FechaInicial);

            for (var i = 0; i < datos.length; i++) {
                var r = datos[i];

                indiceDeAsistencia = indiceDeAsistencia + (r.Asistencias - r.Faltas);
                indicesDeAsistencias.push(indiceDeAsistencia);

                asistenciasTotales += r.Asistencias;
                faltasTotales += r.Faltas;
                cancelacionesTotales += r.Cancelaciones;
                total++;
            }

            detallesDeMiembro.pieChart = new Highcharts.Chart({
                credits: {
                    enabled: false
                },
                colors: detallesDeMiembro.coloresDeLaGraficas,
                chart: {
                    renderTo: 'pieChartContainer',
                    defaultSeriesType: 'pie',
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false
                },
                title: {
                    text: 'Totales'
                },
                tooltip: {
                    enabled: false
                },
                plotOptions: {
                    pie: {
                        showInLegend: false,
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            formatter: function () {
                                return '<b>' + this.point.name + '</b>: ' + Math.round((this.y / total) * 100) + ' % [' + this.y + "]";
                            }
                        }
                    }
                },
                series: [{
                    name: 'Totales',
                    data: [{
                        name: 'Asistencias',
                        y: asistenciasTotales,
                        sliced: true,
                        selected: true
                    }, {
                        name: 'Faltas',
                        y: faltasTotales
                    }, {
                        name: 'Cancelaciones',
                        y: cancelacionesTotales
                    }]
                }]
            });

            detallesDeMiembro.timeChart = new Highcharts.Chart({
                credits: {
                    enabled: false
                },
                colors: detallesDeMiembro.coloresDeLaGraficas,
                chart: {
                    renderTo: 'timeChartContainer',
                    defaultSeriesType: 'area',
                    zoomType: 'x',
                    animation: true
                },
                title: {
                    text: 'Detalle de Asistencias en el Tiempo'
                },
                tooltip: {
                    shared: true,
                    formatter: function () {
                        var s = '<b>Primera Asistencia</b><br/>',
                            data = this.points[0].series.data;
                        for (var i = 1; i < data.length; i++) {
                            if (data[i].x == this.x) {
                                var tipo;
                                switch (true) {
                                    case (data[i].y == data[i - 1].y):
                                        tipo = "Cancelacion";
                                        break;
                                    case (data[i].y > data[i - 1].y):
                                        tipo = "Asistencia";
                                        break;
                                    case (data[i].y < data[i - 1].y):
                                        tipo = "Falta";
                                        break;
                                }
                                s = '<b>' + tipo + '</b><br/>';
                            }
                        }

                        s += '<small>En la semana del ' + Highcharts.dateFormat('%A, %e/%b/%Y', this.x) + '</small>';
                        return s;
                    }
                },
                xAxis: {
                    type: 'datetime',
                    maxZoom: 21 * 24 * 3600000 // 21 dias
                },
                yAxis: {
                    startOnTick: false,
                    showFirstLabel: false,
                    title: {
                        enabled: false,
                        text: ''
                    }
                },
                plotOptions: {
                    area: {
                        fillColor: {
                            linearGradient: [0, 0, 0, 300],
                            stops: [
                                      [0, Highcharts.getOptions().colors[0]],
                                      [1, 'rgba(2,0,0,0)']
                                   ]
                        },
                        lineWidth: 1,
                        marker: {
                            enabled: false,
                            states: {
                                hover: {
                                    enabled: true,
                                    radius: 5
                                }
                            }
                        },
                        shadow: false,
                        states: {
                            hover: {
                                lineWidth: 1
                            }
                        }
                    }
                },
                toolbar: {
                    itemStyle: {
                        color: '#000000',
                        fontSize: '10pt',
                        fontWeight: 'bold'
                    }
                },
                series: [{
                    name: 'Indice de Asistencias',
                    data: indicesDeAsistencias,
                    pointInterval: msPorSemana,
                    pointStart: fechaInicial,
                    stack: 'C1'
                }]
            });
        }
    }
}

Ext.onReady(function () {
    Highcharts.setOptions({
        lang: {
            months: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
            weekdays: ['Domingo', 'Lunes', 'Martes', 'Miercoles', 'Jueves', 'Viernes', 'Sabado'],
            resetZoom: "Reiniciar Zoom..."
        }
    });
});