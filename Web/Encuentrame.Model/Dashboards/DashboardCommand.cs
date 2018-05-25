using System.Collections.Generic;
using System.Linq;
using Encuentrame.Model.Supports;
using Encuentrame.Support;
using NailsFramework.IoC;
using NHibernate.Transform;

namespace Encuentrame.Model.Dashboards
{
    [Lemming]
    public class DashboardCommand : BaseCommand, IDashboardCommand
    {

        public int PeopleThatIAmTracking(int? businessId)
        {
            var sql = @"SELECT
                            COUNT(DISTINCT aa.User_id) AS Amount
                        FROM Activities aa
                        WHERE NOT aa.Event_id IS NULL
                        AND (:businessId IS NULL
                        OR aa.Event_id IN (SELECT ev.Id FROM events ev INNER JOIN users us ON ev.Organizer_id=us.id  WHERE us.business_id=:businessId))
                        AND aa.User_id IN (SELECT
                            po.UserId
                        FROM Positions po
                        WHERE po.Creation >= :from)";

            var amount = NHibernateContext.CurrentSession.CreateSQLQuery(sql)
                .SetParameter("from", SystemDateTime.Now.AddDays(-1))
                .SetParameter("businessId", businessId)
                .UniqueResult<int>();
                

            return amount;
        }



        public int PeopleInEvents(int? businessId)
        {
            var sql = @"SELECT COUNT(DISTINCT aa.User_id) AS Amount
                        FROM Activities aa
                        INNER JOIN EVENTS ev ON aa.Event_id=ev.Id
                        WHERE (:businessId IS NULL
                               OR aa.Event_id IN
                                 (SELECT ev.Id
                                  FROM EVENTS ev
                                  INNER JOIN users us ON ev.Organizer_id=us.id
                                  WHERE us.business_id=:businessId))
                          AND ev.Status IN ('InProgress',
                                            'Pending',
                                            'InEmergency')";

            var amount = NHibernateContext.CurrentSession.CreateSQLQuery(sql)
                .SetParameter("businessId", businessId)
                .UniqueResult<int>();


            return amount;
        }

        public int EventsInEmergency(int? businessId)
        {
            var sql = @"SELECT COUNT(DISTINCT ev.id) AS Amount
                        FROM EVENTS ev 
                        WHERE (:businessId IS NULL
                               OR ev.id IN
                                 (SELECT ev.Id
                                  FROM EVENTS ev
                                  INNER JOIN users us ON ev.Organizer_id=us.id
                                  WHERE us.business_id=:businessId))
                          AND ev.Status IN ('InEmergency')";

            var amount = NHibernateContext.CurrentSession.CreateSQLQuery(sql)
                .SetParameter("businessId", businessId)
                .UniqueResult<int>();


            return amount;
        }

        public int PeopleWithoutAnswer(int? businessId)
        {
            var sql = @"SELECT count(distinct aa.User_id) AS Amount
                        FROM Activities aa
                        INNER JOIN EVENTS ev ON aa.Event_id=ev.Id
						INNER JOIN BaseAreYouOks ba ON ba.Target_id=aa.User_id
                        WHERE ba.Event_id=ev.Id AND ba.ReplyDatetime IS NULL AND (:businessId IS NULL
                               OR aa.Event_id IN
                                 (SELECT ev.Id
                                  FROM EVENTS ev
                                  INNER JOIN users us ON ev.Organizer_id=us.id
                                  WHERE us.business_id=:businessId))
                          AND ev.Status IN (
                                            'InEmergency')";

            var amount = NHibernateContext.CurrentSession.CreateSQLQuery(sql)
                .SetParameter("businessId", businessId)
                .UniqueResult<int>();


            return amount;

        }

        public EventStatusQuantityInfo EventsByStatus(int? businessId)
        {
            var sql = @"SELECT  SUM(CASE WHEN ev.Status='InProgress' THEN 1 ELSE 0 END) AS InProgress,
                                SUM(CASE WHEN ev.Status='Pending' THEN 1 ELSE 0 END) AS Pending,
                                SUM(CASE WHEN ev.Status='InEmergency' THEN 1 ELSE 0 END) AS InEmergency
                        FROM EVENTS ev 
                        WHERE (:businessId IS NULL
                               OR ev.id IN
                                 (SELECT ev.Id
                                  FROM EVENTS ev
                                  INNER JOIN users us ON ev.Organizer_id=us.id
                                  WHERE us.business_id=:businessId))
                          AND ev.Status IN ('InProgress',
                                            'Pending',
                                            'InEmergency')";

            var amount = NHibernateContext.CurrentSession.CreateSQLQuery(sql)
                .SetParameter("businessId", businessId)
                    .SetResultTransformer(Transformers.AliasToBean(typeof(EventStatusQuantityInfo))).UniqueResult<EventStatusQuantityInfo>();
              


            return amount;
        }

        public List<EventsAlongTheTimeQuantityInfo> EventsAlongTheTime(int? businessId)
        {
            var sql = @"SELECT YEAR(ev.BeginDateTime) AS [Year], 
                                MONTH(ev.BeginDateTime) AS [Month], 
                                COUNT(ev.id) AS [All],
                                SUM(CASE WHEN NOT ev.EmergencyDateTime IS NULL THEN 1 ELSE 0 END) AS InEmergency
                        FROM EVENTS ev 
                        WHERE (:businessId IS NULL
                               OR ev.id IN
                                 (SELECT ev.Id
                                  FROM EVENTS ev
                                  INNER JOIN users us ON ev.Organizer_id=us.id
                                  WHERE us.business_id=:businessId))
                          AND ev.Status IN ('Completed','InProgress',
                                            'Pending',
                                            'InEmergency')	
AND ev.BeginDateTime>=:from
                        GROUP BY YEAR(ev.BeginDateTime), month(ev.BeginDateTime)";


            var from = SystemDateTime.Now.AddMonths(-11).SetAtBeginDay();
            var list = NHibernateContext.CurrentSession.CreateSQLQuery(sql)
                .SetParameter("from",from)
                .SetParameter("businessId", businessId)
                .SetResultTransformer(Transformers.AliasToBean(typeof(EventsAlongTheTimeQuantityInfo))).List<EventsAlongTheTimeQuantityInfo>();



            return list.ToList();
        }
    }
}
