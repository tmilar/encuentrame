using System;
using System.Data;
using System.IO;
using NailsFramework.IoC;
using NailsFramework.Persistence;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Encuentrame.Tests.Supports
{
    public static class DatabaseCreator
    {
        [Inject]
        public static NHibernateContext NHibernateContext { private get; set; }

        [Inject("ScriptsPath")]
        public static string ScriptsPath { private get; set; }

        private static SchemaExport GetSchemaExport(string fileName, bool writeScripts)
        {
            var schemaExport = new SchemaExport(NHibernateContext.Configuration);
            if (writeScripts) 
                return schemaExport.SetOutputFile(Path.Combine(ScriptsPath, fileName));
            else 
                return schemaExport;
        }

       
        public static void Create(IDbConnection connection)
        {
            new SchemaExport(NHibernateContext.Configuration).Execute((s) => { }, true, false, connection, null);
        }

    
        public static void Create()
        {
            Create(NHibernateContext.CurrentSession.Connection);
        }
    }
}