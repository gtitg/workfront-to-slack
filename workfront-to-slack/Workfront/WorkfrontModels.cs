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

    public class TeamResponse
    {
        public List<Team> data { get; set; }
    }

    public class UserResponse
    {
        public User data { get; set; }
    }

    public class Team
    {
        // team name
        public string name { get; set; }
        public List<User> users { get; set; }
    }

    public class User
    {
        public string ID { get; set; }
        public string avatarDownloadURL { get; set; }
        public string emailAddr { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string name { get; set; }

        public List<Update> updates { get; set; }
    }

    public class MessageArg
    {
        public string ID { get; set; }
        public string text { get; set; }
        public string type { get; set; }
    }

    public class UpdateForCSV
    {
        public string updateObjCode { get; set; }
        public string updateObjID { get; set; }
    }

    public class Update
    {
        public string message { get; set; }
        public List<MessageArg> messageArgs { get; set; }
        public string objCode { get; set; }
        public string updateObjCode { get; set; }
        public string updateObjID { get; set; }
        public string enteredByName { get; set; }
        public string iconName { get; set; }
        public string iconPath { get; set; }

        public string entryDate { get; set; }

        public Note updateNote { get; set; }
        public JournalEntry updateJournalEntry { get; set; }
        public List<Update> nestedUpdates { get; set; }

        public UpdateForCSV getCSVVersion()
        {
            var newCSVUpdate = new UpdateForCSV() { updateObjID = this.updateObjID, updateObjCode = this.updateObjCode };
            return newCSVUpdate;
        }

        public override string ToString()
        {
            if(messageArgs != null && messageArgs.Count > 0)
            {
                // format the string using the messageArgs
                List<string> stringFormatArgs = new List<string>();
                foreach(var arg in messageArgs)
                {
                    stringFormatArgs.Add(arg.text);
                }
                return String.Format(message, stringFormatArgs.ToArray());
            }
            else
            {
                // just send back the regular message text
                return message;
            }
        }

        public string projectName()
        {
            if(updateNote != null && updateNote.project != null)
            {
                return updateNote.project.name;
            }
            else if(updateJournalEntry != null && updateJournalEntry.project != null)
            {
                return updateJournalEntry.project.name;
            }
            else
            {
                return "None";
            }
        }

        public string taskName()
        {
            if (updateNote != null && updateNote.task != null)
            {
                return updateNote.task.name;
            }
            else if (updateJournalEntry != null && updateJournalEntry.task != null)
            {
                return updateJournalEntry.task.name;
            }
            else
            {
                return "None";
            }
        }

        public string taskID()
        {
            if (updateNote != null && updateNote.task != null)
            {
                return updateNote.task.ID;
            }
            else if (updateJournalEntry != null && updateJournalEntry.task != null)
            {
                return updateJournalEntry.task.ID;
            }
            else
            {
                return null;
            }
        }
    }

    public class Project
    {
        public string ID { get; set; }
        public string name { get; set; }
        public string objCode { get; set; }
    }

    public class Task
    {
        public string ID { get; set; }
        public string name { get; set; }
        public string objCode { get; set; } 
    }

    public class Note
    {
        public string ID { get; set; }
        public string objCode { get; set; }
        public Project project { get; set; }
        public Task task { get; set; }
    }

    public class JournalEntry
    {
        public string ID { get; set; }
        public string objCode { get; set; }
        public Project project { get; set; }
        public Task task { get; set; }
    }


}
