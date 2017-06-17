using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using BLL;
using API.Models;
using System.Configuration;
using AttributeRouting.Web.Http;
using Common;


namespace API.Controllers
{
    public class RegisterController : ApiController
    {
        // GET api/values
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}



        // POST api/values
        public string Post(Register_model @user)
        {
            try
            {
                string token = Register.NewRegister(@user).ToString();
                EmailClass EC = new EmailClass();
                EC.SenderEmailAddress = ConfigurationManager.AppSettings["sender_email"];
                EC.SenderPassword = ConfigurationManager.AppSettings["sender_password"];
                EC.SenderHostName = ConfigurationManager.AppSettings["sender_host_name"];
                EC.SenderPort = Convert.ToInt32(ConfigurationManager.AppSettings["sender_port_number"]);
                EC.EnableSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["enable_ssl"]);

                String URL = Request.RequestUri.OriginalString.Replace(Request.RequestUri.PathAndQuery, "");
                string RESET_LINK_MAIL_BODY = "Hello,<br /><br />Please click below link to verify your account.<br /><br /><a href='{0}'>Verify account</a><br /><br /><br />Regards,<br />Bank of Baroda";
                string RESET_LINK_MAIL_SUBJECT = "Account Varification";

                RESET_LINK_MAIL_BODY = String.Format(RESET_LINK_MAIL_BODY, URL + "/Home/ValidateToken?ID=" + token);
                bool EmailSent = EC.SendEmail(RESET_LINK_MAIL_SUBJECT, RESET_LINK_MAIL_BODY, @user.EmailID, System.Net.Mail.MailPriority.Normal, null, null);

                return "1";
            }
            catch (Exception ex)
            {
                return "";
            }
            return "";

        }

        [System.Web.Http.HttpGet]
        [GET("Register/ValidateToken/{token}")]
        public string ValidateToken(string id)
        {
            return Token.ValidateToken(id).ToString();
        }


        [System.Web.Http.HttpGet]
        [GET("Register/EmailAvailability/{id}")]
        public string EmailAvailability(string id)
        {
            Boolean Flag = Register.CheckEmailAvailability(id);
            string returnvalue = "";
            if (Flag == false)
            {
                returnvalue = "0";
            }
            else
            {
                returnvalue = "1";
            }
            return returnvalue;

        }




        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

    }
}
