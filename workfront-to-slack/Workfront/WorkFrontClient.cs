using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workfront_to_slack.Workfront
{
    class WorkFrontClient
    {
        public string workfrontURL { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string teamID { get; set; }

        private RestClient client { get; set; }
        private string _sessionID { get; set; }

        public bool hasActiveSession
        {
            get
            {
                return !String.IsNullOrWhiteSpace(_sessionID);
            }
        }

        public WorkFrontClient(string url, string username, string password, string teamID)
        {
            this.workfrontURL = url;
            this.username = username;
            this.password = password;
            this.teamID = teamID;

            client = new RestClient(this.workfrontURL);
            //client.AddHandler("application/json", new WorkfrontDeserializer());
        }

        public bool connect()
        {
            try
            {
                var request = new RestRequest("login", Method.GET);
                request.AddParameter("username", this.username);
                request.AddParameter("password", this.password);

                // execute the request
                var connectResponse = client.Execute<WorkfrontLoginSession>(request);

                if(connectResponse != null 
                    && connectResponse.StatusCode.Equals(System.Net.HttpStatusCode.OK)
                    && connectResponse.Data != null 
                    && connectResponse.Data.data != null 
                    && !String.IsNullOrWhiteSpace(connectResponse.Data.data.sessionID))
                {
                    Console.WriteLine("Started new workfront session successfully");
                    _sessionID = connectResponse.Data.data.sessionID;
                    client.Authenticator = new WorkfrontSessionAuthenticator(connectResponse.Data.data.sessionID);
                }
                else
                {
                    Console.WriteLine("Failed to authenticate new workfront session.");
                    return false;
                }
                
                return true;
                
            }
            catch(Exception e)
            {
                Console.WriteLine("Failed to authenticate new workfront session with exception: " + e.ToString());
                return false;
            }
        }

        public void logout()
        {
            try
            {
                client.Execute(new RestRequest("logout", Method.GET));
                _sessionID = null;
                client.Authenticator = null;
            }
            catch(Exception e)
            {
                Console.WriteLine("Error logging out: " + e.ToString());
            }
        }

        public List<Update> getTeamUpdates()
        {
            if(!hasActiveSession)
            {
                Console.WriteLine("Client not authenticated.  Logging in now.");
                if(!connect())
                {
                    return null;
                }
            }

            var updateRequest = new RestRequest("team/search", Method.GET);
            updateRequest.AddParameter("id", this.teamID);
            updateRequest.AddParameter("fields", "updates, updates:enteredByName, updates:iconName, updates:iconPath, updates:entryDate");

            var updateResponse = client.Execute<UpdateResponse>(updateRequest);
            Console.WriteLine("response status: " + updateResponse.ResponseStatus + " http status code: " + updateResponse.StatusCode + ", " + updateResponse.StatusDescription);

            if (updateResponse.Data != null
                && updateResponse.Data.data != null)
            {
                var updateCount = updateResponse.Data.data.Count();
                if (updateCount == 1)
                {
                    return updateResponse.Data.data.First().updates;
                }
                else if (updateCount == 0)
                {
                    Console.WriteLine("Team not found in workfront. Check to make sure your Team ID setting is correct.");
                }
                else if (updateCount > 1)
                {
                    // this should never happen
                    Console.WriteLine("More than one team result from workfront with the same id.  Stopping here.  Unknown behvior lies ahead.");
                }
            }

            return null;
        }
    }
}
