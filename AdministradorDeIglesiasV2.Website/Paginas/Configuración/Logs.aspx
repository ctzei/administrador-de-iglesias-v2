<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.Master" AutoEventWireup="true" validateRequest="false" CodeBehind="Logs.aspx.cs" Inherits="AdministradorDeIglesiasV2.Website.Paginas.Configuracion.Logs" %>
<%@ Register Assembly="ZagueEF.Core.Web.ExtNET" Namespace="ZagueEF.Core.Web.ExtNET.Controls" TagPrefix="Z" %>
<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>
<%@ MasterType VirtualPath="~/MainMasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">

    <style type="text/css">

        #cphMain_txtPropiedades,
        #cphMain_txtLog {
            font: 12px Consolas, Lucida Console, Courier, Monospace;
        }

    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <h1>Propiedades</h1>
    <ext:TextArea runat="server" id="txtPropiedades" StyleSpec="width:99%; height: 20vh;" ReadOnly="true"></ext:TextArea>

    <h1>Logs</h1>
    <select runat="server" id="cboLogs" onchange="cargarLog()">
        <option disabled="disabled" selected="selected">-- Seleccione --</option>
    </select>

    <h1>Log</h1>
    <ext:TextArea runat="server" id="txtLog" StyleSpec="width:99%; height: 65vh" ReadOnly="true" wrap="off"></ext:TextArea>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cphFooter" runat="server">

    <script type="text/javascript">
        function cargarLog() {
            Ext.net.DirectMethods.CargarLog(Ext.get("cphMain_cboLogs").getValue());
            return false;
        }
    </script>

</asp:Content>

