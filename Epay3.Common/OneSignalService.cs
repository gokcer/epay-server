using System;
using System.Net;
using RestSharp;

namespace Epay3.Common
{
    public class OneSignalService
    {
        public string SendToClient(string client, string header, string message)
        {

            RestClient rclient = new RestClient("https://onesignal.com/api/v1/");

            RestRequest restRequest = new RestRequest("notifications", Method.POST);

            var key = "DEPLOY_ONE_SIGNAL_KEY";
            var appId = "DEPLOY_ONE_SIGNAL_APP_ID";

            restRequest.AddHeader("Authorization", string.Format("Basic {0}", key));
            restRequest.AddHeader("Content-Type", "application/json");

            string body =
                $"{{\"app_id\":\"{appId}\",\"contents\":{{\"en\":\"{message}\"}},\"headings\":{{\"en\":\"{header}\"}},\"data\":null,\"filters\":[{{\"field\":\"tag\",\"key\":\"notificationToken\",\"relation\":\"=\",\"value\":\"{client}\"}}]}}";

            restRequest.AddParameter("application/json", body, ParameterType.RequestBody);

            IRestResponse restResponse = rclient.Execute(restRequest);

            if (restResponse.StatusCode == HttpStatusCode.Created && restResponse.StatusCode == HttpStatusCode.OK)
            {
                if (restResponse.ErrorException != null)
                    throw restResponse.ErrorException;
                if (restResponse.StatusCode != HttpStatusCode.OK && restResponse.Content != null)
                    throw new Exception(restResponse.Content);
            }

            var notificationCreateResult = restResponse.Content;

            return notificationCreateResult;

        }
    }
}
