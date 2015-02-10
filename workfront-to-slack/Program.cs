using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workfront_to_slack.Workfront;
using workfront_to_slack.Slack;

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

            var slack_webhook_URL = ConfigurationManager.AppSettings["Slack_Webhook_URL"];
            var slack_channel = ConfigurationManager.AppSettings["Slack_Channel"];
            var slack_username = ConfigurationManager.AppSettings["Slack_Username"];
            var slack_user_icon_URL = ConfigurationManager.AppSettings["Slack_User_Icon_URL"];

            Console.WriteLine("workfront username: " + workFront_Username);

            var workFrontClient = new WorkFrontClient(workFront_URL, workFront_Username, workFront_Password, workFront_Team_ID);
            var updates = workFrontClient.getTeamUpdates();

            var slackClient = new SlackClient(slack_webhook_URL, slack_username, slack_channel, slack_user_icon_URL);

            foreach(var u in updates)
            {
                Console.WriteLine(u.message);
                Console.WriteLine();

                slackClient.sendMessage(u.message);
            }

            workFrontClient.logout();
        }
    }
}
