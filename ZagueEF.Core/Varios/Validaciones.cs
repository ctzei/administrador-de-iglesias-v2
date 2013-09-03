using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Data;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace ZagueEF.Core
{
    public class Validaciones
    {
        #region Validaciones

        /// <summary>
        /// Valida que la fecha de entrada se encuentre en rangos permitidos, de lo contrario regresa la fecha actual
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public static DateTime ValidarFecha(object fecha)
        {
            int fechaLimiteInferior = 1900;
            DateTime rtn = (fecha != null) ? (DateTime)fecha : new DateTime();
            try
            {
                if (rtn.Year < fechaLimiteInferior)
                {
                    rtn = rtn.AddYears(fechaLimiteInferior - rtn.Year);
                }
                return new DateTime(rtn.Year, rtn.Month, rtn.Day);
            }
            catch (Exception)
            {
                return DateTime.Now;
            }
        }

        /// <summary>
        /// Valida que el texto de entrada tengo un formato de "email" correcto (Genera una excepcion al no ser valido)
        /// </summary>
        /// <param name="email"></param>
        public static void ValidarEmail(string email)
        {
             ValidarEmail(email, true);
        }

        /// <summary>
        /// Valida que el texto de entrada tengo un formato de "email" correcto
        /// </summary>
        /// <param name="email"></param>
        /// <param name="generarExcepcion">Determina si se genera una excepcion o se regresa un boleano unicamente</param>
        public static bool ValidarEmail(string email, bool generarExcepcion)
        {
            if (email == null) { email = string.Empty; }
            Regex emailRegex = new Regex(@"[a-zA-Z0-9_\-\.]+@[a-zA-Z0-9_\-\.]+\.[a-zA-Z]{2,5}");
            Match match = emailRegex.Match(email);
            if (!match.Success)
            {
                if (generarExcepcion)
                {
                    throw new ExcepcionReglaNegocio("El formato del email no es el correcto.");
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Valida los datos de algun DataTable (tales como los posibles booleans en blanco...)
        /// </summary>
        /// <param name="dt"></param>
        public static void ValidarDataTable(ref DataTable dt)
        {
            //Establecemos como FALSE las columnas de tipo BOOLEAN que no tengan valor (cadenas en blanco)
            foreach (DataRow dr in dt.Rows)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    if (dc.DataType == typeof(Boolean))
                    {
                        if (dr[dc].ToString().ToLowerInvariant() != "true")
                        {
                            dr[dc] = false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Determina si el directorio especificado existe, y de no ser asi lo crea
        /// </summary>
        /// <param name="dir"></param>
        /// <returns>TRUE = se creo el directorio; FALSE = ya existia el directorio</returns>
        public static bool ValidarDirectorio(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Determina si un campo especifico fue modificado en cierta entidad
        /// </summary>
        /// <returns>TRUE = el campo especificado si fue modificado; FALSE = el campo especificado se quedo igual</returns>
        public static bool ValidarCambiosEnCampo(System.Data.Objects.ObjectStateEntry entidad, string nombreDelCampo)
        {
            for (int i = 0; i < entidad.OriginalValues.FieldCount; i++)
            {
                if (entidad.OriginalValues.GetName(i).Equals(nombreDelCampo, StringComparison.OrdinalIgnoreCase))
                {
                    if (!entidad.OriginalValues.GetString(i).Equals(entidad.CurrentValues.GetString(i), StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            return false;
        }

        #endregion
    }
}
