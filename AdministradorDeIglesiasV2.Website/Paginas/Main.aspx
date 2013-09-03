<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="AdministradorDeIglesiasV2.Website.Paginas.Main" %>
<%@ Register Assembly="ZagueEF.Core.Web.ExtNET" Namespace="ZagueEF.Core.Web.ExtNET.Controls" TagPrefix="Z" %>
<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>
<%@ MasterType VirtualPath="~/MainMasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <ext:Viewport ID="ViewportMain" runat="server" Layout="border">
        <Items>
            <ext:BorderLayout ID="BorderLayout1" runat="server">
                <West Collapsible="true" Split="true" CollapseMode="Mini">
                    <Z:ZTreePanel ID="pnlPantallas" runat="server" Title="Navegación" RootVisible="false" Width="200">
                        <Tools>
                            <ext:Tool Type="Close" Qtip="Cerrar Sesión" Handler="cerrarSesion();"></ext:Tool>
                        </Tools>
                    </Z:ZTreePanel>
                </West>
                <Center>
                    <ext:TabPanel ID="pnlTabsDePantallas" runat="server">
                        <Content>
                            <div id="divLogo" class="logo faded main-area"></div>
                        </Content>
                        <Plugins>
                            <ext:TabCloseMenu CloseAllTabsText="Cerrar todas" CloseOtherTabsText="Cerrar otras" CloseTabText="Cerrar actual" />
                        </Plugins>
                    </ext:TabPanel>
                </Center>
            </ext:BorderLayout>
        </Items>
    </ext:Viewport>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphFooter" runat="server">
    <script src="<%= ResolveUrl("~/Recursos/js/Paginas/main.js")%>" type="text/javascript"></script>
</asp:Content>
