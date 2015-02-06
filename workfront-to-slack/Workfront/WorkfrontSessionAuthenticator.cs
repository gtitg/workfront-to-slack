using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workfront_to_slack.Workfront
{
    class WorkfrontSessionAuthenticator : IAuthenticator
    {
        private readonly string _sessionID;

        public WorkfrontSessionAuthenticator(string sessionID)
        {
            _sessionID = sessionID;
        }

        public void Authenticate(IRestClient client, IRestRequest request)
        {
            if (!request.Parameters.Any(p => p.Name.Equals("SessionID") && p.Type == ParameterType.HttpHeader))
            {
                request.AddHeader("SessionID", _sessionID);
            }
        }
    }
}
