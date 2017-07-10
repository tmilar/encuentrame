using System.IO;
using System.Net;
using NailsFramework.IoC;
using NailsFramework.Persistence;
using NHibernate.Tool.hbm2ddl;
using NailsFramework;
using NHibernate.Cfg;

namespace Encuentrame.Support.Mappings
{
    public static class DatabaseCreator
    {

        [Inject("ScriptsPath")]
        public static string ScriptsPath { private get; set; }

        [Inject]
        public static NHibernateContext NHibernateContext { private get; set; }

        private static SchemaExport GetSchemaExport(string fileName, bool writeScripts)
        {
            var schemaExport = new SchemaExport(NHibernateContext.Configuration);
            if (writeScripts)
            {
                return schemaExport.SetOutputFile(Path.Combine(ScriptsPath, fileName));
            }
            else
            {
                return schemaExport;
            }


        }
        private static SchemaUpdate GetSchemaUpdate()
        {
            var schemaUpdate = new SchemaUpdate(NHibernateContext.Configuration);

            return schemaUpdate;
        }

        public static void Create(bool writeScripts)
        {
            GetSchemaExport("ScriptDrop.sql", writeScripts).Execute(true, true, true);
            GetSchemaExport("ScriptCreate.sql", writeScripts).Execute(true, true, false);

        }
        public static void Create()
        {
            Create(ScriptsPath.NotIsNullOrEmpty());
        }
        public static void Update()
        {
            if (ScriptsPath.NotIsNullOrEmpty())
            {
                StreamWriter fileWrite = new StreamWriter(Path.Combine(ScriptsPath, "ScriptUpdate.sql"), false);
                GetSchemaUpdate().Execute((str) =>
                {
                    fileWrite.Write(str);
                }, true);
                fileWrite.Close();
            }
            else
            {
                GetSchemaUpdate().Execute(false, true);    
            }
            
        }
    }
}