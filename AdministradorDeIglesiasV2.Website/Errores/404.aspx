﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="404.aspx.cs" Inherits="AdministradorDeIglesiasV2.Website.Paginas.Error404" %>
<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>
<%@ MasterType VirtualPath="~/MainMasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <ext:Viewport ID="LoginViewport" runat="server" Layout="Center">
        <Items>
            <ext:Panel ID="pnlLogin" runat="server" Width="300px" AutoHeight="true" Title="Error: 404" PaddingSummary="10px 20px" MonitorPoll="500" MonitorValid="true" ButtonAlign="Right">
                <Content>
                    La pagina requerida no ha sido encontrada; asegurese que el vinculo sea correcto o contacte al administrador del sistema.
                </Content>
                <Buttons>
                    <ext:Button ID="cmdRegresar" runat="server" Text="Regresar">
                        <Listeners>
                            <Click Handler="history.back(); return false;" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:Panel>
        </Items>
    </ext:Viewport>

    <ext:Viewport ID="LogoViewport" runat="server" Layout="Center">
        <Items>
            <ext:Container ID="LogoContainer" runat="server" AutoHeight="true" Height="250">
                <Content>
                    <div id="divLogo" style="margin:10px auto; text-align:center; background-image:url('<%= ResolveUrl("~/Recursos/img/Logo.png") %>'); background-position:center top; background-repeat:no-repeat; height: 200px;"></div>
                </Content>
            </ext:Container>
        </Items>
    </ext:Viewport>

</asp:Content>
