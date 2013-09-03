<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="DetallesDeCelula.aspx.cs" Inherits="AdministradorDeIglesiasV2.Website.Paginas.DetallesDeCelula" %>
<%@ Register Assembly="ZagueEF.Core.Web.ExtNET" Namespace="ZagueEF.Core.Web.ExtNET.Controls" TagPrefix="Z" %>
<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>
<%@ MasterType VirtualPath="~/MainMasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <meta name="viewport" content="initial-scale=1.0, user-scalable=no"/> 
    <script src="http://maps.google.com/maps/api/js?sensor=false" type="text/javascript"></script> 
    <script src="../../Recursos/js/extjs.gmaps.helpers.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

        <ext:Store ID="StoreMiembros" runat="server" IgnoreExtraFields="true">
            <Reader>
                <ext:JsonReader IDProperty="Id">
                    <Fields>
                        <ext:RecordField Name="Id" />
                        <ext:RecordField Name="Nombre" />
                        <ext:RecordField Name="Email" />
                    </Fields>
                </ext:JsonReader>
            </Reader>            
        </ext:Store>

        <ext:Store ID="StoreLideres" runat="server" IgnoreExtraFields="true">
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

        <ext:Viewport ID="Viewport" runat="server" Layout="Border">
            <Items>

                <ext:TabPanel ID="TabPanel1" runat="server" Region="Center">
                    <Items>
                        <ext:FormPanel ID="pnlDatosGenerales" runat="server" Title="Datos Generales" AutoHeight="true" ButtonAlign="Center" PaddingSummary="5px 5px 0" BodyBorder="false" LabelWidth="80">
                            <Items>
                                <ext:DisplayField ID="registroId" runat="server" FieldLabel="Id" Cls="bold"></ext:DisplayField>
                                <ext:DisplayField ID="registroDescripcion" runat="server" FieldLabel="Descripción" Cls="bold"></ext:DisplayField>
                                <ext:DisplayField ID="registroRed" runat="server" FieldLabel="Red" Cls="small"></ext:DisplayField>
                                <ext:DisplayField ID="registroMunicipio" runat="server" FieldLabel="Municipio" Cls="bold"></ext:DisplayField>
                                <ext:DisplayField ID="registroColonia" runat="server" FieldLabel="Colonia" Cls="bold"></ext:DisplayField>
                                <ext:DisplayField ID="registroDireccion" runat="server" FieldLabel="Direccion" Cls="bold"></ext:DisplayField>  
                                <Z:ZCompositeField runat="server" FieldLabel="Día/Hora" Cls="bold">
                                    <Items>
                                        <ext:DisplayField ID="registroDia" runat="server" FieldLabel="Dia"></ext:DisplayField>  
                                        <ext:DisplayField ID="registroHora" runat="server" FieldLabel="Hora"></ext:DisplayField>  
                                    </Items>
                                </Z:ZCompositeField>
                                <Z:ZGridPanel ID="gridLideres" runat="server" Title="Lideres de la Celula" StoreID="StoreLideres" Height="100px" AnchorHorizontal="right">
                                    <ColumnModel>
                                        <Columns>
                                            <ext:Column Header="Id" Width="25" DataIndex="Id" />
                                            <ext:Column Header="Nombre" Width="150" DataIndex="Nombre" />
                                            <ext:Column Header="Email" Width="100" DataIndex="Email" />
                                            <ext:Column Header="Tel. Movil" Width="60" DataIndex="TelMovil" />
                                            <ext:Column Header="Tel. Casa" Width="60" DataIndex="TelCasa" />
                                            <ext:Column Header="Tel. Trabajo" Width="60" DataIndex="TelTrabajo" />
                                        </Columns>
                                    </ColumnModel>
                                </Z:ZGridPanel>
                                <ext:Panel ID="gridDireccion" runat="server" AnchorHorizontal="right" Height="380px" Title="Direccion" AutoWidth="true"/>
                            </Items>
                        </ext:FormPanel>
                        <Z:ZGridPanel ID="gridMiembros" runat="server" Title="Miembros" StoreID="StoreMiembros">
                            <ColumnModel>
                                <Columns>
                                    <ext:Column Header="Id" Width="25" DataIndex="Id" />
                                    <ext:Column Header="Nombre" Width="150" DataIndex="Nombre" />
                                    <ext:Column Header="Email" Width="100" DataIndex="Email" />
                                </Columns>
                            </ColumnModel>
                            <FooterBar>
                                <ext:StatusBar runat="server">
                                    <Items>
                                        <ext:DisplayField ID="registroNumeroDeMiembros" runat="server" Text="{0} Miembros"></ext:DisplayField>
                                    </Items>
                                </ext:StatusBar>
                            </FooterBar>
                        </Z:ZGridPanel>
                    </Items>
                </ext:TabPanel>

            </Items>
        </ext:Viewport>

</asp:Content>