using HomeMonitor.Generic.xml;
using HomeMonitor.logger;
using HomeMonitor.message;
using HomeMonitror.Security.xml;
using MemBus;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HomeMonitor.notification
{
    public class MailNotification : NotificationGeneric
    {
        IBus _bus = null;
        SmtpClient smtpClient = null; // new SmtpClient();

        private bool Armed { get; set; }
        private int Sensitivity { get; set; }

        private int MinSensitivity { get; set; }
        private int MaxSensitivity { get; set; }

        private string Subject { get; set; }
        private string Sender { get; set; }
        private List<string> Receivers = new List<string>();


        public MailNotification(IBus bus, MailConfig config)
        {
            _bus = bus;
            MinSensitivity = config.MinSensitivity;
            MaxSensitivity = config.MaxSensitivity;
            MinLevel = config.levelMin;
            MaxLevel = config.levelMax;


            _bus.Subscribe<SecuritySystemStateMessage>(msg => SystemStateChangedEvent(msg));

            smtpClient = new SmtpClient(config.Host);// "smtp.gmail.com");
            smtpClient.Port = config.Port;// 587;
            smtpClient.Credentials = new System.Net.NetworkCredential(config.UserId, config.Password); //new System.Net.NetworkCredential("my.name", "my.password");
            smtpClient.EnableSsl = true;
            ServicePointManager.ServerCertificateValidationCallback =
                delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };

            Sender = config.From;
            Subject = config.Subject;
            foreach (MailToConfig receiver in config.Receivers)
            {
                Receivers.Add(receiver.Address);
            }

        }


        private void _notify(MailMessage mail, Notification notify)
        {
            //TODO:: HAndle receivers and senders
            mail.Body += string.Format("{0:dd-MM-yyyy HH:mm:ss}: {1}\r\n\r\n", notify.EventDateTime, notify.Message);
                //string.Format("{0}\r\n\r\n", notify.Message);
        }


        private ConcurrentQueue<Notification> notifications = new ConcurrentQueue<Notification>();
        private Task dequeTask = null;


        protected void AddMessage(Notification notification)
        {
            notifications.Enqueue(notification);
            lock (this)
            {
                if (dequeTask == null)
                {
                    dequeTask = new Task(DequeueMessages);
                    dequeTask.Start();
                }
            }
        }
        protected void DequeueMessages()
        {
            Notification notification = null;

            //Sleep 30 seconds
            Thread.Sleep(30000);



            MailMessage mail = new MailMessage();

            mail.From = new MailAddress(Sender);

            foreach(string receiver in Receivers)
                mail.To.Add(receiver);

            mail.Subject = Subject;

            while (this.notifications.TryDequeue(out notification))
            {
                _notify(mail, notification);
            }

            lock (this)
            {
                dequeTask = null;
            }
            smtpClient.Send(mail);
        }

        protected void SystemStateChangedEvent(SecuritySystemStateMessage msg)
        {
            switch (msg.Command)
            {
                case AlarmSystemCommand.arm:
                case AlarmSystemCommand.sensitivity:
                    Armed = true;
                    Sensitivity = msg.SensitivityNew;
                    break;
                case AlarmSystemCommand.disarm:
                    Armed = false;
                    Sensitivity = 0;
                    break;
            }
        }

        protected void _notify(NotificationType type, String Message)
        {
            //ÆSEL
            if ((Armed==true) && ((MinSensitivity  <= Sensitivity) && (MaxSensitivity >= Sensitivity)) &&
                (MinLevel <= type) && (MaxLevel >= type))
            {
                AddMessage(new Notification(type, Message));
            }
        }

        public override void notifyAlert(String Message)
        {
            
            _notify(NotificationType.Alert, Message);
        }

        public override void notifyInfo(String Message)
        {
            String Subject = "";
            _notify(NotificationType.Info, Message);
        }

        public override void notifyWarning(String Message)
        {
            /*
             * String Subject = "";
            AddMessage(new Notification(NotificationType.Alert, "WARNING: " + Subject, Message));
            
            */
            //_notify(NotificationType.Warning, Message);
        }

    }
}
