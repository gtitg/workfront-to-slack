using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace workfront_to_slack.Workfront
{
    class WorkfrontUtils
    {
        public static string getUserLink(string baseURL, string userID)
        {
            if (String.IsNullOrWhiteSpace(baseURL) || String.IsNullOrWhiteSpace(userID))
            {
                return null;
            }
            return getWorkfrontItemLink(baseURL, "user", userID);
        }

        public static string getTaskLink(string baseURL, string taskID)
        {
            if (String.IsNullOrWhiteSpace(baseURL) || String.IsNullOrWhiteSpace(taskID))
            {
                return null;
            }
            return getWorkfrontItemLink(baseURL, "task", taskID);
        }

        private static string getWorkfrontItemLink(string workfrontBaseURL, string itemType, string itemID)
        {
            Uri baseUri = new Uri(workfrontBaseURL);
            Uri finalURI = new Uri(baseUri, itemType + "/view");

            var uriBuilder = new UriBuilder(finalURI);

            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["ID"] = itemID;
            uriBuilder.Query = parameters.ToString();

            var finalURL = uriBuilder.Uri.ToString();
            return finalURL;
        }
    }
}
