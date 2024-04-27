using UnityEngine;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.IO;

public class EmailSender : MonoBehaviour
{
    public string recipientEmail = "nandha@iastate.edu";
    public string subject = "Test Email with Attachment";
    public string body = "This is a test email with an attachment.";
    public string attachmentFilePath = "C:/Users/nandha/AppData/LocalLow/DefaultCompany/Document%20Scanner/output10.pdf"; // Change this to the path of your attachment file

    public string senderEmail = "inandha97@outlook.com";
    public string senderPassword = "Home@1728";

    public PNGToPDFConverter pdf;
    private void Start()
    {
        //SendEmail();
    }
    public void SendEmail()
    {
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress(senderEmail);
        mail.To.Add(recipientEmail);
        mail.Subject = subject;
        mail.Body = body;

        Attachment attachment = new Attachment(pdf.outputPath);
        mail.Attachments.Add(attachment);

        SmtpClient smtpServer = new SmtpClient("smtp-mail.outlook.com");
        smtpServer.Port = 587;
        smtpServer.Credentials = new NetworkCredential(senderEmail, senderPassword) as ICredentialsByHost;
        smtpServer.EnableSsl = true;

        ServicePointManager.ServerCertificateValidationCallback =
            delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            { return true; };

        smtpServer.Send(mail);
        Debug.Log("Email sent successfully.");
    }
}
