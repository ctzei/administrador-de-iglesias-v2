<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="Calendario.aspx.cs" Inherits="AdministradorDeIglesiasV2.Website.Paginas.Alabanza.Calendario" %>

<%@ Register Assembly="ZagueEF.Core.Web.ExtNET" Namespace="ZagueEF.Core.Web.ExtNET.Controls" TagPrefix="Z" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ MasterType VirtualPath="~/MainMasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <link href="<%= ResolveUrl("~/Recursos/css/FullCalendar/fullcalendar.css")%>" rel="stylesheet" type="text/css" />
    <script src='<%= System.Web.Configuration.WebConfigurationManager.AppSettings["jQueryJsUrl"] %>' type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/Recursos/js/FullCalendar/fullcalendar.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

    <ext:Store ID="StoreEnsayos" runat="server" IgnoreExtraFields="true">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Fecha" />
                    <ext:RecordField Name="HoraInicio" />
                    <ext:RecordField Name="HoraFin" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>

    <ext:Store ID="StoreEventos" runat="server" IgnoreExtraFields="true">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Descripcion" />
                    <ext:RecordField Name="Fecha" />
                    <ext:RecordField Name="HoraInicio" />
                    <ext:RecordField Name="HoraFin" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>

    <ext:Store ID="StoreDiasNoDisponibles" runat="server" IgnoreExtraFields="true">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Razon" />
                    <ext:RecordField Name="FechaInicio" />
                    <ext:RecordField Name="FechaFin" />
                    <ext:RecordField Name="Dias" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>

    <!-- PopUps -->

    <Z:ZModalWindow
        ID="wndAgregarDiasNoDisponibles"
        runat="server"
        Title="Agregar Días No Disponibles"
        Width="400px"
        Height="150px">
        <Items>
            <Z:ZFormPanel ID="pnlAgregarDiasNoDisponibles" runat="server" BodyBorder="false">
                <Items>
                    <Z:ZTextField
                        ID="registroDiasNoDisponiblesRazon"
                        runat="server"
                        FieldLabel="Razón"
                        AllowBlank="false"
                        Flex="1"
                        AnchorHorizontal="100%"
                        MinLength="5"
                        MsgTarget="Qtip" />
                    <Z:ZCompositeField runat="server" FieldLabel="Fechas (inicio/fin)" AnchorHorizontal="100%" MsgTarget="Qtip">
                        <Items>
                            <Z:ZDateField
                                ID="registroDiasNoDisponiblesFechaInicio"
                                runat="server"
                                FieldLabel="Fecha (inicio)"
                                AnchorHorizontal="90%"
                                Flex="1"
                                AllowBlank="false"
                                MsgTarget="Qtip" />
                            <Z:ZDateField
                                ID="registroDiasNoDisponiblesFechaFin"
                                runat="server"
                                FieldLabel="Fecha (fin)"
                                AnchorHorizontal="90%"
                                Flex="1"
                                AllowBlank="false"
                                MsgTarget="Qtip" />
                        </Items>
                    </Z:ZCompositeField>
                </Items>
                <Buttons>
                    <ext:Button ID="btnAgregarDiaNoDisponible" Text="Agregar Días">
                        <Listeners>
                            <Click Handler="if (#{pnlAgregarDiasNoDisponibles}.getForm().isValid()) {Ext.net.DirectMethods.AgregarDiasNoDisponibles();}" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </Z:ZFormPanel>
        </Items>
    </Z:ZModalWindow>

    <ext:Viewport ID="Viewport" runat="server" Layout="BorderLayout">
        <Items>
            <ext:TabPanel runat="server" Region="Center">
                <Items>
                    <Z:ZPanel ID="pnlDatosGenerales" runat="server" Title="Mi Calendario" AutoScroll="true">
                        <Content>
                            <div id="calendario"></div>
                        </Content>
                    </Z:ZPanel>
                    <Z:ZGridPanel ID="gridEnsayos" runat="server" Title="Mis Ensayos" StoreID="StoreEnsayos">
                        <ColumnModel>
                            <Columns>
                                <ext:Column Header="Fecha" Width="50" DataIndex="Fecha">
                                    <Renderer Fn="Ext.Renderers.IsoDateSimple" />
                                </ext:Column>
                                <ext:Column Header="Día" Width="75" DataIndex="Fecha">
                                    <Renderer Fn="Ext.Renderers.IsoDateDayOfWeekOnly" />
                                </ext:Column>
                                <ext:Column Header="Hora (inicio)" Width="50" DataIndex="HoraInicio">
                                    <Renderer Fn="Ext.Renderers.DetallesDeEnsayo" />
                                </ext:Column>
                                <ext:Column Header="Hora (fin)" Width="50" DataIndex="HoraFin">
                                    <Renderer Fn="Ext.Renderers.DetallesDeEnsayo" />
                                </ext:Column>
                            </Columns>
                        </ColumnModel>
                    </Z:ZGridPanel>
                    <Z:ZGridPanel ID="gridEventos" runat="server" Title="Mis Eventos" StoreID="StoreEventos">
                        <ColumnModel>
                            <Columns>
                                <ext:Column Header="Descripción" Width="225" DataIndex="Descripcion">
                                    <Renderer Fn="Ext.Renderers.DetallesDeEvento" />
                                </ext:Column>
                                <ext:Column Header="Fecha" Width="50" DataIndex="Fecha">
                                    <Renderer Fn="Ext.Renderers.IsoDateSimple" />
                                </ext:Column>
                                <ext:Column Header="Día" Width="75" DataIndex="Fecha">
                                    <Renderer Fn="Ext.Renderers.IsoDateDayOfWeekOnly" />
                                </ext:Column>
                                <ext:Column Header="Hora (inicio)" Width="50" DataIndex="HoraInicio" />
                                <ext:Column Header="Hora (fin)" Width="50" DataIndex="HoraFin" />
                            </Columns>
                        </ColumnModel>
                    </Z:ZGridPanel>
                    <Z:ZGridPanel ID="gridDiasNoDisponibles" runat="server" Title="Mis Días No Disponibles" StoreID="StoreDiasNoDisponibles" AgregarColumnaParaBorrar="true" ManejadorColumnaParaBorrar="BorrarDiaNoDisponible">
                        <ColumnModel>
                            <Columns>
                                <ext:Column Header="Razón" Width="225" DataIndex="Razon" />
                                <ext:Column Header="Fecha (inicial)" Width="50" DataIndex="FechaInicio">
                                    <Renderer Fn="Ext.Renderers.IsoDateSimple" />
                                </ext:Column>
                                <ext:Column Header="Día (inicial)" Width="75" DataIndex="FechaInicio">
                                    <Renderer Fn="Ext.Renderers.IsoDateDayOfWeekOnly" />
                                </ext:Column>
                                <ext:Column Header="Fecha (final)" Width="50" DataIndex="FechaFin">
                                    <Renderer Fn="Ext.Renderers.IsoDateSimple" />
                                </ext:Column>
                                <ext:Column Header="Día (final)" Width="75" DataIndex="FechaFin">
                                    <Renderer Fn="Ext.Renderers.IsoDateDayOfWeekOnly" />
                                </ext:Column>
                                <ext:Column Header="# de Días" Width="50" DataIndex="Dias" />
                            </Columns>
                        </ColumnModel>
                        <Buttons>
                            <ext:Button ID="btnAgregarNuevoDiaNoDisponible" runat="server" Icon="Add" Text="Agregar...">
                                <Listeners>
                                    <Click Handler="#{wndAgregarDiasNoDisponibles}.show();" />
                                </Listeners>
                            </ext:Button>
                        </Buttons>
                    </Z:ZGridPanel>
                </Items>
            </ext:TabPanel>

        </Items>
    </ext:Viewport>


    <style type='text/css'>
        body
        {
            margin-top: 40px;
            text-align: center;
            font-size: 14px;
            font-family: "Lucida Grande",Helvetica,Arial,Verdana,sans-serif;
        }

        #calendario
        {
            width: 65%;
            margin: 0 auto;
            padding-top: 10px;
        }

            #calendario .evento, #calendario .ensayo
            {
                cursor: pointer;
            }
    </style>


    <script type="text/javascript">

        function BorrarDiaNoDisponible(registro) {
            Ext.Msg.confirm('', 'Desea eliminar el registro con razon: <span class="bold">' + registro.data.Razon + '</span>?', function (btn) {
                if (btn == 'yes') {
                    Ext.net.DirectMethods.BorrarDiaNoDisponible(registro.id);
                }
            }, this);
        }

        Ext.onReady(function () {
            Ext.net.DirectMethods.ObtenerEventos({
                success: function (result) {

                    $(result.eventos).each(function (index) {
                        result.eventos[index].start = result.eventos[index].date.substring(0, 11) + result.eventos[index].startTime + "Z";
                        result.eventos[index].end = result.eventos[index].date.substring(0, 11) + result.eventos[index].endTime + "Z";

                    });

                    $(result.ensayos).each(function (index) {
                        result.ensayos[index].start = result.ensayos[index].date.substring(0, 11) + result.ensayos[index].startTime + "Z";
                        result.ensayos[index].end = result.ensayos[index].date.substring(0, 11) + result.ensayos[index].endTime + "Z";
                    });

                    eventos = result.eventos;
                    ensayos = result.ensayos;
                    diasNoDisponibles = result.diasNoDisponibles;

                    var date = new Date();
                    var d = date.getDate();
                    var m = date.getMonth();
                    var y = date.getFullYear();

                    $('#calendario').fullCalendar({
                        monthNames: Date.monthNames,
                        dayNames: Date.dayNames,
                        buttonText: {
                            today: 'hoy',
                            month: 'mes',
                            week: 'semana',
                            day: 'día'
                        },
                        columnFormat: {
                            month: 'dddd',
                            week: 'dddd d/M',
                            day: 'dddd d/M'
                        },
                        titleFormat: {
                            month: 'MMMM yyyy',
                            week: "MMMM d[ yyyy]{ '&#8212;'[ MMMM] d yyyy}",
                            day: 'dddd, MMMM d, yyyy'
                        },
                        timeFormat: {
                            agenda: 'h:mm{ - h:mm}',
                            '': 'h(:mm)t{ - h(:mm)t}'
                        },
                        allDaySlot: true,
                        allDayText: "día completo",
                        header: {
                            left: 'prev,next today',
                            center: 'title',
                            right: 'month,agendaWeek,agendaDay'
                        },
                        editable: false,
                        eventClick: function (calEvent, jsEvent, view) {
                            if (calEvent.source.className.indexOf("evento") != -1) {
                                ADMI.VerDetallesDeEvento(calEvent.id);
                            }
                            else if (calEvent.source.className.indexOf("ensayo") != -1) {
                                ADMI.VerDetallesDeEnsayo(calEvent.id);
                            }
                        },
                        eventSources: [
                            {
                                events: eventos,
                                className: "evento"
                            },
                            {
                                events: ensayos,
                                color: 'green',
                                textColor: 'white',
                                className: "ensayo"
                            },
                            {
                                events: diasNoDisponibles,
                                color: 'red',
                                textColor: 'white',
                                className: "diasNoDisponibles"
                            }
                        ]
                    });
                }

            });

        });

    </script>

</asp:Content>
