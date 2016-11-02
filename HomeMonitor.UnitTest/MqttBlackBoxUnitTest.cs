using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using uPLibrary.Networking.M2Mqtt;
using System.Threading;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt.Exceptions;
using System.Net;
using uPLibrary.Networking.M2Mqtt.Communication;

namespace HomeMonitor.UnitTest
{
    [TestClass]
    public class MqttBlackBoxUnitTest
    {



        private MqttBroker mqttBroker = null;
        private MqttClient mqttClient = null;

        private TestContext testContextInstance;
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }




        public MqttBlackBoxUnitTest()
        {
/*
            MqttSettings settings = MqttSettings.Instance;
            int Port = 11883;
            MqttTcpCommunicationLayer commLayer = new MqttTcpCommunicationLayer(Port);
            
            mqttBroker = new MqttBroker(commLayer, settings);
            


            mqttBroker.Start();

    //        Thread.Sleep(20000);
            try
            {
                string topic = "unittest/#";
                mqttClient = new MqttClient("127.0.0.1", 11883, false, null, null, MqttSslProtocols.None);
                
                
                mqttClient.MqttMsgConnected += client_MqttConnected;
                mqttClient.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
                
                string clientId = Guid.NewGuid().ToString();
                
                mqttClient.Connect("unittest");


                mqttClient.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
            }
            catch(MqttCommunicationException exception)
            {
               TestContext.WriteLine(exception.ToString());
            }
  */          
        }
        
        ~MqttBlackBoxUnitTest()
        {
            if (mqttClient!=null)
                mqttClient.Disconnect();

            if (mqttBroker != null)
                mqttBroker.Stop();
        }

        static void client_MqttConnected(object sender, MqttMsgConnectEventArgs e)
        {
        }


        static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {


        }
        /*
        [TestMethod]
        public void TestMethod1()
        {
            
            
            Console.WriteLine("HEST 1");
            Thread.Sleep(10000);
            Console.WriteLine("HEST 2");
            
        }
        */
    }
}

