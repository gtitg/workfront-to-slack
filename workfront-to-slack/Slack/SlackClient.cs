using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workfront_to_slack.Slack
{
    class SlackClient
    {
        public string webhookURL { get; set; }
        public string channel { get; set; }
        public string username { get; set; }
        public string iconURL { get; set; }

        private RestClient client { get; set; }

        public SlackClient(string url, string username, string channel, string iconURL)
        {
            this.webhookURL = url;
            this.username = username;
            this.channel = channel;
            this.iconURL = iconURL;

            client = new RestClient(this.webhookURL);
            //client.AddHandler("application/json", new WorkfrontDeserializer());
        }

        public void sendMessage(string name, string message, string taskName, string projectName, string userLink, string taskLink)
        {
            var request = new RestRequest("", Method.POST);
            request.RequestFormat = DataFormat.Json;

            var slackPayload = new SlackMessage();

            var updateAttachment = new SlackAttachment();
            updateAttachment.fields = new List<SlackAttachmentField>();

            var projectField = new SlackAttachmentField { title = "Project", value = projectName, Short = false };
            updateAttachment.fields.Add(projectField);

            updateAttachment.text = message;
            updateAttachment.fallback = name + ": " + message;
            updateAttachment.title = taskName;
            updateAttachment.author_name = name;

            if(!String.IsNullOrWhiteSpace(taskLink))
            {
                updateAttachment.title_link = taskLink;
            }

            if (!String.IsNullOrWhiteSpace(userLink))
            {
                updateAttachment.author_link = userLink;
            }

            slackPayload.attachments = new List<SlackAttachment>();
            slackPayload.attachments.Add(updateAttachment);

            
            slackPayload.username = this.username;
            slackPayload.icon_url = this.iconURL;
            slackPayload.channel = this.channel;
            //slackPayload.mrkdwn = false;
            request.AddBody(slackPayload);

            var response = client.Execute(request);
        }
    }
}
