using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZagueEF.Core.Web.Interfaces
{
    public interface ICatalogo
    {
        void CargarControles();
        void Buscar();
        void Mostrar(int id);
        void Borrar(int id);
        void Guardar(int id, RegistrosHelper.ListaDeRegistrosDeDatos listaDeRegistrosDeDatos);      
    }
}
