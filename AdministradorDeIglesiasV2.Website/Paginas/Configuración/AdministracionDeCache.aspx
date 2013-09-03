<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" CodeBehind="AdministracionDeCache.aspx.cs" Inherits="AdministradorDeIglesiasV2.Website.Paginas.AdministracionDeCache" %>
<%@ Register Assembly="ZagueEF.Core.Web.ExtNET" Namespace="ZagueEF.Core.Web.ExtNET.Controls" TagPrefix="Z" %>
<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>
<%@ MasterType VirtualPath="~/MainMasterPage.master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <h1>Cache Limpiada!</h1>
    <br />
    <h2>Elementos eliminados:</h2>
    <textarea runat="server" id="elementosEliminados" style="width:99%; height: 150px;" readonly="readonly"></textarea>

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cphFooter" runat="server">
</asp:Content>

