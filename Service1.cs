﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TicketEmailSender.Models;

namespace TicketEmailSender
{
    public partial class Service1 : ServiceBase
    {
        delegate void EmailToRead(EmailConfiguration emailConfiguration);
        public Service1()
        {
            InitializeComponent();
        }

        public void Start()
        {
            OnStart(new string[0]);
        }

        EmailConfiguration emailConfiguration = null;
        protected override void OnStart(string[] args)
        {


            RunAsync();
        }

        protected override void OnStop()
        {
        }

        async Task RunAsync()
        {

            Console.WriteLine("Calling the back-end API");

            string apiBaseAddress = "http://localhost:54780/";

            CustomDelegatingHandler customDelegatingHandler = new CustomDelegatingHandler();

            HttpClient client = HttpClientFactory.Create(customDelegatingHandler);

            HttpResponseMessage response = await client.GetAsync(apiBaseAddress + "api/Configuration");

            if (response.IsSuccessStatusCode)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseString);
                Console.WriteLine("HTTP Status: {0}, Reason {1}. Press ENTER to exit", response.StatusCode, response.ReasonPhrase);


                List<EmailConfiguration> emailConfigurations = JsonConvert.DeserializeObject<List<EmailConfiguration>>(responseString);

                PrepareEmailsToRead(emailConfigurations);
            }
            else
            {
                Console.WriteLine("Failed to call the API. HTTP Status: {0}, Reason {1}", response.StatusCode, response.ReasonPhrase);
            }



            //Console.ReadLine();
        }

         void PrepareEmailsToRead(List<EmailConfiguration> emailConfigurations)
        {
            EmailReader emailReader = new EmailReader();

            foreach (var email in emailConfigurations)
            {
                EmailToRead emailToRead;
                emailToRead = emailReader.readEmail;
                emailToRead(email);
            }
        }
    }
  


        public class CustomDelegatingHandler : DelegatingHandler
        {
            //Obtained from the server earlier, APIKey MUST be stored securly and in App.Config
            private string APPId = "4d53bce03ec34c0a911182d4c228ee6c";
            private string APIKey = "A93reRTUJHsCuQSHR+L3GxqOJyDmQpCgps102ciuabc=";

            protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {

                HttpResponseMessage response = null;
                string requestContentBase64String = string.Empty;

                string requestUri = System.Web.HttpUtility.UrlEncode(request.RequestUri.AbsoluteUri.ToLower());

                string requestHttpMethod = request.Method.Method;

                //Calculate UNIX time
                DateTime epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
                TimeSpan timeSpan = DateTime.UtcNow - epochStart;
                string requestTimeStamp = Convert.ToUInt64(timeSpan.TotalSeconds).ToString();

                //create random nonce for each request
                string nonce = Guid.NewGuid().ToString("N");

                //Checking if the request contains body, usually will be null wiht HTTP GET and DELETE
                if (request.Content != null)
                {
                    byte[] content = await request.Content.ReadAsByteArrayAsync();
                    MD5 md5 = MD5.Create();
                    //Hashing the request body, any change in request body will result in different hash, we'll incure message integrity
                    byte[] requestContentHash = md5.ComputeHash(content);
                    requestContentBase64String = Convert.ToBase64String(requestContentHash);
                }

                //Creating the raw signature string
                string signatureRawData = String.Format("{0}{1}{2}{3}{4}{5}", APPId, requestHttpMethod, requestUri, requestTimeStamp, nonce, requestContentBase64String);

                var secretKeyByteArray = Convert.FromBase64String(APIKey);

                byte[] signature = Encoding.UTF8.GetBytes(signatureRawData);

                using (HMACSHA256 hmac = new HMACSHA256(secretKeyByteArray))
                {
                    byte[] signatureBytes = hmac.ComputeHash(signature);
                    string requestSignatureBase64String = Convert.ToBase64String(signatureBytes);
                    //Setting the values in the Authorization header using custom scheme (amx)
                    request.Headers.Authorization = new AuthenticationHeaderValue("amx", string.Format("{0}:{1}:{2}:{3}", APPId, requestSignatureBase64String, nonce, requestTimeStamp));
                }

                response = await base.SendAsync(request, cancellationToken);

                return response;
        }

        private void GenerateAPPKey()
        {
            using (var cryptoProvider = new RNGCryptoServiceProvider())
            {
                byte[] secretKeyByteArray = new byte[32]; //256 bit
                cryptoProvider.GetBytes(secretKeyByteArray);
                var APIKey = Convert.ToBase64String(secretKeyByteArray);
            }
        }
    }
}
