<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ResultadoMiembros.ascx.cs" Inherits="AdministradorDeIglesiasV2.Website.Paginas.UserControls.ResultadoMiembros" %>
<%@ Register Assembly="ZagueEF.Core.Web.ExtNET" Namespace="ZagueEF.Core.Web.ExtNET.Controls" TagPrefix="Z" %>
<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>

<script src='<%= ResolveUrl("~/Recursos/js/Paginas/UserControls/Miembros/resultadoMiembros.js")%>'type="text/javascript"></script>

<Z:ZGridCatalogo ID="GridDeResultados" runat="server" StoreID="StoreResultados" MostrarInformacionExtra="true" ColorearFilas="true" >
    <ColumnModel ID="ColumnModel1" runat="server">
        <Columns>
            <ext:Column Header="ID" Width="40" DataIndex="Id" />
            <ext:Column Header="Email" Width="100" DataIndex="Email" />
            <ext:Column Header="Primer Nombre" Width="100" DataIndex="PrimerNombre" />
            <ext:Column Header="Segundo Nombre" Width="100" DataIndex="SegundoNombre" />
            <ext:Column Header="Apellido Paterno" Width="100" DataIndex="ApellidoPaterno" />
            <ext:Column Header="Apellido Materno" Width="100" DataIndex="ApellidoMaterno" />
            <ext:Column Header="Célula" Width="100" DataIndex="Celula" />
            <ext:Column Header="Estado Civil" Width="100" DataIndex="EstadoCivil" />
            <ext:Column Header="Municipio" Width="100" DataIndex="Municipio" />
            <ext:Column Header="Colonia" Width="100" DataIndex="Colonia" />
            <ext:Column Header="Tel (Casa)" Width="100" DataIndex="TelCasa" />
            <ext:Column Header="Tel (Cel)" Width="100" DataIndex="TelMovil" />
            <ext:Column Header="Tel (Trabajo)" Width="100" DataIndex="TelTrabajo" />
            <ext:Column Header="Cumpleaños" Width="100" DataIndex="Nacimiento" >
                <Renderer Fn="Ext.Renderers.IsoDateSimple" />
            </ext:Column>
            <ext:CheckColumn Header="Asiste a Iglesia" Width="75" DataIndex="AsisteIglesia" />
            <ext:Column Header="Género" Width="100" DataIndex="Genero" />
        </Columns>
    </ColumnModel>
</Z:ZGridCatalogo>