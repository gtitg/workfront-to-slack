using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workfront_to_slack.Workfront;
using workfront_to_slack.Slack;
using System.IO;
using CsvHelper;

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

            var workFrontUpdatesFile = ConfigurationManager.AppSettings["WorkFront_Update_File"];

            var slack_webhook_URL = ConfigurationManager.AppSettings["Slack_Webhook_URL"];
            var slack_channel = ConfigurationManager.AppSettings["Slack_Channel"];
            var slack_username = ConfigurationManager.AppSettings["Slack_Username"];
            var slack_user_icon_URL = ConfigurationManager.AppSettings["Slack_User_Icon_URL"];

            var csvUpdateList = new List<UpdateForCSV>();

            if (File.Exists(workFrontUpdatesFile))
            {
                using (TextReader reader = File.OpenText(workFrontUpdatesFile))
                {
                    var csv = new CsvReader(reader);
                    csvUpdateList = csv.GetRecords<UpdateForCSV>().ToList();
                }
            }

            Console.WriteLine("workfront username: " + workFront_Username);

            var workFrontClient = new WorkFrontClient(workFront_URL, workFront_Username, workFront_Password, workFront_Team_ID);
            var teamUsers = workFrontClient.getTeam();

            var slackClient = new SlackClient(slack_webhook_URL, slack_username, slack_channel, slack_user_icon_URL);

            foreach(var user in teamUsers)
            {
                var userUpdates = workFrontClient.getUserUpdates(user.ID);
                foreach(var update in userUpdates)
                {
                    //Console.WriteLine(update.ToString());
                    var alreadySentSearch = csvUpdateList.Where(u => u.updateObjCode.Equals(update.updateObjCode) && u.updateObjID.Equals(update.updateObjID)).FirstOrDefault();
                    if(alreadySentSearch == null)
                    {
                        // this update hasn't been sent yet, so we can send it and then add it to the csv list
                        //slackClient.sendMessage(update.enteredByName, update.ToString(), update.taskName(), update.projectName(), "");
                        csvUpdateList.Add(update.getCSVVersion());
                    }
                    else
                    {
                        Console.WriteLine("update already send to slack previously.");
                    }
                    
                }
            }

            using (TextWriter writer = File.CreateText(workFrontUpdatesFile))
            {
                var csv = new CsvWriter(writer);
                csv.WriteRecords(csvUpdateList);
            }
            //foreach(var u in updates)
            //{
            //    Console.WriteLine(u.message);
            //    Console.WriteLine();

            //    slackClient.sendMessage(u.enteredByName, u.message, "");
            //    break;
            //}

            

            workFrontClient.logout();
        }
    }
}
