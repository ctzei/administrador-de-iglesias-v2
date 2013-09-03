using System;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Data.EntityClient;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace AdministradorDeIglesiasV2.Core.Modelos
{
    public partial class IglesiaEntities : ObjectContext
    {
        partial void OnContextCreated()
        {
            this.CommandTimeout = 180; //Forzamos todos los comandos a la BD a esperar 3 minutos antes de hacer "timeout"
        }
    }
}
