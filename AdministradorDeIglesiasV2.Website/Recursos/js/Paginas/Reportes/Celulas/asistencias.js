var chartAsistenciaPorFechasGraficaPromedios,
    chartAsistenciaPorFechasGraficaComportamiento,
    chartMiembrosVsServidores,
    chartMiembrosQueAsistenIglesia,
    coloresDeLaGraficas = ['#C94B25', '#6E99B9', '#676767', '#B2BC4C'];

function limpiarReporte() {
    window.reporteDeAsistencia = [];

    if ((typeof (chartAsistenciaPorFechasGraficaPromedios) != 'undefined') && (chartAsistenciaPorFechasGraficaPromedios.destroy)) { chartAsistenciaPorFechasGraficaPromedios.destroy(); }
    if ((typeof (chartAsistenciaPorFechasGraficaComportamiento) != 'undefined') && (chartAsistenciaPorFechasGraficaComportamiento.destroy)) { chartAsistenciaPorFechasGraficaComportamiento.destroy(); }
    if ((typeof (chartMiembrosVsServidores) != 'undefined') && (chartMiembrosVsServidores.destroy)) { chartMiembrosVsServidores.destroy(); }
    if ((typeof (chartMiembrosQueAsistenIglesia) != 'undefined') && (chartMiembrosQueAsistenIglesia.destroy)) { chartMiembrosQueAsistenIglesia.destroy(); }

    Ext.fly('asistenciaPorFechas').dom.innerHTML = '';
}

function obtenerFormDeFiltros(){
    return Ext.getCmp('cphMain_pnlFiltros');
}

function obtenerComboDeCelulas() {
    if ((typeof (window.cboCelula) == 'undefined') || (window.cboCelula == null)){
        window.cboCelula = Ext.getCmp('cphMain_cboCelula');
    }
    return window.cboCelula;
}

function obtenerFecha(id){
    var fecha = Ext.getCmp(id).getValue();
    return fecha.getFullYear() + "-" + (fecha.getMonth() + 1 )+ "-" + fecha.getDate() + "T00:00:00.000";
}

function mostrarReporte() {
    if (obtenerFormDeFiltros().getForm().isValid()) {
        limpiarReporte();

        var celulaId = obtenerComboDeCelulas().getValue();
        var descripcion = obtenerComboDeCelulas().getText();
        var fechaInicial = obtenerFecha('cphMain_dtpFechaInicial');
        var fechaFinal = obtenerFecha('cphMain_dtpFechaFinal');

        //Mostramos los elementos...
        Ext.getCmp('cphMain_tabsReporte').show();
        Ext.get('informacionGeneral').show();
        Ext.get('asistenciaPorFechas').show();
        Ext.get('asistenciaPorFechasGraficaPromedios').show();

        Ext.net.DirectMethods.ObtenerInformacionGeneralPorRed({
            success: function (rtn) {
                var datos = eval(rtn);
                mostrarInformacionGeneral(datos, celulaId, descripcion);
            }
        });
		
		Ext.net.DirectMethods.ObtenerReporteDeAsistencias(celulaId, fechaInicial, fechaFinal, {
            success: function (rtn) {
                window.reporteDeAsistencia = eval(rtn);
                mostrarAsistenciaPorFechas(window.reporteDeAsistencia, celulaId, descripcion);
                mostrarAsistenciaPorFechasGraficaDePromedios(window.reporteDeAsistencia, celulaId, descripcion);
            }
        });
    }
}

function mostrarInformacionGeneral(datos, celulaId, descripcion) {
    if (datos) {
        Ext.fly('cantidadDeCelulas').dom.innerHTML = datos.CantidadDeCelulas;
        Ext.fly('cantidadDeMiembros').dom.innerHTML = datos.CantidadDeMiembros;
        Ext.fly('cantidadDeMiembrosHombres').dom.innerHTML = datos.CantidadDeMiembrosHombres;
        Ext.fly('cantidadDeMiembrosMujeres').dom.innerHTML = datos.CantidadDeMiembrosMujeres;
        Ext.fly('cantidadDeLideresDeCelula').dom.innerHTML = datos.CantidadDeLideresDeCelula;
        Ext.fly('cantidadDeEstacas').dom.innerHTML = datos.CantidadDeEstacas;
        Ext.fly('cantidadDeFolis').dom.innerHTML = datos.CantidadDeFolis;
        Ext.fly('cantidadDeMiembrosQueAsistenIglesia').dom.innerHTML = datos.CantidadDeMiembrosQueAsistenIglesia;
        Ext.fly('cantidadDeMiembrosQueAsistenIglesiaHombres').dom.innerHTML = datos.CantidadDeMiembrosQueAsistenIglesiaHombres;
        Ext.fly('cantidadDeMiembrosQueAsistenIglesiaMujeres').dom.innerHTML = datos.CantidadDeMiembrosQueAsistenIglesiaMujeres;

        //Creamos la grafica que compara los miembros contra los servidores (lideres y estacas)
        chartMiembrosVsServidores = new Highcharts.Chart({
            credits: {
                    enabled: false
                },
                colors: coloresDeLaGraficas,
                chart: {
                    renderTo: 'chartMiembrosVsServidores',
                    defaultSeriesType: 'pie',
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false
                },
                title: {
                    text: 'Miembros vs. Servidores'
                },
                tooltip: {
                    enabled: false
                },
                plotOptions: {
                    pie: {
                        shadow: false,
                        animation: true,
                        showInLegend: false,
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            formatter: function () {
                                return '<b>' + this.point.name + '</b>: ' + this.y  + ' [' + Math.round((this.y / datos.CantidadDeMiembros) * 100) + "%]";
                            }
                        }
                    }
                },
                series: [{
                    name: 'Totales',
                    data: [{
                        name: 'Miembros',
                        y: datos.CantidadDeMiembros - datos.CantidadDeLideresDeCelula - datos.CantidadDeEstacas
                    }, {
                        name: 'Lideres de Celula',
                        y: datos.CantidadDeLideresDeCelula,
                        sliced: true,
                        selected: true
                    }, {
                        name: 'Estacas',
                        y: datos.CantidadDeEstacas
                    }]
                }]
            });

        //Creamos la grafica que compara los miembros que van a la iglesia contra los que no
        chartMiembrosQueAsistenIglesia = new Highcharts.Chart({
            credits: {
                    enabled: false
                },
                colors: coloresDeLaGraficas,
                chart: {
                    renderTo: 'chartMiembrosQueAsistenIglesia',
                    defaultSeriesType: 'pie',
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false
                },
                title: {
                    text: 'Miembros que Asisten a la Iglesia'
                },
                tooltip: {
                    enabled: false
                },
                plotOptions: {
                    pie: {
                        shadow: false,
                        animation: true,
                        showInLegend: false,
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            formatter: function () {
                                return '<b>' + this.point.name + '</b>: ' + this.y + ' [' + Math.round((this.y / datos.CantidadDeMiembros) * 100) + "%]";
                            }
                        }
                    }
                },
                series: [{
                    name: 'Totales',
                    data: [{
                        name: 'Asisten a la Iglesia',
                        y: datos.CantidadDeMiembrosQueAsistenIglesia,
                        sliced: true,
                        selected: true
                    }, {
                        name: 'No Asisten',
                        y: datos.CantidadDeMiembros - datos.CantidadDeMiembrosQueAsistenIglesia
                    }]
                }]
            });

    }
}

function mostrarAsistenciaPorFechas(datos, celulaId, descripcion){
    var table = document.createElement('table');
    var thead = document.createElement('thead');
    var tbody = document.createElement('tbody');

    table.width = "100%";
    table.className = "gridDatos center";
    table.border = "1";

    thead.className = "bold";

    var row, cell, a;
    row = document.createElement('tr');

    cell = document.createElement('td');
	cell.appendChild(document.createTextNode('Id'))
    cell.rowSpan = 2;
	row.appendChild(cell);

    cell = document.createElement('td');
	cell.appendChild(document.createTextNode('Descripcion'))
    cell.rowSpan = 2;
	row.appendChild(cell);

    //Creamos las columnas de las semanas
    for (var i=0; i<datos.Fechas.length; i++){
        var fecha = datos.Fechas[i];
		cell=document.createElement('td');
		cell.appendChild(document.createTextNode('Semana ' + (i + 1) + ' (' + getShortDateFromMSSQL(fecha.FechaInicial) + ')'));
        cell.colSpan = 2;
		row.appendChild(cell);
	}
	thead.appendChild(row);
	
    //Creamos las columnas de Asistencia/Gente por cada semana
	row = document.createElement('tr');
	for (var i=0; i<datos.Fechas.length; i++){
		cell=document.createElement('td');
		cell.appendChild(document.createTextNode('Asistencia'));
		row.appendChild(cell);
		
		cell=document.createElement('td');
		cell.appendChild(document.createTextNode('Gente'));
		row.appendChild(cell);
	}
	
    //Creamos la columna para "Ver grafica"
    cell=document.createElement('td');
	cell.appendChild(document.createTextNode('Grafica'));
	row.appendChild(cell);
    thead.appendChild(row);

    //Creamos las filas de cada celula
    for (var i=0; i<datos.AsistenciasPorSemana.length; i++){
        var asistenciaPorSemana = datos.AsistenciasPorSemana[i];

        //No mostramos la RED si no son mas de 2 celulas...
        if ((asistenciaPorSemana.CelulaId > 0) || (datos.AsistenciasPorSemana.length > 2)){
            row = document.createElement('tr');

		    cell=document.createElement('td');
		    cell.appendChild(document.createTextNode(asistenciaPorSemana.CelulaId));
		    row.appendChild(cell);
		
            if (asistenciaPorSemana.CelulaId > 0 && asistenciaPorSemana.CelulaId != celulaId){
                a = document.createElement('a');
                a.appendChild(document.createTextNode(asistenciaPorSemana.Descripcion));
                a.href = "#";
                Ext.get(a).on('click', function(asistenciaPorSemana) {
                    return function() {
                        if (confirm('Ver detalle?')){
                            obtenerComboDeCelulas().setValue(asistenciaPorSemana.CelulaId); 
                            mostrarReporte(); 
                        }
                        return false;
                    };
                 }(asistenciaPorSemana));

		        cell=document.createElement('td');
		        cell.appendChild(a);
		        row.appendChild(cell);
            }
            else{
                cell=document.createElement('td');
                cell.appendChild(document.createTextNode(asistenciaPorSemana.Descripcion));
		        row.appendChild(cell); 
            }

            //Creamos las columnas con las asistencias/gente de cada semana, por cada celula
            for (var j=0; j<asistenciaPorSemana.Asistencias.length; j++){
                var asistencia = asistenciaPorSemana.Asistencias[j];

                var span = document.createElement("span"); 
                span.appendChild(document.createTextNode(asistencia.Asistencias));
                if (asistencia.Asistencias / asistencia.Gente < 0.3){
                    span.className = 'alert';
                }

                cell=document.createElement('td');
		        cell.appendChild(span);
		        row.appendChild(cell);  

                cell=document.createElement('td');
		        cell.appendChild(document.createTextNode(asistencia.Gente));
		        row.appendChild(cell);  
            }

            
            a = document.createElement('a');
            a.appendChild(document.createTextNode('ver'));
            a.href = "#";
            Ext.get(a).on('click', function(asistenciaPorSemana) {
               return function() {mostrarAsistenciaPorFechasGraficaDeComportamiento(datos, asistenciaPorSemana.CelulaId, asistenciaPorSemana.Descripcion); return false;};
             }(asistenciaPorSemana));

	        cell=document.createElement('td');
            cell.appendChild(a);
	        row.appendChild(cell);

            tbody.appendChild(row);
        }
	}
	
    table.appendChild(thead);
    table.appendChild(tbody);

    document.getElementById('asistenciaPorFechas').appendChild(table);
}

function mostrarAsistenciaPorFechasGraficaDePromedios(datos, celulaId, descripcion) {
    if (datos){
        var celulas = [],
            labels = [],
            asistencias = [],
            faltas = [],
            cancelaciones = [],
            miembros = [];

        for (var i = 0; i < datos.AsistenciasTotales.length; i++) {
            var asistenciaTotalPorCelula = datos.AsistenciasTotales[i];
            if (!(asistenciaTotalPorCelula.CelulaId <= 0 && datos.AsistenciasTotales.length <= 2)) { //No mostramos la "RED" si es una celula unicamente...
                for (var j = 0; j < asistenciaTotalPorCelula.Asistencias.length; j++) {
                    var asistenciaPorFecha = asistenciaTotalPorCelula.Asistencias[j];
                    labels.push(asistenciaTotalPorCelula.Descripcion);
                    asistencias.push(asistenciaPorFecha.Asistencias);
                    faltas.push(asistenciaPorFecha.Faltas);
                    cancelaciones.push(asistenciaPorFecha.Cancelaciones);
                    miembros.push(asistenciaPorFecha.Gente);
                }
            }
        }

        chartAsistenciaPorFechasGraficaPromedios = new Highcharts.Chart({
            credits: {
                enabled: false
            },
            colors: coloresDeLaGraficas,
            chart: {
                renderTo: 'chartAsistenciaPorFechasGraficaPromedios',
                defaultSeriesType: 'column',
                shadow: false,
                animation: false
            },
            title: {
                text: 'Promedios para: ' + descripcion
            },
            xAxis: {
                categories: labels
            },
            yAxis: {
                title: {
                    text: "Asistencias/Faltas"
                }
            },
            plotOptions: {
                column: {
                    pointPadding: 0,
                    shadow: false,
                    animation: false,
                    cursor: 'pointer'
                }
            },
            series: [{
                name: 'Faltas (Promedio por Semana)',
                data: faltas
            }, {
                name: 'Asistencias (Promedio por Semana)',
                data: asistencias
            }, {
                name: 'Cancelaciones (Promedio por Semana)',
                data: cancelaciones
            }, {
                name: 'Miembros (Promedio Total)',
                data: miembros
            }]
        });
    }
}

function mostrarAsistenciaPorFechasGraficaDeComportamiento(datos, celulaId, descripcion) {
    if (datos) {
        Ext.getCmp('cphMain_wndGraficaDeComportamiento').show();
        var asistencias = [],
            faltas = [],
            cancelaciones = [];

        for (var i = 0; i < datos.AsistenciasPorSemana.length; i++) {
            var asistenciaPorCelula = datos.AsistenciasPorSemana[i];
            if (asistenciaPorCelula.CelulaId == celulaId) {
                for (var j = 0; j < asistenciaPorCelula.Asistencias.length; j++) {
                    var asistenciaPorFecha = asistenciaPorCelula.Asistencias[j];
                    var fecha = getUTCDateFromMSSQL(datos.Fechas[j].FechaInicial);
                    asistencias.push([fecha, asistenciaPorFecha.Asistencias]);
                    faltas.push([fecha, asistenciaPorFecha.Faltas]);
                    cancelaciones.push([fecha, asistenciaPorFecha.Cancelaciones]);
                }
                break;
            }
        }

        detailChart = new Highcharts.Chart({
            credits: {
                enabled: false
            },
            colors: coloresDeLaGraficas,
            chart: {
                renderTo: "chartAsistenciaPorFechasGraficaComportamiento",
                defaultSeriesType: 'area',
                zoomType: 'null',
                animation: false
            },
            title: {
                text: 'Comportamiento Semanal para: ' + descripcion
            },
            xAxis: {
                type: 'datetime',
                maxZoom: 21 * 24 * 3600000 // 21 dias
            },
            yAxis: {
                title: {
                    text: "Asistencias/Faltas"
                }
            },
            plotOptions: {
                area: {
                    fillOpacity: 0.5,
                    lineWidth: 3,
                    marker: {
                        enabled: false,
                        states: {
                            hover: {
                                enabled: true,
                                radius: 5
                            }
                        }
                    },
                    states: {
                        hover: {
                            enabled: true,
                            lineWidth: 3
                        }
                    },
                    shadow: false,
                    animation: false
                }
            },
            toolbar: {
                itemStyle: {
                    color: '#000000',
                    fontSize: '10pt',
                    fontWeight: 'bold'
                }
            },
            tooltip: {
                shared: true,
                formatter: function () {
                    var s = "<b>Asistencias: " + this.points[1].y + "</b><br/><b>Faltas: " + this.points[0].y + "</b><br/>";

                    if (this.points[2].y > 0) {
                        s += "<b><small>Hubo al menos una Cancelación: " + this.points[2].y + "</small></b><br/>";
                    }

                    s += '<small>En la semana del ' + Highcharts.dateFormat('%A, %e/%b/%Y', this.x) + '</small>';
                    return s;
                }
            },
            series: [{
                name: 'Faltas',
                data: faltas
            }, {
                name: 'Asistencias',
                data: asistencias
            }, {
                name: 'Cancelaciones',
                data: cancelaciones
            }]
        });


    }
}

Ext.onReady(function () {
    Highcharts.setOptions({
        lang: {
            months: Date.monthNames,
            weekdays: Date.dayNames,
            resetZoom: "Reiniciar Zoom..."
        }
    });
});