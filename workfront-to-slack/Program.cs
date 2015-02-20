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
            var workFront_BASE_URL = ConfigurationManager.AppSettings["WorkFront_BASE_URL"];
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
            var updatesEncounteredInThisSession = new List<UpdateForCSV>();

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
                var userLink = WorkfrontUtils.getUserLink(workFront_BASE_URL, user.ID);
                foreach(var update in userUpdates)
                {
                    //Console.WriteLine(update.ToString());
                    updatesEncounteredInThisSession.Add(update.getCSVVersion());
                    if (update.EntryDate.Date == DateTime.Now.Date)
                    {
                        // only attempt the check and send the update if it is from today
                        var alreadySentSearch = csvUpdateList.Where(u => u.updateObjCode.Equals(update.updateObjCode) && u.updateObjID.Equals(update.updateObjID)).FirstOrDefault();
                        if (alreadySentSearch == null)
                        {
                            // this update hasn't been sent yet, so we can send it and then add it to the csv list
                            var taskLink = WorkfrontUtils.getTaskLink(workFront_BASE_URL, update.taskID());
                            slackClient.sendMessage(update.enteredByName, update.ToString(), update.taskName(), update.projectName(), userLink, taskLink);
                            csvUpdateList.Add(update.getCSVVersion());
                        }
                        else
                        {
                            Console.WriteLine("update already send to slack previously.");
                        }
                    }
                }
            }

            var updatesToRemoveFromFile = new List<UpdateForCSV>();
            for (int i = 0; i < csvUpdateList.Count(); i++ )
            {
                // loop through the csv file updates, and see if there are records in the file which we did not
                // encounter in this run of the program.  If that is the case, we can prune them from the file
                // to prevent the file from getting too large.

                var fileUpdate = csvUpdateList[i];
                var searchResult = updatesEncounteredInThisSession.Where(u => u.updateObjCode.Equals(fileUpdate.updateObjCode) && u.updateObjID.Equals(fileUpdate.updateObjID)).FirstOrDefault();
                if(searchResult == null)
                {
                    // we didn't see this update in this run, so we should prune it from the file
                    updatesToRemoveFromFile.Add(fileUpdate);
                }
            }

            foreach(var fileUpdate in updatesToRemoveFromFile)
            {
                csvUpdateList.Remove(fileUpdate);
            }

                using (TextWriter writer = File.CreateText(workFrontUpdatesFile))
                {
                    var csv = new CsvWriter(writer);
                    csv.WriteRecords(csvUpdateList);
                }

            workFrontClient.logout();
        }
    }
}
