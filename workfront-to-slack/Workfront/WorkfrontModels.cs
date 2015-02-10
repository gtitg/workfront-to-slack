using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workfront_to_slack.Workfront
{
    class WorkfrontModels
    {
    }

    public class LoginData
    {
        public string sessionID { get; set; }
    }

    public class WorkfrontLoginSession
    {
        public LoginData data { get; set; }
    }

    public class UpdateResponse
    {
        public List<UpdateData> data { get; set; }
    }

    public class UpdateData
    {
        // team name
        public string name { get; set; }
        public List<Update> updates { get; set; }
    }

    public class Update
    {
        public string message { get; set; }
        public string objCode { get; set; }
        public string updateObjCode { get; set; }
        public string updateObjID { get; set; }
        public string enteredByName { get; set; }
        public string iconName { get; set; }
        public string iconPath { get; set; }

        public string entryDate { get; set; }
    }
}
