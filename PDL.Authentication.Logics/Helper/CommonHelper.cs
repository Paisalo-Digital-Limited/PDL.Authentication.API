
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Newtonsoft.Json;
using PDL.Authentication.Entites.VM;
using PDL.Authentication.Logics.BLL;
using System.Net;
using System.Net.Http.Headers;

using System.Text;

namespace PDL.Authentication.Logics.Helper
{
    public class CommonHelper : BaseBLL
    {
        //public bool SendMail(SendMailVM objVM, string Type)
        //{
        //    bool isSuccess = false;
        //    var resetotp = "Your " + Type + " OTP is {newpwd}";
        //    try
        //    {
        //        resetotp = resetotp.Replace("{newpwd}", objVM.Password);
        //        MailMessage message = new MailMessage();
        //        SmtpClient smtp = new SmtpClient();
        //        message.From = new MailAddress("noreply@paisalo.in");
        //        message.Subject = objVM.Subject;
        //        message.IsBodyHtml = true;
        //        string[] ToMuliId = objVM.ToEmail.Split(',');
        //        foreach (string ToEMailId in ToMuliId)
        //        {
        //            message.To.Add(new MailAddress(ToEMailId));
        //        }
        //        if (!string.IsNullOrEmpty(objVM.ccEmail))
        //        {
        //            string[] CCId = objVM.ccEmail.Split(',');
        //            foreach (string CCEmail in CCId)
        //            {
        //                message.CC.Add(new MailAddress(CCEmail));

        //            }
        //        }
        //        if (objVM.attachment != null)
        //        {
        //            if (objVM.attachment.Length > 0)
        //            {
        //                using (var ms = new MemoryStream())
        //                {
        //                    objVM.attachment.CopyTo(ms);
        //                    var fileBytes = ms.ToArray();
        //                    Attachment att = new Attachment(new MemoryStream(fileBytes), objVM.attachment.FileName);
        //                    message.Attachments.Add(att);

        //                }
        //            }
        //        }
        //        message.Body = resetotp;
        //        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        smtp.Port = 587;
        //        smtp.Host = "smtp.gmail.com";
        //        smtp.EnableSsl = true;
        //        smtp.UseDefaultCredentials = false;
        //       // smtp.Credentials = new NetworkCredential("reporting.donotreply@seil.in", "Seil2@Report");
        //        smtp.Credentials = new NetworkCredential("noreply1@paisalo.in", "Norep@34$w&");
        //        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        smtp.Send(message);
        //        isSuccess = true;

        //    }
        //    catch (Exception ex)
        //    {
        //        //throw new Exception(ex.Message);
        //        return false;
        //    }
        //    return isSuccess;

        //}
        
        public bool SendMail(SendMailVM objVM, string Type)
        {
            bool isSuccess = false;
            var resetotp = "Your " + Type + " OTP is {newpwd}";
            try
            {
                // Replace OTP in the body text
                resetotp = resetotp.Replace("{newpwd}", objVM.Password);

                // Create MimeMessage
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Paisalo Digital Limited", "noreply@paisalo.in"));

                // To - Handle multiple recipients
                var toList = objVM.ToEmail.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var to in toList)
                    message.To.Add(new MailboxAddress("", to.Trim()));

                // CC - Handle multiple CCs
                if (!string.IsNullOrWhiteSpace(objVM.ccEmail))
                {
                    var ccList = objVM.ccEmail.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var cc in ccList)
                        message.Cc.Add(new MailboxAddress("", cc.Trim()));
                }

                // Set the subject
                message.Subject = objVM.Subject;

                // Create the body builder with OTP in HTML format
                var builder = new BodyBuilder
                {
                    HtmlBody = $"<p>{resetotp}</p>"
                };

                // Handle attachment if available
                if (objVM.attachment != null)
                {
                    if (objVM.attachment.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            objVM.attachment.CopyTo(ms);
                            var fileBytes = ms.ToArray();
                            builder.Attachments.Add(objVM.attachment.FileName, fileBytes, new ContentType("application", "octet-stream"));
                        }
                    }
                }

                // Assign the message body
                message.Body = builder.ToMessageBody();

                // Send the email using MailKit's SMTP client
                using (var client = new SmtpClient())
                {
                    // Connect to the SMTP server
                    client.Connect("email.paisalo.in", 465, SecureSocketOptions.SslOnConnect); // Implicit SSL
                    client.Authenticate("noreply1@paisalo.in", "Norep@34$w&"); // Use correct email and password

                    // Send the email
                    client.Send(message);
                    client.Disconnect(true);
                }

                isSuccess = true;
            }
            catch (Exception ex)
            {
                // Handle any errors
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
            return isSuccess;
        }

        public string GenerateOTP()
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
        public string GenerateOTP(int length = 6)
        {
            Random random = new Random();
            string otp = new string(Enumerable.Repeat("0123456789", length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            return otp;
        }
    }
}