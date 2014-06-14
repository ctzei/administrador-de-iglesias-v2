<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="DetallesDeMiembro.aspx.cs" Inherits="AdministradorDeIglesiasV2.Website.Paginas.DetallesDeMiembro" %>
<%@ Register Assembly="ZagueEF.Core.Web.ExtNET" Namespace="ZagueEF.Core.Web.ExtNET.Controls" TagPrefix="Z" %>
<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>
<%@ MasterType VirtualPath="~/MainMasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <meta name="viewport" content="initial-scale=1.0, user-scalable=no"/> 
    <script src="http://maps.google.com/maps/api/js?sensor=false" type="text/javascript"></script> 
    <script src='<%= ResolveUrl("~/Recursos/js/extjs.gmaps.helpers.js")%>' type="text/javascript"></script>
    <script src='<%= System.Web.Configuration.WebConfigurationManager.AppSettings["jQueryUrl"] %>' type="text/javascript"></script>
    <script src='<%= ResolveUrl("~/Recursos/js/Highcharts/highcharts.js")%>' type="text/javascript"></script>

    <style type="text/css">
        .tableInsidePanel-100 table 
        {
            width: 100%;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

        <ext:Store ID="StoreLideresDirectos" runat="server" IgnoreExtraFields="true">
            <Reader>
                <ext:JsonReader IDProperty="Id">
                    <Fields>
                        <ext:RecordField Name="Id" />
                        <ext:RecordField Name="Nombre" />
                        <ext:RecordField Name="Email" />
                        <ext:RecordField Name="TelMovil" />
                        <ext:RecordField Name="TelCasa" />
                        <ext:RecordField Name="TelTrabajo" />
                    </Fields>
                </ext:JsonReader>
            </Reader>            
        </ext:Store>

        <ext:Store ID="StoreLiderzagoDirecto" runat="server" IgnoreExtraFields="true">
            <Reader>
                <ext:JsonReader IDProperty="Id">
                    <Fields>
                        <ext:RecordField Name="Id" />
                        <ext:RecordField Name="Descripcion" />
                    </Fields>
                </ext:JsonReader>
            </Reader>            
        </ext:Store>

        <ext:Store ID="StoreLiderzagoIndirecto" runat="server" IgnoreExtraFields="true">
            <Reader>
                <ext:JsonReader IDProperty="Id">
                    <Fields>
                        <ext:RecordField Name="Id" />
                        <ext:RecordField Name="Descripcion" />
                    </Fields>
                </ext:JsonReader>
            </Reader>            
        </ext:Store>

        <ext:Viewport ID="Viewport" runat="server" Layout="Border">
            <Items>

                <ext:TabPanel ID="TabPanel1" runat="server" Region="Center">
                    <Items>
                        <ext:FormPanel ID="pnlDatosGenerales" runat="server" Title="Datos Generales" AutoHeight="true" ButtonAlign="Center" PaddingSummary="5px 5px 0" BodyBorder="false" LabelWidth="200" Layout="Fit">
                            <Items>
                                <ext:TableLayout ID="tblDatosGenerales" runat="server" Columns="2" AnchorHorizontal="100%" Flex="1" StyleSpec="width:100%" Cls="tableInsidePanel-100" >
                                    <Cells>
                                        <ext:Cell>
                                            <ext:FormPanel ID="FormPanel1" runat="server" AutoHeight="true" ButtonAlign="Center" PaddingSummary="5px 5px 0" BodyBorder="false" LabelWidth="85">
                                                <Items>
                                                    <ext:DisplayField ID="registroId" runat="server" FieldLabel="Id" Cls="bold"></ext:DisplayField>
                                                    <ext:DisplayField ID="registroNombre" runat="server" FieldLabel="Nombre" Cls="bold"></ext:DisplayField>
                                                    <ext:DisplayField ID="registroRed" runat="server" FieldLabel="Red" Cls="small"></ext:DisplayField>
                                                    <ext:DisplayField ID="registroEmail" runat="server" FieldLabel="Email" Cls="bold"></ext:DisplayField>
                                                    <ext:DisplayField ID="registroMunicipio" runat="server" FieldLabel="Municipio" Cls="bold"></ext:DisplayField>
                                                    <ext:DisplayField ID="registroColonia" runat="server" FieldLabel="Colonia" Cls="bold"></ext:DisplayField>
                                                    <ext:DisplayField ID="registroDireccion" runat="server" FieldLabel="Dirección" Cls="bold"></ext:DisplayField>  
                                                    <ext:DisplayField ID="registroEstadoCivil" runat="server" FieldLabel="Estado Civil" Cls="bold"></ext:DisplayField>  
                                                    <ext:DisplayField ID="registroFechaDeNacimiento" runat="server" FieldLabel="Nacimiento" Cls="bold"></ext:DisplayField>
                                                    <ext:DisplayField ID="registroTelefonos" runat="server" FieldLabel="Teléfonos" Cls="bold"></ext:DisplayField>  
                                                    <ext:DisplayField ID="registroComentario" runat="server" FieldLabel="Comentario" Cls="bold"></ext:DisplayField>  
                                                </Items>
                                            </ext:FormPanel>
                                        </ext:Cell>
                                        <ext:Cell>
                                             <ext:Image ID="registroFoto" runat="server" Height="160px" MaxHeight="160px" AutoHeight="false" AutoWidth="false" ImageUrl="../../Recursos/img/sin_foto.jpg"></ext:Image>
                                        </ext:Cell>
                                        <ext:Cell ColSpan="2">
                                            <ext:Panel ID="gridDireccion" runat="server" AnchorHorizontal="right" Height="400px" Title="Direccion" AutoWidth="true"/>
                                        </ext:Cell>
                                    </Cells>
                                </ext:TableLayout>
                            </Items>
                        </ext:FormPanel>
                         <ext:FormPanel ID="pnlLideres" runat="server" Title="Líderes" AutoHeight="true" ButtonAlign="Center" PaddingSummary="5px 5px 0" BodyBorder="false">
                            <Items>
                                <Z:ZGridPanel ID="gridLideresDirectos" runat="server" Title="Líderes Directos del Miembro Actual" StoreID="StoreLideresDirectos" Height="160px" AnchorHorizontal="right">
                                    <ColumnModel>
                                        <Columns>
                                            <ext:Column Header="Id" Width="25" DataIndex="Id" />
                                            <ext:Column Header="Nombre" Width="150" DataIndex="Nombre" />
                                            <ext:Column Header="Email" Width="100" DataIndex="Email" />
                                            <ext:Column Header="Tel. Móvil" Width="60" DataIndex="TelMovil" />
                                            <ext:Column Header="Tel. Casa" Width="60" DataIndex="TelCasa" />
                                            <ext:Column Header="Tel. Trabajo" Width="60" DataIndex="TelTrabajo" />
                                        </Columns>
                                    </ColumnModel>
                                </Z:ZGridPanel>
                                <Z:ZGridPanel ID="gridLiderzagoDirecto" runat="server" Title="Células de las que es Líder Directo" StoreID="StoreLiderzagoDirecto" Height="150px" AnchorHorizontal="right">
                                    <ColumnModel>
                                        <Columns>
                                            <ext:Column Header="Id" Width="25" DataIndex="Id" />
                                            <ext:Column Header="Descripción" Width="250" DataIndex="Descripcion" >
                                                <Renderer Fn="Ext.Renderers.DetallesDeCelula" />
                                            </ext:Column>
                                        </Columns>
                                    </ColumnModel>
                                </Z:ZGridPanel>
                                <Z:ZGridPanel ID="gridLiderzagoIndirecto" runat="server" Title="Células de las que es Líder Indirecto" StoreID="StoreLiderzagoIndirecto" Height="160px" AnchorHorizontal="right">
                                    <ColumnModel>
                                        <Columns>
                                            <ext:Column Header="Id" Width="25" DataIndex="Id" />
                                            <ext:Column Header="Descripción" Width="250" DataIndex="Descripcion" >
                                                <Renderer Fn="Ext.Renderers.DetallesDeCelula" />
                                            </ext:Column>
                                        </Columns>
                                    </ColumnModel>
                                </Z:ZGridPanel>
                            </Items>
                        </ext:FormPanel>
                        <ext:FormPanel ID="pnlAsistencias" runat="server" Title="Asistencias" AutoHeight="true" ButtonAlign="Center" PaddingSummary="10px 5px 0" BodyBorder="false" LabelWidth="170">
                            <Items>
                                <ext:DisplayField ID="registroUltimaAsistencia" runat="server" FieldLabel="Última asistencia" Cls="bold"></ext:DisplayField>
                            </Items>
                            <Content>
                                <div id="mainChart">
                                    <div id="pieChartContainer" style="height: 230px; width:100%;"></div>
                    	            <div id="timeChartContainer" style="height: 250px; width:100%;"></div>
                                </div>
                            </Content>
                        </ext:FormPanel>
                    </Items>
                </ext:TabPanel>

            </Items>
        </ext:Viewport>

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cphFooter" runat="server">
    <script src='<%= ResolveUrl("~/Recursos/js/Paginas/Miembros/detallesDeMiembro.js")%>' type="text/javascript"></script>
</asp:Content>

