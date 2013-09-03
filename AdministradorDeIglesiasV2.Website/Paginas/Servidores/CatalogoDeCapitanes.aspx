<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/MasterPages/Catalogos.master" AutoEventWireup="true" CodeBehind="CatalogoDeCapitanes.aspx.cs" Inherits="AdministradorDeIglesiasV2.Website.Paginas.CatalogoDeCapitanes" %>
<%@ Register Assembly="ZagueEF.Core.Web.ExtNET" Namespace="ZagueEF.Core.Web.ExtNET.Controls" TagPrefix="Z" %>
<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>
<%@ MasterType VirtualPath="~/Paginas/MasterPages/Catalogos.master" %>

<%@ Register src="../UserControls/Miembros/FiltrosMiembros.ascx" tagname="FiltrosMiembros" tagprefix="uc1" %>
<%@ Register src="../UserControls/Miembros/ResultadoMiembros.ascx" tagname="ResultadoMiembros" tagprefix="uc2" %>
<%@ Register src="../UserControls/Buscador.ascx" tagname="Buscador" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphStores" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphFiltros" runat="server">
    <uc1:FiltrosMiembros ID="Filtros" runat="server" MostrarUsuarioActual="true" TipoDeMiembro="ServidorCapitan"/>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphEdicion" runat="server">
    <Z:ZPanelCatalogo ID="pnlEdicion" runat="server" Height="350px">
        <Items>
            <ext:ColumnLayout ID="colsEdicion" runat="server">
                <Columns>
                    <ext:LayoutColumn ColumnWidth="0.5">
                        <ext:Container ID="colEdicion1" runat="server" Layout="Form">
                            <Items>
                                <ext:Container ID="Container2" runat="server" Layout="Form">
                                    <Content>
                                          <uc3:Buscador ID="BuscadorCapitan" runat="server" TipoDeBusqueda="Miembro" Titulo="Capitan" Altura="355" TipoDeLista="Simple" />  
                                    </Content>
                                </ext:Container>
                            </Items>
                        </ext:Container>
                    </ext:LayoutColumn>
                    
                    <ext:LayoutColumn ColumnWidth="0.5">
                        <ext:Container ID="Container1" runat="server" Layout="Form">
                            <Content>
                                <uc3:Buscador ID="BuscadorIntegrantes" runat="server" TipoDeBusqueda="Miembro" Titulo="Integrantes de Equipo" Altura="355" />  
                            </Content>
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

<asp:Content ID="Content5" ContentPlaceHolderID="cphFooterCatalogo" runat="server">
    <script src="../../Recursos/js/Paginas/Celulas/catalogoDeLideresDeCelula.js" type="text/javascript"></script>
</asp:Content>


