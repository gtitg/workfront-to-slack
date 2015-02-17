using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workfront_to_slack.Slack
{
    class SlackModels
    {
    }

    public class SlackAttachmentField
    {
        public string title { get; set; }
        public string value { get; set; }
        public bool Short { get; set; }
    }

    public class SlackAttachment
    {
        public string text { get; set; }
        public string fallback { get; set; }

        public string author_name { get; set; }
        public string author_link { get; set; }

        public string title { get; set; }
        public string title_link { get; set; }

        public List<SlackAttachmentField> fields { get; set; }
    }

    public class SlackMessage
    {
        

        public string icon_url { get; set; }
        public string username { get; set; }
        //public bool mrkdwn { get; set; }

        

        public List<SlackAttachment> attachments { get; set; }
    }
}
