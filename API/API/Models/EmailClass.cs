using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.RegularExpressions;

namespace API.Models
{


    /// <summary>
    /// Summary description for EmailMessage
    /// </summary>
    public class EmailClass
    {
        public string SenderEmailAddress { get; set; }
        public string SenderPassword { get; set; }
        public string SenderHostName { get; set; }
        public int SenderPort { get; set; }
        public bool EnableSSL { get; set; }
        private Regex regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");

        /// <summary>
        /// Default constructor
        /// </summary>
        public EmailClass()
        {

        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="senderEmailAddress"></param>
        /// <param name="senderPassword"></param>
        /// <param name="senderHostName"></param>
        /// <param name="senderPort"></param>
        public EmailClass(string senderEmailAddress, string senderPassword, string senderHostName, int senderPort, bool enableSSL)
        {
            try
            {
                this.SenderEmailAddress = senderEmailAddress;
                this.SenderPassword = senderPassword;
                this.SenderHostName = senderHostName;
                this.SenderPort = senderPort;
                this.EnableSSL = enableSSL;
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Validate email address
        /// </summary>
        /// <param name="EmailAddress"></param>
        /// <returns></returns>
        public bool IsValidEmailAddress(string EmailAddress)
        {
            try
            {
                return regex.IsMatch(EmailAddress) && !EmailAddress.EndsWith(".");
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        ///  Method validate multiple email addresses and returns invalid email addresses
        /// </summary>
        /// <param name="ListEmailAddresses"></param>
        /// <returns></returns>
        public List<string> IsValidEmailAddress(List<string> ListEmailAddresses)
        {
            try
            {
                List<string> ListEmail = new List<string>();
                foreach (string email in ListEmailAddresses)
                {
                    if (!(regex.IsMatch(email) && !email.EndsWith(".")))
                    {
                        ListEmail.Add(email);
                    }
                }
                return ListEmail;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Send email message to single recepient with some attachements
        /// </summary>
        /// <param name="Subject"></param>
        /// <param name="Body"></param>
        /// <param name="EmailTO"></param>
        /// <param name="Priority"></param>
        /// <param name="AttchementPath"></param>
        /// <returns></returns>
        public bool SendEmail(string Subject, string Body, string EmailTO, MailPriority Priority = MailPriority.Normal, List<string> AttchementPath = null)
        {
            try
            {
                List<string> ListEmailTO = new List<string>();
                ListEmailTO.Add(EmailTO);
                return SendEmailMessage(Subject, Body, ListEmailTO, null, null, Priority, AttchementPath);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Send email message to single recepient with some images in message
        /// </summary>
        /// <param name="Subject"></param>
        /// <param name="Body"></param>
        /// <param name="EmailTO"></param>
        /// <param name="Priority"></param>
        /// <param name="LinkedResources"></param>
        /// <returns></returns>
        public bool SendEmail(string Subject, string Body, string EmailTO, MailPriority Priority = MailPriority.Normal, string LinkedResources = null)
        {
            try
            {
                List<string> ListEmailTO = new List<string>();
                ListEmailTO.Add(EmailTO);
                return SendEmailMessage(Subject, Body, ListEmailTO, null, null, Priority, null, LinkedResources);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Send email message to single recepient with some attachements and images in message
        /// </summary>
        /// <param name="Subject"></param>
        /// <param name="Body"></param>
        /// <param name="EmailTO"></param>
        /// <param name="AttchementPath"></param>
        /// <param name="LinkedResources"></param>
        /// <param name="Priority"></param>
        /// <returns></returns>
        public bool SendEmail(string Subject, string Body, string EmailTO, MailPriority Priority = MailPriority.Normal, List<string> AttchementPath = null, string LinkedResources = null)
        {
            try
            {
                List<string> ListEmailTO = new List<string>();
                ListEmailTO.Add(EmailTO);
                return SendEmailMessage(Subject, Body, ListEmailTO, null, null, Priority, AttchementPath, LinkedResources);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Send email message to single recepient with CC/BCC options and some attachements
        /// </summary>
        /// <param name="Subject"></param>
        /// <param name="Body"></param>
        /// <param name="EmailTO"></param>
        /// <param name="EmailCC"></param>
        /// <param name="EmailBCC"></param>
        /// <param name="Priority"></param>
        /// <param name="AttchementPath"></param>
        /// <returns></returns>
        public bool SendEmail(string Subject, string Body, string EmailTO, string EmailCC = null, string EmailBCC = null, MailPriority Priority = MailPriority.Normal, List<string> AttchementPath = null)
        {
            try
            {
                List<string> ListEmailTO = new List<string>();
                List<string> ListEmailCC = null;
                List<string> ListEmailBCC = null;

                ListEmailTO.Add(EmailTO);
                if (!string.IsNullOrEmpty(EmailCC))
                {
                    ListEmailCC = new List<string>();
                    ListEmailCC.Add(EmailCC);
                }
                if (!string.IsNullOrEmpty(EmailBCC))
                {
                    ListEmailBCC = new List<string>();
                    ListEmailBCC.Add(EmailBCC);
                }
                return SendEmailMessage(Subject, Body, ListEmailTO, ListEmailCC, ListEmailBCC, Priority, AttchementPath);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Send email message to single recepient with CC/BCC options and some images in message
        /// </summary>
        /// <param name="Subject"></param>
        /// <param name="Body"></param>
        /// <param name="EmailTO"></param>
        /// <param name="EmailCC"></param>
        /// <param name="EmailBCC"></param>
        /// <param name="Priority"></param>
        /// <param name="LinkedResources"></param>
        /// <returns></returns>
        public bool SendEmail(string Subject, string Body, string EmailTO, string EmailCC = null, string EmailBCC = null, MailPriority Priority = MailPriority.Normal, string LinkedResources = null)
        {
            try
            {
                List<string> ListEmailTO = new List<string>();
                List<string> ListEmailCC = null;
                List<string> ListEmailBCC = null;

                ListEmailTO.Add(EmailTO);
                if (!string.IsNullOrEmpty(EmailCC))
                {
                    ListEmailCC = new List<string>();
                    ListEmailCC.Add(EmailCC);
                }
                if (!string.IsNullOrEmpty(EmailBCC))
                {
                    ListEmailBCC = new List<string>();
                    ListEmailBCC.Add(EmailBCC);
                }
                return SendEmailMessage(Subject, Body, ListEmailTO, ListEmailCC, ListEmailBCC, Priority, null, LinkedResources);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Send email message to single recepient with CC/BCC options and some attachements and images in message
        /// </summary>
        /// <param name="Subject"></param>
        /// <param name="Body"></param>
        /// <param name="EmailTO"></param>
        /// <param name="EmailCC"></param>
        /// <param name="EmailBCC"></param>
        /// <param name="Priority"></param>
        /// <param name="AttchementPath"></param>
        /// <param name="LinkedResources"></param>
        /// <returns></returns>
        public bool SendEmail(string Subject, string Body, string EmailTO, string EmailCC = null, string EmailBCC = null, MailPriority Priority = MailPriority.Normal, List<string> AttchementPath = null, string LinkedResources = null)
        {
            try
            {
                List<string> ListEmailTO = new List<string>();
                List<string> ListEmailCC = null;
                List<string> ListEmailBCC = null;

                ListEmailTO.Add(EmailTO);
                if (!string.IsNullOrEmpty(EmailCC))
                {
                    ListEmailCC = new List<string>();
                    ListEmailCC.Add(EmailCC);
                }
                if (!string.IsNullOrEmpty(EmailBCC))
                {
                    ListEmailBCC = new List<string>();
                    ListEmailBCC.Add(EmailBCC);
                }
                return SendEmailMessage(Subject, Body, ListEmailTO, ListEmailCC, ListEmailBCC, Priority, AttchementPath, LinkedResources);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Send email message to multiple recepients with some attachements
        /// </summary>
        /// <param name="Subject"></param>
        /// <param name="Body"></param>
        /// <param name="ListEmailTO"></param>
        /// <param name="Priority"></param>
        /// <param name="AttchementPath"></param>
        /// <returns></returns>
        public bool SendEmail(string Subject, string Body, List<string> ListEmailTO, MailPriority Priority = MailPriority.Normal, List<string> AttchementPath = null)
        {
            try
            {
                return SendEmailMessage(Subject, Body, ListEmailTO, null, null, Priority, AttchementPath);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Send email message to multiple recepients with some images in message
        /// </summary>
        /// <param name="Subject"></param>
        /// <param name="Body"></param>
        /// <param name="ListEmailTO"></param>
        /// <param name="Priority"></param>
        /// <param name="LinkedResources"></param>
        /// <returns></returns>
        public bool SendEmail(string Subject, string Body, List<string> ListEmailTO, MailPriority Priority = MailPriority.Normal, string LinkedResources = null)
        {
            try
            {
                return SendEmailMessage(Subject, Body, ListEmailTO, null, null, Priority, null, LinkedResources);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Send email message to multiple recepients with some attachements and images in message
        /// </summary>
        /// <param name="Subject"></param>
        /// <param name="Body"></param>
        /// <param name="ListEmailTO"></param>
        /// <param name="Priority"></param>
        /// <param name="AttchementPath"></param>
        /// <param name="LinkedResources"></param>
        /// <returns></returns>
        public bool SendEmail(string Subject, string Body, List<string> ListEmailTO, MailPriority Priority = MailPriority.Normal, List<string> AttchementPath = null, string LinkedResources = null)
        {
            try
            {
                return SendEmailMessage(Subject, Body, ListEmailTO, null, null, Priority, AttchementPath, LinkedResources);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Send email message to multiple recepients with multiple CC/BCC options with some attachements
        /// </summary>
        /// <param name="Subject"></param>
        /// <param name="Body"></param>
        /// <param name="ListEmailTO"></param>
        /// <param name="ListEmailCC"></param>
        /// <param name="ListEmailBCC"></param>
        /// <param name="Priority"></param>
        /// <param name="AttchementPath"></param>
        /// <returns></returns>
        public bool SendEmail(string Subject, string Body, List<string> ListEmailTO, List<string> ListEmailCC = null, List<string> ListEmailBCC = null, MailPriority Priority = MailPriority.Normal, List<string> AttchementPath = null)
        {
            try
            {
                return SendEmailMessage(Subject, Body, ListEmailTO, ListEmailCC, ListEmailBCC, Priority, AttchementPath);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Send email message to multiple recepients with multiple CC/BCC options with some images in message
        /// </summary>
        /// <param name="Subject"></param>
        /// <param name="Body"></param>
        /// <param name="ListEmailTO"></param>
        /// <param name="ListEmailCC"></param>
        /// <param name="ListEmailBCC"></param>
        /// <param name="Priority"></param>
        /// <param name="LinkedResources"></param>
        /// <returns></returns>
        public bool SendEmail(string Subject, string Body, List<string> ListEmailTO, List<string> ListEmailCC = null, List<string> ListEmailBCC = null, MailPriority Priority = MailPriority.Normal, string LinkedResources = null)
        {
            try
            {
                return SendEmailMessage(Subject, Body, ListEmailTO, ListEmailCC, ListEmailBCC, Priority, null, LinkedResources);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Send email message to multiple recepients with multiple CC/BCC options with some attachements and images in message
        /// </summary>
        /// <param name="Subject"></param>
        /// <param name="Body"></param>
        /// <param name="ListEmailTO"></param>
        /// <param name="ListEmailCC"></param>
        /// <param name="ListEmailBCC"></param>
        /// <param name="Priority"></param>
        /// <param name="AttchementPath"></param>
        /// <param name="LinkedResources"></param>
        /// <returns></returns>
        public bool SendEmail(string Subject, string Body, List<string> ListEmailTO, List<string> ListEmailCC = null, List<string> ListEmailBCC = null, MailPriority Priority = MailPriority.Normal, List<string> AttchementPath = null, string LinkedResources = null)
        {
            try
            {
                return SendEmailMessage(Subject, Body, ListEmailTO, ListEmailCC, ListEmailBCC, Priority, AttchementPath, LinkedResources);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Method to send Email
        /// </summary>
        /// <param name="objMail"></param>
        /// <returns></returns>
        private bool SendEmailMessage(string Subject, string Body, List<string> ListEmailTO, List<string> ListEmailCC = null, List<string> ListEmailBCC = null, MailPriority Priority = MailPriority.Normal, List<string> ListAttchementPath = null, string LinkedResources = null)
        {
            try
            {
                System.Net.Mail.MailMessage objMail = new System.Net.Mail.MailMessage();
                //Set from address
                objMail.From = new MailAddress(this.SenderEmailAddress);

                // Set to addresses
                if (ListEmailTO.Count > 0)
                {
                    foreach (string emailTO in ListEmailTO)
                    {
                        objMail.To.Add(new MailAddress(emailTO));
                    }
                }

                // Set CC addresses
                if (ListEmailCC != null)
                {
                    if (ListEmailCC.Count > 0)
                    {
                        foreach (string emailCC in ListEmailCC)
                        {
                            objMail.CC.Add(new MailAddress(emailCC));
                        }
                    }
                }

                // Set BCC addresses
                if (ListEmailBCC != null)
                {
                    if (ListEmailBCC.Count > 0)
                    {
                        foreach (string emailBCC in ListEmailBCC)
                        {
                            objMail.Bcc.Add(new MailAddress(emailBCC));
                        }
                    }
                }

                // Set the content
                // Set subject
                objMail.Subject = Subject;
                // Set body/message
                // objMail.Body = Body;
                AlternateView htmlMail = null;
                htmlMail = AlternateView.CreateAlternateViewFromString(Body, null, MediaTypeNames.Text.Html);

                // Set priority default is "Normal"
                objMail.Priority = Priority;

                objMail.AlternateViews.Add(htmlMail);
                objMail.IsBodyHtml = true;

                // Set priority default is "Normal"
                objMail.Priority = Priority;

                // Set attachement 
                if (ListAttchementPath != null)
                {
                    if (ListAttchementPath.Count > 0)
                    {
                        foreach (string path in ListAttchementPath)
                        {
                            objMail.Attachments.Add(new Attachment(path));
                        }
                    }
                }

                SmtpClient SMTPClientObj = new SmtpClient();
                SMTPClientObj.Credentials = new System.Net.NetworkCredential(this.SenderEmailAddress, this.SenderPassword);
                SMTPClientObj.Host = this.SenderHostName;
                SMTPClientObj.Port = this.SenderPort;
                SMTPClientObj.EnableSsl = this.EnableSSL;
                SMTPClientObj.Send(objMail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}