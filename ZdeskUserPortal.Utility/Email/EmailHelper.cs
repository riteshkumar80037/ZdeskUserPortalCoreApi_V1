using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using ZdeskUserPortal.Domain.Model.Master;

namespace ZdeskUserPortal.Utility.Email
{
    public static class EmailHelper
    {
        public static SmtpEntity smtpEntity { get; set; }
        public const string DELIMITERS = ",;";

        public static bool IsEmailAddressValid(string emailAddress)
        {
            if (string.IsNullOrWhiteSpace(emailAddress))
                return false;

            var emails = emailAddress.Split(DELIMITERS.ToCharArray());
            foreach (var email in emails)
            {
                if (string.IsNullOrWhiteSpace(email) || email.Contains(" ") || !MailAddress.TryCreate(email, out MailAddress _))
                    return false;
            }
            return true;
        }

        public static void SendEmail(string fromAddress, string toAddresses, string ccAddresses, string bccAddresses, string subject, bool isBodyHtml, string body, MailPriority priority = MailPriority.Normal)
        {
            SendEmail(fromAddress, toAddresses, ccAddresses, bccAddresses, subject, isBodyHtml, body, null, null, priority);
        }

        public static void SendEmail(string fromAddress, string toAddresses, string ccAddresses, string bccAddresses, string subject, bool isBodyHtml, string body, IEnumerable<string> attachments, MailPriority priority = MailPriority.Normal)
        {
            SendEmail(fromAddress, toAddresses, ccAddresses, bccAddresses, subject, isBodyHtml, body, attachments, null, priority);
        }

        public static void SendEmail(string fromAddress, string toAddresses, string ccAddresses, string bccAddresses, string subject, bool isBodyHtml, string body, IEnumerable<string> attachments, Dictionary<string, string> images, MailPriority priority = MailPriority.Normal)
        {
            // create the mail message
            using (var mail = new MailMessage())
            {
                mail.Priority = priority;

                // set the subject and email addresses
                mail.From = new MailAddress(string.IsNullOrWhiteSpace(fromAddress) ? smtpEntity.FromAddress : fromAddress);
                mail.Subject = subject;

                if (!string.IsNullOrWhiteSpace(toAddresses))
                {
                    string[] addressArray = toAddresses.Split(DELIMITERS.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    foreach (string address in addressArray.Distinct())
                    {
                        if (!string.IsNullOrWhiteSpace(address))
                        {
                            mail.To.Add(new MailAddress(address));
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(ccAddresses))
                {
                    string[] addressArray = ccAddresses.Split(DELIMITERS.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    foreach (string address in addressArray.Distinct())
                    {
                        if (!string.IsNullOrWhiteSpace(address))
                        {
                            mail.CC.Add(new MailAddress(address));
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(bccAddresses))
                {
                    string[] addressArray = bccAddresses.Split(DELIMITERS.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    foreach (string address in addressArray.Distinct())
                    {
                        if (!string.IsNullOrWhiteSpace(address))
                        {
                            mail.Bcc.Add(new MailAddress(address));
                        }
                    }
                }

                // set the email body
                mail.IsBodyHtml = isBodyHtml;
                if (isBodyHtml && images is not null && images.Count > 0)
                {
                    var htmlView = AlternateView.CreateAlternateViewFromString(body, null, "text/html");

                    foreach (var image in images)
                    {
                        if (System.IO.File.Exists(image.Value))
                        {
                            var logo = new LinkedResource(image.Value);
                            logo.ContentId = image.Key;

                            htmlView.LinkedResources.Add(logo);
                        }
                    }

                    mail.AlternateViews.Add(htmlView);
                }
                else
                {
                    mail.Body = body;
                }

                // set the email attachments
                if (attachments is not null && attachments.Any())
                {
                    foreach (string attachment in attachments)
                    {
                        if (System.IO.File.Exists(attachment))
                        {
                            mail.Attachments.Add(new Attachment(attachment));
                        }
                    }
                }

                var sent = false;




                try
                {
                    using (var smtpClient = new SmtpClient())
                    {
                        smtpClient.Host = smtpEntity.ServiceProtocol;
                        smtpClient.Port = (int)smtpEntity.SmptPort;
                        smtpClient.Timeout = 20000;
                        smtpClient.Credentials = new NetworkCredential(smtpEntity.UserName, smtpEntity.Password);
                        smtpClient.EnableSsl = true;
                        smtpClient.Send(mail);

                        // email successfully sent, break out of the hosts loop.
                        sent = true;

                    }
                }
                catch (SmtpException)
                {
                    // exception occurred while sending the email to the host, swallow the exception and try the next host
                }


                if (!sent)
                {
                    throw new ApplicationException($"An error occurred while trying to send an email via smtp to '{toAddresses}'; Subject '{subject}'.");
                }

            }

        }
    }
}

