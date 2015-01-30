<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="Asistencias.aspx.cs" Inherits="AdministradorDeIglesiasV2.Website.Paginas.Reportes.Celulas.Asistencias" %>
<%@ Register Assembly="ZagueEF.Core.Web.ExtNET" Namespace="ZagueEF.Core.Web.ExtNET.Controls" TagPrefix="Z" %>
<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>
<%@ MasterType VirtualPath="~/MainMasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <script src='<%= System.Web.Configuration.WebConfigurationManager.AppSettings["jQueryJsUrl"] %>' type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/Recursos/js/Highcharts/highcharts.js")%>" type="text/javascript"></script>
</asp:Content>

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

        <ext:Viewport ID="ViewportAsistencias" runat="server" Layout="Anchor">
            <Items>
                <ext:FormPanel ID="pnlFiltros" runat="server" AnchorHorizontal="right" Height="105px" Layout="Form" ButtonAlign="Center" PaddingSummary="5px 5px 0" BodyBorder="false" LabelWidth="200">
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
                                </Z:ZDateField >
                                <Z:ZDateField  
                                    FieldLabel="Fin" 
                                    ID="dtpFechaFinal" 
                                    runat="server"
                                    AllowBlank="false"
                                    Flex="1">
                                    <Listeners>
                                        <Change Handler="limpiarReporte();" />
                                    </Listeners>
                                </Z:ZDateField >
                            </Items>
                        </Z:ZCompositeField>
                    </Items>
                    <Buttons>
                        <ext:Button ID="cmdMostrarReporte" runat="server" Text="Mostrar Reporte" >
                            <Listeners>
                                <Click Handler="mostrarReporte();" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>

                <ext:TabStrip ID="tabsReporte" runat="server" AutoWidth="true" Hidden="true">
                    <Items>
                        <ext:TabStripItem runat="server" Title="Informacion General" ActionItemID="informacionGeneral" />
                        <ext:TabStripItem runat="server" Title="Asistencia por Fechas"  ActionItemID="asistenciaPorFechas" />
                        <ext:TabStripItem runat="server" Title="Asistencia por Fechas (Grafica de Promedios)"  ActionItemID="asistenciaPorFechasGraficaPromedios" />
                    </Items>
                </ext:TabStrip>

                <ext:Window 
                    ID="wndGraficaDeComportamiento" 
                    runat="server" 
                    Title="Comportamiento Semanal" 
                    Hidden="true" 
                    Draggable="false"
                    Resizable="false"
                    Modal="true"
                    Width="800"
                    Height="300">
                    <Content>
                        <div id="chartAsistenciaPorFechasGraficaComportamiento" style="width: 100%; height: 250px"></div>
                    </Content>
                </ext:Window>

            </Items>
        </ext:Viewport>

        <div id="informacionGeneral" style="min-height:400px; display:none">
            <table class="gridDatos" border="1" width="100%">
                <tr>
                    <td>Cantidad de Celulas: <span id="cantidadDeCelulas" class="bold"></span></td>
                </tr>
                <tr>
                    <td>Cantidad de Miembros: <span id="cantidadDeMiembros" class="bold"></span> (H:<span id="cantidadDeMiembrosHombres" class="bold"></span> | M:<span id="cantidadDeMiembrosMujeres" class="bold"></span>)</td>
                </tr>
                <tr>
                    <td>Cantidad de Lideres de Celula: <span id="cantidadDeLideresDeCelula" class="bold"></span></td>
                </tr>
                <tr>
                    <td>Cantidad de Estacas: <span id="cantidadDeEstacas" class="bold"></span></td>
                </tr>
                <tr>
                    <td>Cantidad de Folis: <span id="cantidadDeFolis" class="bold"></span></td>
                </tr>
                <tr>
                    <td>Cantidad de Miembros que Asisten a la Iglesia: <span id="cantidadDeMiembrosQueAsistenIglesia" class="bold"></span> (H:<span id="cantidadDeMiembrosQueAsistenIglesiaHombres" class="bold"></span> | M:<span id="cantidadDeMiembrosQueAsistenIglesiaMujeres" class="bold"></span>)</td>
                </tr>
            </table>   

            <table class="gridDatos" width="100%">
                <tr>
                    <td><div id="chartMiembrosVsServidores" style="width: 500px; height: 325px; margin:0px auto;"></div></td>
                    <td><div id="chartMiembrosQueAsistenIglesia" style="width: 500px; height: 325px; margin:0px auto;"></div></td>
                </tr>
            </table>
        </div>

        <div id="asistenciaPorFechas" style="min-height:100px; width:100%; overflow: auto; display:none"></div>

        <div id="asistenciaPorFechasGraficaPromedios" style="min-height:100px; width:100%; overflow: auto; display:none">
            <div id="chartAsistenciaPorFechasGraficaPromedios" style="width: 100%; height: 400px"></div>
        </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphFooter" runat="server">
    <script src="<%= ResolveUrl("~/Recursos/js/Paginas/Reportes/Celulas/asistencias.js")%>" type="text/javascript"></script>
</asp:Content>
