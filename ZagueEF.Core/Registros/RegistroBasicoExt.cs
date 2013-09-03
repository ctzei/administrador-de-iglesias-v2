using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZagueEF.Core
{
    /// <summary>
    /// Es el registro mas basico, utilizado comunmente en listados (con propiedades extras)
    /// </summary>
    public class RegistroBasicoExt : RegistroBasico
    {
        /// <summary>
        /// Es un valor tipo "checkbox" (OPCIONAL)
        /// </summary>
        public bool Marcado { get; set; }

        /// <summary>
        /// Es un valor que guarda una relacion con otro registros (OPCIONAL)
        /// </summary>
        public int PadreId { get; set; }
    }
}
