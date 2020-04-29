using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Epay3.Api.Tenancy;
using RestSharp;

namespace Epay3.Api.Services
{
    public class MessagingService
    {
        private readonly AppTenant appTenant;

        public MessagingService()
        {
            
        }

        public void sendSms(string phoneNumber, string smsBody,string tenant)
        {
                smsWithAmazonSNS(phoneNumber, smsBody);
        }

       
        private static void smsWithAmazonSNS(string phoneNumber, string smsBody)
        {
            AmazonSimpleNotificationServiceClient snsClient = new AmazonSimpleNotificationServiceClient
            (new BasicAWSCredentials("AMAZON_KEY", "AMAZON_TOKEN"), RegionEndpoint.APEast1);

            PublishRequest pubRequest = new PublishRequest();
            pubRequest.Message = smsBody;
            pubRequest.PhoneNumber = phoneNumber;
            var publishAsync = snsClient.PublishAsync(pubRequest);
            publishAsync.Wait();
            var publishAsyncResult = publishAsync.Result;
        }
    }
}
