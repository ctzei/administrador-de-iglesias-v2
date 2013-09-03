using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZagueEF.Core
{
    public class RegistrosHelper
    {

        /// <summary>
        /// Obtenemos los registros nuevos/eliminados a partir de una lista de registros; el formato debera de ser [registro1][registro2]... y dentro de cada registro sera una lista de campos con valores como [campo1, valor1][campor2, valor2]...
        /// </summary>
        /// <param name="registros">La lista de diccionarios que definen los registros</param>
        /// <returns></returns>
        public static RegistrosDeDatos ObtenerRegistrosDiferenciados(List<Dictionary<string, string>> registros)
        {
            Dictionary<string, bool> registroEliminadoKeys = new Dictionary<string, bool>();
            registroEliminadoKeys.Add("Marcado", false);
            registroEliminadoKeys.Add("deleted", true);

            return ObtenerRegistrosDiferenciados(registros, "Id", registroEliminadoKeys);
        }

        /// <summary>
        /// Obtenemos los registros nuevos/eliminados a partir de una lista de registros; el formato debera de ser [registro1][registro2]... y dentro de cada registro sera una lista de campos con valores como [campo1, valor1][campor2, valor2]...
        /// </summary>
        /// <param name="registros">La lista de diccionarios que definen los registros (Es una lista de objetos JSON)</param>
        /// <param name="registroIdKey">El campo que define el valor a regresar como id (Default: "id")</param>
        /// <param name="registroEliminadoKeys">El o los campos que define si un registro fue eliminado o no; es uno o varios campos y el primero en ser encontrado es el que se usara (Default: "checked, deleted")</param>
        /// <returns></returns>
        public static RegistrosDeDatos ObtenerRegistrosDiferenciados(List<Dictionary<string, string>> registros, string registroIdKey, Dictionary<string, bool> registroEliminadoKeys)
        {
            RegistrosDeDatos rtn = new RegistrosDeDatos();

            foreach (Dictionary<string, string> registro in registros)
            {
                int id = int.Parse(registro[registroIdKey]);
                bool registroEliminado = false;

                //Determinamos si el registro recibido es uno nuevo/modificado o uno eliminado
                //###################//
                foreach (KeyValuePair<string, bool> registroEliminadoKey in registroEliminadoKeys)
                {
                    if (registro.Keys.Contains(registroEliminadoKey.Key))
                    {
                        if (bool.TryParse(registro[registroEliminadoKey.Key], out registroEliminado))
                        {
                            registroEliminado = !(registroEliminado ^ registroEliminadoKey.Value); //Usamos un XNOR
                            break;
                        }
                        else
                        {
                            registroEliminado = false;
                        }
                    }
                    else
                    {
                        registroEliminado = false;
                    }
                }
                //###################//

                if (!registroEliminado)
                {
                    rtn.RegistrosNuevos.Add(id, registro);
                }
                else
                {
                    rtn.RegistrosEliminados.Add(id, registro);
                }
            }

            return rtn;
        }

        /// <summary>
        /// Representa los datos obtenidos desde un GRID; la lista de registros nuevos, eliminados y un id unico del GRID
        /// </summary>
        public class RegistrosDeDatos
        {
            public string Id { get; set; }
            public Dictionary<int, Dictionary<string, string>> RegistrosNuevos { get; set; }
            public Dictionary<int, Dictionary<string, string>> RegistrosEliminados { get; set; }
            public List<int> RegistrosNuevosId { 
                get {
                    if (this.RegistrosNuevos != null)
                    {
                        return this.RegistrosNuevos.Keys.ToList<int>();
                    }
                    else
                    {
                        return new List<int>();
                    }
                } 
            }
            public List<int> RegistrosEliminadosId
            {
                get
                {
                    if (this.RegistrosNuevos != null)
                    {
                        return this.RegistrosEliminados.Keys.ToList<int>();
                    }
                    else
                    {
                        return new List<int>();
                    }
                }
            }

            public RegistrosDeDatos()
            {
                this.RegistrosNuevos = new Dictionary<int, Dictionary<string, string>>();
                this.RegistrosEliminados = new Dictionary<int, Dictionary<string, string>>();
            }
        }

        /// <summary>
        /// Representa una lista de datos obtenidos de multiples GRIDs
        /// </summary>
        public class ListaDeRegistrosDeDatos
        {
            private List<RegistrosHelper.RegistrosDeDatos> listaDeRegistros;

            public List<RegistrosHelper.RegistrosDeDatos> ListaDeRegistros { get{ return listaDeRegistros;} }

            public ListaDeRegistrosDeDatos()
            {
                listaDeRegistros = new List<RegistrosDeDatos>();
            }

            public void Agregar(RegistrosHelper.RegistrosDeDatos registrosDeDatos)
            {
                listaDeRegistros.Add(registrosDeDatos);
            }

            public void Agregar(RegistrosHelper.RegistrosDeDatos registrosDeDatos, string id)
            {
                registrosDeDatos.Id = id;
                listaDeRegistros.Add(registrosDeDatos);
            }

            public RegistrosHelper.RegistrosDeDatos Obtener(string id)
            {
                foreach (RegistrosHelper.RegistrosDeDatos registrosDeDatos in listaDeRegistros)
                {
                    if (registrosDeDatos.Id.Trim().Equals(id.Trim(), StringComparison.OrdinalIgnoreCase))
                    {
                        return registrosDeDatos;
                    }
                }

                return new RegistrosDeDatos();
            }

        }
    }
}
