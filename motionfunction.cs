using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;

namespace motion.function
{
    public static class motionfunction
    {
        private static HttpClient client = new HttpClient();

        [FunctionName("motionfunction")]
        public static void Run([IoTHubTrigger("messages/events", Connection = "IoTHubConnectionString")]EventData message, ILogger log)
        {
            log.LogInformation($"C# IoT Hub trigger function processed a message: {Encoding.UTF8.GetString(message.Body.Array)}");
            sendEmail().Wait();
            log.LogInformation("Email sent");
        }

        static async Task sendEmail()
        {
        var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress("motion@cloudadventures.org", "Chewbacca");
        var subject = "Motion Detected";
        var to = new EmailAddress("whaakman@live.com", "Wesley Haakman");
        var plainTextContent = "uughghhhgh huuguughghg huuguughghg";
        var htmlContent = "<strong>uughghhhgh huuguughghg huuguughghg</strong>";
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
        Console.WriteLine("Email sent");
        }
    }
}