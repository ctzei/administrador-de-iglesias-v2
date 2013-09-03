<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/MasterPages/Catalogos.master" AutoEventWireup="true" CodeBehind="CatalogoDeRoles.aspx.cs" Inherits="AdministradorDeIglesiasV2.Website.Paginas.CatalogoDeRoles" %>
<%@ Register Assembly="ZagueEF.Core.Web.ExtNET" Namespace="ZagueEF.Core.Web.ExtNET.Controls" TagPrefix="Z" %>
<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>
<%@ MasterType VirtualPath="~/Paginas/MasterPages/Catalogos.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphStores" runat="server">
        <ext:Store ID="StoreResultados" runat="server" IgnoreExtraFields="true">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="Id" />
                        <ext:RecordField Name="Descripcion" />
                        <ext:RecordField Name="DiaSemanaDesc" />
                        <ext:RecordField Name="HoraDiaDesc" />
                    </Fields>
                </ext:JsonReader>
            </Reader>            
        </ext:Store>

        <ext:Store ID="StorePantallasPermitidas" runat="server" IgnoreExtraFields="true">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="Id" />
                        <ext:RecordField Name="Marcado" />
                        <ext:RecordField Name="Nombre" />
                        <ext:RecordField Name="Categoria" />
                        <ext:RecordField Name="AppId" />
                    </Fields>
                </ext:JsonReader>
            </Reader>            
        </ext:Store>

        <ext:Store ID="StorePermisosEspeciales" runat="server" IgnoreExtraFields="true">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="Id" />
                        <ext:RecordField Name="Marcado" />
                        <ext:RecordField Name="Descripcion" />
                    </Fields>
                </ext:JsonReader>
            </Reader>            
        </ext:Store>

        <ext:Store ID="StoreRolesAsignables" runat="server" IgnoreExtraFields="true">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="Id" />
                        <ext:RecordField Name="Marcado" />
                        <ext:RecordField Name="Descripcion" />
                    </Fields>
                </ext:JsonReader>
            </Reader>            
        </ext:Store>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphFiltros" runat="server">
    <Z:ZPanelCatalogo ID="pnlFiltros" runat="server" Height="25px">
        <Items>
            <ext:ColumnLayout ID="colsFiltros" runat="server">
                <Columns>
                    <ext:LayoutColumn ColumnWidth="0.5">
                        <ext:Container ID="colFiltros1" runat="server" Layout="Form">
                            <Items>
                                <Z:ZNumberField 
                                    ID="filtroId" 
                                    runat="server"
                                    FieldLabel="Id"
                                    />
                            </Items>
                        </ext:Container>
                    </ext:LayoutColumn>
                    
                    <ext:LayoutColumn ColumnWidth="0.5">
                        <ext:Container ID="colFiltros2" runat="server" Layout="Form">
                            <Items>
                                <Z:ZTextField 
                                    ID="filtroDescripcion" 
                                    runat="server"
                                    FieldLabel="Descripción"
                                    AnchorHorizontal="90%"
                                    />
                            </Items>
                        </ext:Container>
                    </ext:LayoutColumn>
                </Columns>
            </ext:ColumnLayout>
        </Items>
    </Z:ZPanelCatalogo>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphEdicion" runat="server">
    <Z:ZPanelCatalogo ID="pnlEdicion" runat="server" Height="370px">
        <Items>
            <ext:ColumnLayout ID="colsEdicion" runat="server">
                <Columns>
                    <ext:LayoutColumn ColumnWidth="0.5">
                        <ext:Container ID="colEdicion1" runat="server" Layout="Form">
                            <Items>
                                <Z:ZNumberField 
                                    ID="registroId" 
                                    runat="server"
                                    FieldLabel="Id"
                                    ReadOnly="true"
                                    Text="-1"
                                    AllowBlank="false"
                                    />
                                <Z:ZGridPanel ID="registroPantallasPermitidas" Title="Pantallas Permitidas" runat="server" StoreID="StorePantallasPermitidas" Height="330">
                                    <ColumnModel>
                                        <Columns>
                                            <ext:CheckColumn Header="" Width="30" DataIndex="Marcado" Editable="true" >
                                                <Editor>
                                                    <ext:Checkbox ID="Checkbox1" runat="server" AllowBlank="false" />
                                                </Editor>
                                            </ext:CheckColumn>
                                            <ext:Column Header="Nombre" Width="175" DataIndex="Nombre" />
                                            <ext:Column Header="Categoria" Width="80" DataIndex="Categoria" />
                                            <ext:Column Header="AppId" Width="50" DataIndex="AppId" />
                                        </Columns>
                                    </ColumnModel>
                                </Z:ZGridPanel>
                            </Items>
                        </ext:Container>
                    </ext:LayoutColumn>
                    
                    <ext:LayoutColumn ColumnWidth="0.5">
                        <ext:Container ID="colEdicion2" runat="server" Layout="Form">
                            <Items>
                                <Z:ZTextField 
                                    ID="registroDescripcion" 
                                    runat="server"
                                    FieldLabel="Descripción"
                                    AllowBlank="false"
                                    />
                                <Z:ZGridPanel ID="registroPermisosEspeciales" Title="Permisos Especiales" runat="server" StoreID="StorePermisosEspeciales" Height="120">
                                    <ColumnModel>
                                        <Columns>
                                            <ext:CheckColumn Header="" Width="30" DataIndex="Marcado" Editable="true" >
                                                <Editor>
                                                    <ext:Checkbox ID="Checkbox2" runat="server" AllowBlank="false" />
                                                </Editor>
                                            </ext:CheckColumn>
                                            <ext:Column Header="Descripción" Width="175" DataIndex="Descripcion" />
                                        </Columns>
                                    </ColumnModel>
                                </Z:ZGridPanel>
                                <ext:Container runat="server" Height="10px"></ext:Container>
                                <Z:ZGridPanel ID="registroRolesAsignables" Title="Roles Asignables" runat="server" StoreID="StoreRolesAsignables" Height="200">
                                    <ColumnModel>
                                        <Columns>
                                            <ext:CheckColumn Header="" Width="30" DataIndex="Marcado" Editable="true" >
                                                <Editor>
                                                    <ext:Checkbox ID="Checkbox3" runat="server" AllowBlank="false" />
                                                </Editor>
                                            </ext:CheckColumn>
                                            <ext:Column Header="Descripción" Width="320" DataIndex="Descripcion" />
                                        </Columns>
                                    </ColumnModel>
                                </Z:ZGridPanel>
                            </Items>
                        </ext:Container>
                    </ext:LayoutColumn>
                </Columns>
            </ext:ColumnLayout>
        </Items>
    </Z:ZPanelCatalogo>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cphGridDeResultados" runat="server">
    <Z:ZGridCatalogo ID="GridDeResultados" runat="server" StoreID="StoreResultados">
        <ColumnModel ID="ColumnModel1" runat="server">
            <Columns>
                <ext:Column Header="ID" Width="40" DataIndex="Id" />
                <ext:Column Header="Descripción" Width="200" DataIndex="Descripcion" />
            </Columns>
        </ColumnModel>
    </Z:ZGridCatalogo>
</asp:Content>

