using Newtonsoft.Json;
using PDL.Authentication.Entites.VM;
using PDL.Authentication.Logics.BLL;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;

namespace PDL.Authentication.Logics.Helper
{
    public class CommonHelper : BaseBLL
    {
        public  bool SendMail(SendMailVM objVM)
        {
            bool isSuccess = false;
            var resetpassword = "Your New Password is {newpwd} . Now after Login Your Account and change your Password";
            try
            {
                resetpassword = resetpassword.Replace("{newpwd}", objVM.Password);
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("reporting.donotreply@seil.in");
                message.Subject = objVM.Subject;
                message.IsBodyHtml = true;
                string[] ToMuliId = objVM.ToEmail.Split(',');
                foreach (string ToEMailId in ToMuliId)
                {
                    message.To.Add(new MailAddress(ToEMailId));
                }
                if (!string.IsNullOrEmpty(objVM.ccEmail))
                {
                    string[] CCId = objVM.ccEmail.Split(',');
                    foreach (string CCEmail in CCId)
                    {
                        message.CC.Add(new MailAddress(CCEmail));

                    }
                }
                if (objVM.attachment != null)
                {
                    if (objVM.attachment.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            objVM.attachment.CopyTo(ms);
                            var fileBytes = ms.ToArray();
                            Attachment att = new Attachment(new MemoryStream(fileBytes), objVM.attachment.FileName);
                            message.Attachments.Add(att);

                        }
                    }
                }
                message.Body = resetpassword;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("reporting.donotreply@seil.in", "Seil2@Report");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
                isSuccess = true;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return isSuccess;

        }
        public  string GenerateOTP()
        {
            string PasswordLength = "6";
            string NewPassword = "";

            string allowedChars = "";
            allowedChars = "1,2,3,4,5,6,7,8,9,0";
            // allowedChars += "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,";
            //allowedChars += "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,";

            char[] sep = {
            ','
            };
            string[] arr = allowedChars.Split(sep);


            string IDString = "";
            string temp = "";

            Random rand = new Random();

            for (int i = 0; i < Convert.ToInt32(PasswordLength); i++)
            {
                temp = arr[rand.Next(0, arr.Length)];
                IDString += temp;
                NewPassword = IDString;

            }
            return NewPassword;
        }
    }
}