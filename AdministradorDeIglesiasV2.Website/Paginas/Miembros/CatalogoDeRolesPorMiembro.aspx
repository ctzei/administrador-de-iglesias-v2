<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/MasterPages/Catalogos.master" AutoEventWireup="true" CodeBehind="CatalogoDeRolesPorMiembro.aspx.cs" Inherits="AdministradorDeIglesiasV2.Website.Paginas.CatalogoDeRolesPorMiembro" %>
<%@ Register Assembly="ZagueEF.Core.Web.ExtNET" Namespace="ZagueEF.Core.Web.ExtNET.Controls" TagPrefix="Z" %>
<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>
<%@ MasterType VirtualPath="~/Paginas/MasterPages/Catalogos.master" %>

<%@ Register src="../UserControls/Miembros/FiltrosMiembros.ascx" tagname="FiltrosMiembros" tagprefix="uc1" %>
<%@ Register src="../UserControls/Miembros/ResultadoMiembros.ascx" tagname="ResultadoMiembros" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphStores" runat="server">
        <ext:Store ID="StoreRoles" runat="server" IgnoreExtraFields="true">
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
    <uc1:FiltrosMiembros ID="Filtros" runat="server" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphEdicion" runat="server">
    <Z:ZPanelCatalogo ID="pnlEdicion" runat="server" Height="150px">
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
                                <Z:ZTextField 
                                    ID="registroEmail" 
                                    runat="server"
                                    FieldLabel="Email"
                                    AllowBlank="false"
                                    ReadOnly="true"
                                    />
                                <Z:ZCompositeField ID="ZCompositeField4" runat="server" FieldLabel="Nombre (s)">
                                    <Items>
                                        <Z:ZTextField 
                                            ID="registroPrimerNombre" 
                                            runat="server"
                                            FieldLabel="Primer Nombre"
                                            AllowBlank="false"
                                            ReadOnly="true"
                                            Flex="1"
                                            />
                                        <Z:ZTextField 
                                            ID="registroSegundoNombre" 
                                            runat="server"
                                            FieldLabel="Segundo Nombre"
                                            ReadOnly="true"
                                            Flex="1"
                                            />
                                    </Items>
                                </Z:ZCompositeField>
                                <Z:ZTextField 
                                    ID="registroApellidoPaterno" 
                                    runat="server"
                                    FieldLabel="Apellido Paterno"
                                    AllowBlank="false"
                                    ReadOnly="true"
                                    />
                                <Z:ZTextField 
                                    ID="registroApellidoMaterno" 
                                    runat="server"
                                    FieldLabel="Apellido Materno"
                                    ReadOnly="true"
                                    />
                            </Items>
                        </ext:Container>
                    </ext:LayoutColumn>
                    
                    <ext:LayoutColumn ColumnWidth="0.5">
                        <ext:Container ID="colEdicion2" runat="server" Layout="Form">
                            <Items>
                                <Z:ZGridPanel ID="registroRoles" Title="Roles" runat="server" StoreID="StoreRoles" Height="145">
                                    <ColumnModel>
                                        <Columns>
                                            <ext:CheckColumn Header="" Width="30" DataIndex="Marcado" Editable="true" >
                                                <Editor>
                                                    <ext:Checkbox ID="Checkbox2" runat="server" AllowBlank="false" />
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
    <uc2:ResultadoMiembros ID="Resultados" runat="server" />
</asp:Content>

