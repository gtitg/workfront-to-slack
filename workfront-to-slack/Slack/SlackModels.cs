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

    public class SlackMessage
    {
        public string text { get; set; }
        public string icon_url { get; set; }
        public string username { get; set; }
        public bool mrkdwn { get; set; }
    }
}
