using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZagueEF.Core
{
    /// <summary>
    /// Es el registro mas basico, utilizado comunmente en listados
    /// </summary>
    public class RegistroBasico
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
    }
}
