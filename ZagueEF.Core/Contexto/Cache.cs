using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Web;
using log4net;

namespace ZagueEF.Core
{
    public sealed class Cache
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Cache));

        private static readonly System.Runtime.Caching.ObjectCache cache = System.Runtime.Caching.MemoryCache.Default;
        private static volatile Cache instance;
        private static object syncRoot = new Object();

        /// <summary>
        /// Aqui se inicializan TODAS las variables cuando se instancia por primera vez para "Cache" 
        /// </summary>
        private Cache()
        {
            
        }

        public static Cache Instance
        {
            get 
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Cache();
                    }
                }

                return instance;
            }
        }

        public T Obtener<T>(string nombre) where T : class
        {
            try
            {
                if (cache.Contains(nombre))
                {
                    return (T)cache[nombre];
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public List<String> ObtenerLlaves()
        {
            return cache.Select(kvp => kvp.Key).ToList();
        }

        public bool Guardar(string nombre, object valor)
        {
            return cache.Add(nombre, valor, DateTime.Now.AddDays(5));
        }

        public bool Guardar(string nombre, object valor, int horasParaExpirar)
        {
            return cache.Add(nombre, valor, DateTime.Now.AddHours(horasParaExpirar));
        }

        public void Limpiar()
        {
            List<string> nombres = cache.Select(kvp => kvp.Key).ToList();
            foreach (string nombre in nombres)
            {
                cache.Remove(nombre);
            }
            log.Info("Cache reiniciada!");
        }

    }
}
