using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ADODB;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NailsFramework.IoC;
using Encuentrame.Tests.Supports;
using Encuentrame.Support.Email;
using Encuentrame.Support.Email.Templates;
using Encuentrame.Support.Email.Templates.EmailModels;

namespace Encuentrame.Tests.EmailTests
{
    [TestClass]
    public class EmailServiceTests : BaseTest
    {
        private const string From = "me@home.com";
        //[Inject("EmailSavePath")]
        public string EmailSavePath { get; set; }
        public EmailService EmailService { get; set; }

        [TestInitialize]
        public override void SetUp()
        {
            InitializeDirectory();
            InitiMailServices();
        }

        private void InitializeDirectory()
        {
            EmailSavePath = ConfigurationManager.AppSettings["EmailSavePath"];
            bool exists = Directory.Exists(EmailSavePath);

            if (!exists)
                Directory.CreateDirectory(EmailSavePath);

            DirectoryInfo directory = new DirectoryInfo(EmailSavePath);
            foreach (FileInfo file in directory.GetFiles())
            {
                file.Delete();
            }
        }

        private void InitiMailServices()
        {
            EmailService = new EmailService();
            var emailServerConfiguration = new EmailServerConfiguration();

            emailServerConfiguration.From = From;
            EmailService.EmailSavePath = EmailSavePath;

            EmailService.Init(emailServerConfiguration);

            var templateManager = new EmailTemplateManager();

            EmailService.EmailTemplateManager = templateManager;
        }

        [TestMethod]
        public void SendingWelcomeUserEmail_WithTemplateName_SendsMailToPickupDirectory()
        {
            var email = new WelcomeUserEmailModel();
            email.UserName = "Tim Tom";
            email.Site = "Encuentrame";
            email.WelcomeInstructions = "A partir de ahora ud puede usar el sistema. Por favor ante cualquier duda o inconveniente comuniquese con el administrador.";
            var emailHeader = new MailHeader();
            emailHeader.To = "tim.tom@test.com";
            emailHeader.Subject = "Bienvenido a Encuentrame";

            EmailService.Send(emailHeader, EmailTemplateManager.WelcomeUserTemplateName, email);

            DirectoryInfo directory = new DirectoryInfo(EmailSavePath);
            var files = directory.GetFiles();
            Assert.AreEqual(1, files.Length);

            var mail = GetMessage(files[0]);
            Assert.IsTrue(mail.From.Contains(From));
            Assert.IsTrue(mail.To.Contains(emailHeader.To));
        }

        [TestMethod]
        public void SendingWelcomeUserEmail_WithTemplateType_SendsMailToPickupDirectory()
        {
            var email = new WelcomeUserEmailModel();
            email.UserName = "Tim Tom";
            email.Site = "Encuentrame";
            email.WelcomeInstructions = "A partir de ahora ud puede usar el sistema. Por favor ante cualquier duda o inconveniente comuniquese con el administrador.";
            var emailHeader = new MailHeader();
            emailHeader.To = "tim.tom@test.com";
            emailHeader.Subject = "Bienvenido a Encuentrame";

            EmailService.Send<WelcomeUserTemplate>(emailHeader, email);

            DirectoryInfo directory = new DirectoryInfo(EmailSavePath);
            var files = directory.GetFiles();
            Assert.AreEqual(1, files.Length);

            var mail = GetMessage(files[0]);
            Assert.IsTrue(mail.From.Contains(From));
            Assert.IsTrue(mail.To.Contains(emailHeader.To));
        }

        [TestMethod]
        public void SendingEmail_WithTestTemplate_SendsMailToPickupDirectory()
        {
            var email = new { Text = "Texto" };
            
            var emailHeader = new MailHeader();
            emailHeader.To = "tim.tom@test.com";
            emailHeader.Subject = "Bienvenido a Encuentrame";
            emailHeader.IsHtml = false;
            EmailService.Send<TestTemplate>(emailHeader, email);

            DirectoryInfo directory = new DirectoryInfo(EmailSavePath);
            var files = directory.GetFiles();
            Assert.AreEqual(1, files.Length);

            var mail = GetMessage(files[0]);
            var text1 = mail.TextBody.TrimEnd(Environment.NewLine.ToCharArray());
            text1 = text1.TrimStart(Environment.NewLine.ToCharArray());

            Assert.IsTrue(mail.From.Contains(From));
            Assert.IsTrue(mail.To.Contains(emailHeader.To));
            Assert.AreEqual(email.Text, text1);            
        }

        private CDO.Message GetMessage(FileInfo fileInfo)
        {
            CDO.Message msg = new CDO.Message();
            ADODB.Stream stream = new ADODB.Stream();
            stream.Type = StreamTypeEnum.adTypeBinary;
            stream.Open(Type.Missing, ADODB.ConnectModeEnum.adModeUnknown, ADODB.StreamOpenOptionsEnum.adOpenStreamUnspecified, String.Empty, String.Empty);
            stream.LoadFromFile(fileInfo.FullName);
            stream.Flush();
            msg.DataSource.OpenObject(stream, "_Stream");
            msg.DataSource.Save();
            return msg;
        }
    }
}
