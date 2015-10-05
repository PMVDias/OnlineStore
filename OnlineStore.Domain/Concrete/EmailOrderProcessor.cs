using OnlineStore.Domain.Abstract;
using OnlineStore.Domain.Entities;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Domain.Concrete
{
    public class EmailSettings
    {
        public string MailToAddress = "orders@example.com";
        public string MailFromAddress = "sportsstore@example.com";
        public bool UseSsl = true;
        public string Username = "azure_12678e05de5ffda97282cc1510884a7c@azure.com";
        public string Password = "xWX2t6JP934zBse";
        public string ServerName = "smtp.sendgrid.net";
        public int ServerPort = 587;
        public bool WriteAsFile = true;
        public string FileLocation = @"c:\sports_store_emails";
    }
    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings emailSettings;
        public EmailOrderProcessor(EmailSettings settings)
        {
            emailSettings = settings;
        }
        public void ProcessOrder(Cart cart, ShippingDetails shippingInfo)
        {

            // Create the email object first, then add the properties.
            var myMessage = new SendGridMessage();

            // Add the message properties.
            myMessage.From = new MailAddress("onlineshp@onlineshp.com");

            // Add multiple addresses to the To field.
            List<String> recipients = new List<String>
            {
                shippingInfo.Email
            };

            myMessage.AddTo(recipients);

            myMessage.Subject = "New order submitted!";

            StringBuilder body = new StringBuilder()
               .AppendLine("<h3>A new order has been submitted<h3>")
               .AppendLine("<p>---</p>")
               .AppendLine("<p>Items:</p>");

            foreach (var line in cart.Lines)
            {
                var subtotal = line.Product.Price * line.Quantity;
                body.AppendFormat("<p>{0} x {1} (subtotal: {2:c}</p>", line.Quantity, line.Product.Name, subtotal);
            }

            body.AppendFormat("<p>Total order value: {0:c}</p>", cart.ComputeTotalValue())
            .AppendLine("<p>---</p>")
            .AppendLine("<p>Ship to:</p>")
            .AppendLine("<p>" + shippingInfo.Name + "</p>")
            .AppendLine("<p>" + shippingInfo.Line1 + "</p>")
            .AppendLine("<p>" + shippingInfo.Line2 ?? "" + "</p>")
            .AppendLine("<p>" + shippingInfo.Line3 ?? "" + "</p>")
            .AppendLine("<p>" + shippingInfo.City + "</p>")
            .AppendLine("<p>" + shippingInfo.State ?? "" + "</p>")
            .AppendLine("<p>" + shippingInfo.Country + "</p>")
            .AppendLine("<p>" + shippingInfo.Zip + "</p>")
            .AppendLine("<p>---</p>")
            .AppendFormat("<p>Gift wrap: {0}</p>", shippingInfo.GiftWrap ? "Yes" : "No");

            //Add the HTML and Text bodies
            myMessage.Html = body.ToString();

            // Create credentials, specifying your user name and password
            var credentials = new NetworkCredential(emailSettings.Username, emailSettings.Password);

            // Create an Web transport for sending email.
            var transportWeb = new Web(credentials);

            // Send the email.
            // You can also use the **DeliverAsync** method, which returns an awaitable task.
            transportWeb.DeliverAsync(myMessage);
        }
    }
}