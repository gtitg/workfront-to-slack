using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workfront_to_slack.Workfront;

namespace workfront_to_slack
{
    class Program
    {
        static void Main(string[] args)
        {
            var workFront_URL = ConfigurationManager.AppSettings["WorkFront_URL"];
            var workFront_Username = ConfigurationManager.AppSettings["WorkFront_Username"];
            var workFront_Password = ConfigurationManager.AppSettings["WorkFront_Password"];
            var workFront_Team_ID = ConfigurationManager.AppSettings["WorkFront_Team_ID"];


            Console.WriteLine("workfront username: " + workFront_Username);

            var workFrontClient = new WorkFrontClient(workFront_URL, workFront_Username, workFront_Password, workFront_Team_ID);
            var updates = workFrontClient.getTeamUpdates();

            foreach(var u in updates)
            {
                Console.WriteLine(u.message);
                Console.WriteLine();
            }

            workFrontClient.logout();
        }
    }
}
