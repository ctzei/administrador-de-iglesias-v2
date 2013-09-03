<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="DetallesDeEvento.aspx.cs" Inherits="AdministradorDeIglesiasV2.Website.Paginas.Alabanza.DetallesDeEvento" %>
<%@ Register Assembly="ZagueEF.Core.Web.ExtNET" Namespace="ZagueEF.Core.Web.ExtNET.Controls" TagPrefix="Z" %>
<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>
<%@ MasterType VirtualPath="~/MainMasterPage.master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

    <ext:Store ID="StoreMiembros" runat="server" IgnoreExtraFields="true">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="MiembroId" />
                    <ext:RecordField Name="Nombre" />
                    <ext:RecordField Name="Email" />
                    <ext:RecordField Name="Instrumento" />
                    <ext:RecordField Name="Asistencia" Type="Boolean" />
                    <ext:RecordField Name="Retraso" Type="Boolean" />
                </Fields>
            </ext:JsonReader>
        </Reader>            
    </ext:Store>

    <ext:Store ID="StoreCanciones" runat="server" IgnoreExtraFields="true">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="CancionId" />
                    <ext:RecordField Name="Titulo" />
                    <ext:RecordField Name="Artista" />
                    <ext:RecordField Name="Disco" />
                    <ext:RecordField Name="Tono" />
                    <ext:RecordField Name="Liga" />
                </Fields>
            </ext:JsonReader>
        </Reader>            
    </ext:Store>

    <ext:Store ID="StoreEnsayos" runat="server" IgnoreExtraFields="true">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="EnsayoId" />
                    <ext:RecordField Name="Fecha" />
                    <ext:RecordField Name="HoraInicio" />
                    <ext:RecordField Name="HoraFin" />
                </Fields>
            </ext:JsonReader>
        </Reader>            
    </ext:Store>

    <ext:Viewport ID="Viewport" runat="server" Layout="BorderLayout">
        <Items>

            <ext:FormPanel ID="pnlDatosGenerales" runat="server" Title="Datos Generales" ButtonAlign="Center" PaddingSummary="5px 5px 0" BodyBorder="false" LabelWidth="80" Height="165px" Region="North">
                <Items>
                    <ext:DisplayField ID="registroId" runat="server" FieldLabel="Id" Cls="bold"></ext:DisplayField>
                    <ext:DisplayField ID="registroDescripcion" runat="server" FieldLabel="Descripción" Cls="bold"></ext:DisplayField>
                    <ext:DisplayField ID="registroFecha" runat="server" FieldLabel="Fecha" Cls="bold"></ext:DisplayField>
                    <ext:DisplayField ID="registroHoraInicio" runat="server" FieldLabel="Hora (inicio)" Cls="bold"></ext:DisplayField>
                    <ext:DisplayField ID="registroHoraFin" runat="server" FieldLabel="Hora (fin)" Cls="bold"></ext:DisplayField>
                </Items>
            </ext:FormPanel>
            <ext:TabPanel runat="server" Region="Center">
                <Items>
                    <Z:ZGridPanel ID="gridMiembros" runat="server" Title="Miembros" StoreID="StoreMiembros">
                        <ColumnModel>
                            <Columns>
                                <ext:Column Header="Nombre" Width="150" DataIndex="Nombre">
                                    <Renderer Fn="Ext.Renderers.DetallesDeMiembro" />
                                </ext:Column>
                                <ext:Column Header="Email" Width="125" DataIndex="Email" />
                                <ext:Column Header="Instrumento" Width="100" DataIndex="Instrumento" />
                                <ext:CheckColumn Header="Asistencia" Width="35" DataIndex="Asistencia" />
                                <ext:CheckColumn Header="Retraso" Width="35" DataIndex="Retraso" />
                            </Columns>
                        </ColumnModel>
                    </Z:ZGridPanel>
                    <Z:ZGridPanel ID="gridCanciones" runat="server" Title="Canciones" StoreID="StoreCanciones">
                        <ColumnModel>
                            <Columns>
                                <ext:Column Header="Título" Width="125" DataIndex="Titulo">
                                    <Renderer Fn="Ext.Renderers.DetallesDeCancion" />
                                </ext:Column>
                                <ext:Column Header="Artista" Width="125" DataIndex="Artista" />
                                <ext:Column Header="Disco" Width="75" DataIndex="Disco" />
                                <ext:Column Header="Tono" Width="30" DataIndex="Tono" />
                                <ext:Column Header="Liga" Width="100" DataIndex="Liga">
                                    <Renderer Fn="Ext.Renderers.Url" />
                                </ext:Column>
                            </Columns>
                        </ColumnModel>
                    </Z:ZGridPanel>
                    <Z:ZGridPanel ID="gridEnsayos" runat="server" Title="Ensayos" StoreID="StoreEnsayos">
                        <ColumnModel>
                            <Columns>
                                <ext:Column Header="Fecha" Width="100" DataIndex="Fecha">
                                    <Renderer Fn="Ext.Renderers.IsoDateSimple" />
                                </ext:Column>
                                <ext:Column Header="Hora (inicio)" Width="100" DataIndex="HoraInicio">
                                    <Renderer Fn="Ext.Renderers.DetallesDeEnsayo" />
                                </ext:Column>
                                <ext:Column Header="Hora (fin)" Width="100" DataIndex="HoraFin">
                                    <Renderer Fn="Ext.Renderers.DetallesDeEnsayo" />
                                </ext:Column>
                            </Columns>
                        </ColumnModel>
                    </Z:ZGridPanel>
                </Items>
            </ext:TabPanel>

        </Items>
    </ext:Viewport>

</asp:Content>