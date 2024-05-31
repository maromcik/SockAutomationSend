using System.Net;
using System.Net.Mail;
using StockAutomationCore.EmailService;

namespace SockAutomationSend.EmailService;

public static class EmailController
{
    private static List<Subscription> Subscriptions { get; } =
    [
        new Subscription
        {
            EmailAddress = "roman.mariancik@gmail.com"
        },
        new Subscription
        {
            EmailAddress = "gesvindr@mail.muni.cz"
        }
    ];

    public static void AddSubscriber(string address)
    {
        var newSub = new Subscription { EmailAddress = address };
        Subscriptions.Add(newSub);
    }

    public static void RemoveSubscribers(IEnumerable<Subscription> subscriptions)
    {
        foreach (var sub in subscriptions)
        {
            Subscriptions.Remove(sub);
        }
    }

    public static void SendEmail(string body)
    {
        var host = "smtp.gmail.com";
        var port = 587;
        var username = "stock.advisory.company@gmail.com";
        var password = Environment.GetEnvironmentVariable("STOCKAUTOMATION_EMAIL_PASSWORD");
        var from = "stock.advisory.company@gmail.com";

        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentNullException();
        }

        var smtpClient = new SmtpClient(host)
        {
            Port = port,
            Credentials = new NetworkCredential(username, password),
            EnableSsl = true,
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(from),
            Subject = "Update in holdings is here!",
            Body = CreateEmailBody(body),
            IsBodyHtml = true,
        };

        foreach (var subscription in Subscriptions)
        {
            mailMessage.Bcc.Add(subscription.EmailAddress);
        }

        smtpClient.Send(mailMessage);
    }


    private static string CreateEmailBody(string diff)
    {
        var body = $@"
                    <html>
                    <head>
                        <style>
                            body {{
                                font-family: 'Arial', sans-serif;
                                color: #333;
                                margin: 20px;
                                padding: 0;
                            }}
                            .diff {{
                                white-space: pre;
                            }}
                            .header {{
                                color: #fff;
                                background-color: #2B9ED1;
                                padding: 10px;
                                text-align: center;
                            }}
                            .content {{
                                margin-top: 20px;
                            }}
                            p {{
                                line-height: 1.5;
                            }}
                            .footer {{
                                margin-top: 20px;
                                padding: 10px;
                                background-color: #f0f0f0;
                                text-align: center;
                                font-size: 0.8em;
                            }}
                        </style>
                    </head>
                    <body>
                        <div class='header'>
                            <h1>New Stock Changes in Our Holdings!</h1>
                        </div>
                        <div class='content'>
                            <p>Hello,</p>
                            <p>There are new stock changes in our holdings. Please see below for details:</p>
                            <p class='diff'>{diff}</p>
                            <p>Best regards,</p>
                            <p>Your Quality Soldiers</p>
                        </div>
                        <div class='footer'>
                            Date: {DateTime.Now.ToShortDateString()}
                        </div>
                    </body>
                    </html>
                    ";
        return body;
    }
}
