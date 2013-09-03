using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZagueEF.Core.Web.Interfaces
{
    public interface ICatalogoSoloLectura
    {
        void CargarControles();
        void Buscar();
        void Mostrar(int id);
    }
}
