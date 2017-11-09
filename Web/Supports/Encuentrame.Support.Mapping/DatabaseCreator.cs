using System.Data;
using System.IO;
using System.Net;
using NailsFramework.IoC;
using NailsFramework.Persistence;
using NHibernate.Tool.hbm2ddl;
using NailsFramework;
using NHibernate.Cfg;
using NHibernate.Proxy;

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

            //CreateStoredSoughtPeople();


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

        //private static void CreateStoredSoughtPeople()
        //{
        //    var dropSpSoughtPeople = @"
        //                            IF OBJECT_ID('SoughtPeople', 'P') IS NOT NULL
        //                            DROP PROC SoughtPeople
        //                            ";

        //    var addSpSoughtPeople = @"
        //                            CREATE PROCEDURE SoughtPeople 
	       //                             @userId int , @eventId int
        //                            AS
        //                            BEGIN
	       //                             DECLARE @source geography 
        //                            set @source = (select top 1 geography::Point(Latitude, Longitude , 4326) from Positions where UserId=@userId order by Creation desc);

        //                            SELECT top 20 aa.Target_id AS [UserId], min( @source.STDistance(geography::Point(Latitude, Longitude , 4326)) ) as Distance
        //                                FROM BaseAreYouOks aa 
        //                                inner join positions pp 
        //                                on aa.Target_id=pp.UserId  
        //                                where aa.Event_id=@eventId and aa.ReplyDatetime is null and aa.Target_id<>@userId
        //                                group by Target_id
        //                                order by min( @source.STDistance(geography::Point(Latitude, Longitude , 4326)) ); 
        //                            END
                                    
        //                                                                ";

        //    var dropCommand = NHibernateContext.CurrentSession.Connection.CreateCommand();

        //    dropCommand.CommandText = dropSpSoughtPeople;

        //    NHibernateContext.CurrentSession.Transaction.Enlist(dropCommand);

        //    dropCommand.ExecuteNonQuery();



        //    var addCommand = NHibernateContext.CurrentSession.Connection.CreateCommand();

        //    addCommand.CommandText = addSpSoughtPeople;

        //    NHibernateContext.CurrentSession.Transaction.Enlist(addCommand);

        //    addCommand.ExecuteNonQuery();





        //}
    }
}