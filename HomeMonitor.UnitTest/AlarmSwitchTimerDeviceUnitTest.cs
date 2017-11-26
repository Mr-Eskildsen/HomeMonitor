using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HomeMonitor.model;
using HomeMonitor.Generic.xml;
using MemBus;
using MemBus.Configurators;
using MemBus.Subscribing;
using System.IO;
using System.Reflection;
using System.Xml;
using HomeMonitor.xml;
using System.Xml.Serialization;
using HomeMonitror.Security.xml;
using HomeMonitor.config;
using HomeMonitror.Security.model.sensor;
using System.Linq;
using HomeMonitor.Security;
using HomeMonitor.Security.xml;
using HomeMonitror.Security;
using System.Text;
using HomeMonitor.message;
using System.Threading;
using System.Data;

namespace HomeMonitor.UnitTest
{
    [TestClass]
    public class AlarmSwitchTimerDeviceUnitTest
    {
        private ThingsConfig currentThings = null;
        private SecurityConfig currentSecurityConfig = null;
        private static IBus mockupBus = null;

        public TestContext TestContext { get; set; }


        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            mockupBus = BusSetup.StartWith<Conservative>().Apply<FlexibleSubscribeAdapter>(a => a.RegisterMethods("Handle")).Construct();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            ThingRegistry.ClearData();
            SecurityAlarmMgr.ClearData();

            currentThings = ReadXmlConfigFile<ThingsConfig>("TestSet001", "things.xml");
            currentSecurityConfig = ReadXmlConfigFile<SecurityConfig>("TestSet001", "security.xml");
            ThingRegistry.Initialize(mockupBus, currentThings);
            SecurityAlarmMgr.Initialize(mockupBus, currentSecurityConfig);

            switch (TestContext.TestName)
            {
                case "ManualOnOverridesAutomatic1":
                    //this.IntializeTestMethod1();
                    break;
                case "ManualOnOverridesAutomatic2":
                    //this.IntializeTestMethod2();
                    break;
                default:
                    break;
            }
        }


        protected T ReadXmlConfigFile<T>(string testset, string filename)
        {
            string result = string.Empty;

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = string.Format("HomeMonitor.UnitTest.TestData.{0}.{1}", testset, filename);
            T config;

            //Stream s = assembly.GetManifestResourceStream(resourceName);
            //XmlDocument mappingFile = new XmlDocument();
            //string hejsa = "HomeMonitor.UnitTest.Scrpits." + testset + "." + filename;
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {

                config = ConfigFactory.LoadConfigFromStream<T>(stream);
                stream.Close();
            }
            return config;
        }

        [TestMethod]
        public void TestPirSensor()
        {
            string currentZoneId = "zone1";
            string currentThingId = "testpir1";
            string currentChannelId = "state";

            Channel channel = ThingRegistry.GetChannel(currentThingId, currentChannelId);
            AlarmDeviceConfig configDevice = currentSecurityConfig.Zones.Where<ZoneConfig>(zone => zone.Id == currentZoneId).First<ZoneConfig>().Devices.Where<AlarmDeviceConfig>(device => device.ThingId==currentThingId && device.ChannelId==currentChannelId).First<AlarmDeviceConfig>();

            Assert.AreEqual(channel.ThingId, configDevice.ThingId);
            Assert.AreEqual(channel.Id, configDevice.ChannelId);
            Assert.AreEqual(channel.State, null);

            mockupBus.Publish(new ChannelStateReceived(string.Empty, channel, true, ContactStates.CLOSED));
            Assert.AreEqual(channel.State, ContactStates.CLOSED.ToString());
            
            mockupBus.Publish(new ChannelStateReceived(string.Empty, channel, true, ContactStates.OPEN));
            Assert.AreEqual(channel.State, ContactStates.OPEN.ToString());

            mockupBus.Publish(new ChannelStateReceived(string.Empty, channel, true, ContactStates.CLOSED));
            Assert.AreEqual(channel.State, ContactStates.CLOSED.ToString());

        }
        /*
        [TestMethod]
        public void TestContactSensor()
        {
            string currentZoneId = "zone1";
            string currentThingId = "testcontact1";
            string currentChannelId = "state";

            Channel chPir = ThingRegistry.GetChannel(currentThingId, currentChannelId);
            AlarmDeviceConfig configDevice = currentSecurityConfig.Zones.Where<ZoneConfig>(zone => zone.Id == currentZoneId).First<ZoneConfig>().Devices.Where<AlarmDeviceConfig>(device => device.ThingId == currentThingId && device.ChannelId == currentChannelId).First<AlarmDeviceConfig>();

            Assert.AreEqual(chPir.ThingId, configDevice.ThingId);
            Assert.AreEqual(chPir.Id, configDevice.ChannelId);

            mockupBus.Publish(new ChannelStateReceived(string.Empty, chPir, true, ContactStates.CLOSED));
            Assert.AreEqual(chPir.State, ContactStates.CLOSED.ToString());

            mockupBus.Publish(new ChannelStateReceived(string.Empty, chPir, true, ContactStates.OPEN));
            Assert.AreEqual(chPir.State, ContactStates.OPEN.ToString());

            mockupBus.Publish(new ChannelStateReceived(string.Empty, chPir, true, ContactStates.CLOSED));
            Assert.AreEqual(chPir.State, ContactStates.CLOSED.ToString());
        }
        */
        [TestMethod]
        public void TestWindowSensor()
        {
            string currentZoneId = "zone1";
            string currentThingId = "testwindow1";
            string currentChannelId = "state";

            Channel chPir = ThingRegistry.GetChannel(currentThingId, currentChannelId);
            AlarmDeviceConfig configDevice = currentSecurityConfig.Zones.Where<ZoneConfig>(zone => zone.Id == currentZoneId).First<ZoneConfig>().Devices.Where<AlarmDeviceConfig>(device => device.ThingId == currentThingId && device.ChannelId == currentChannelId).First<AlarmDeviceConfig>();

            Assert.AreEqual(chPir.ThingId, configDevice.ThingId);
            Assert.AreEqual(chPir.Id, configDevice.ChannelId);

            mockupBus.Publish(new ChannelStateReceived(string.Empty, chPir, true, ContactStates.CLOSED));
            Assert.AreEqual(chPir.State, ContactStates.CLOSED.ToString());

            mockupBus.Publish(new ChannelStateReceived(string.Empty, chPir, true, ContactStates.OPEN));
            Assert.AreEqual(chPir.State, ContactStates.OPEN.ToString());

            mockupBus.Publish(new ChannelStateReceived(string.Empty, chPir, true, ContactStates.CLOSED));
            Assert.AreEqual(chPir.State, ContactStates.CLOSED.ToString());
        }


        [TestMethod]
        public void TestTamperSensor()
        {
            string currentZoneId = "zone1";
            string currentThingId = "testpir1";
            string currentChannelId = "state";
            
            Channel chPir = ThingRegistry.GetChannel(currentThingId, currentChannelId);
            AlarmDeviceConfig configDevice = currentSecurityConfig.Zones.Where<ZoneConfig>(zone => zone.Id == currentZoneId).First<ZoneConfig>().Devices.Where<AlarmDeviceConfig>(device => device.ThingId == currentThingId && device.ChannelId == currentChannelId).First<AlarmDeviceConfig>();

            Assert.AreEqual(chPir.ThingId, configDevice.ThingId);
            Assert.AreEqual(chPir.Id, configDevice.ChannelId);

            mockupBus.Publish(new ChannelStateReceived(string.Empty, chPir, true, ContactStates.CLOSED));
            Assert.AreEqual(chPir.State, ContactStates.CLOSED.ToString());

            mockupBus.Publish(new ChannelStateReceived(string.Empty, chPir, true, ContactStates.OPEN));
            Assert.AreEqual(chPir.State, ContactStates.OPEN.ToString());

            mockupBus.Publish(new ChannelStateReceived(string.Empty, chPir, true, ContactStates.CLOSED));
            Assert.AreEqual(chPir.State, ContactStates.CLOSED.ToString());
        }



        [TestMethod]
        public void TestSwitchDevice()
        {
            string currentZoneId = "zone1";
            string currentThingId = "testswitch1";
            string currentChannelId = "state1";

            Channel chSwitch = ThingRegistry.GetChannel(currentThingId, currentChannelId);
            AlarmDeviceConfig configDevice = currentSecurityConfig.Zones.Where<ZoneConfig>(zone => zone.Id == currentZoneId).First<ZoneConfig>().Devices.Where<AlarmDeviceConfig>(device => device.ThingId == currentThingId && device.ChannelId == currentChannelId).First<AlarmDeviceConfig>();

            Assert.AreEqual(chSwitch.ThingId, configDevice.ThingId);
            Assert.AreEqual(chSwitch.Id, configDevice.ChannelId);

            mockupBus.Publish(new ChannelStateReceived(string.Empty, chSwitch, true, SwitchStates.OFF));
            Assert.AreEqual(chSwitch.State, SwitchStates.OFF.ToString());

            mockupBus.Publish(new ChannelStateReceived(string.Empty, chSwitch, true, SwitchStates.ON));
            Assert.AreEqual(chSwitch.State, SwitchStates.ON.ToString());

            mockupBus.Publish(new ChannelStateReceived(string.Empty, chSwitch, true, SwitchStates.OFF));
            Assert.AreEqual(chSwitch.State, SwitchStates.OFF.ToString());
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
                   @"|DataDirectory|\TestData\TriggerTimeout.xml", "TriggerTimeout",
                    DataAccessMethod.Sequential)]
        public void TestTriggerTimeout()
        {
            string triggerThingId = (string)TestContext.DataRow["Trigger-ThingId"];
            string triggerChannelId = (string)TestContext.DataRow["Trigger-ChannelId"];
            int currentSensitivity = 5;
            AlarmZone zone = SecurityAlarmMgr.Instance.GetAlarmZoneById("zone1");
            //Channel chTrigger = ThingRegistry.GetChannel("testpir1", "state");
            Channel chTrigger = ThingRegistry.GetChannel(triggerThingId, triggerChannelId); 
            Channel chSwitch1 = ThingRegistry.GetChannel("testswitch1", "state1");
            Channel chSwitch2 = ThingRegistry.GetChannel("testswitch1", "state2");

            ZoneConfig configZone = currentSecurityConfig.Zones.First<ZoneConfig>(controlZone => controlZone.Id == zone.Id);
            Assert.AreEqual(zone.Id, configZone.Id);

            mockupBus.Publish(new ChannelStateReceived(string.Empty, chTrigger, true, ContactStates.CLOSED));
            Assert.AreEqual(chTrigger.State, ContactStates.CLOSED.ToString());
            Assert.AreEqual(chSwitch1.State, SwitchStates.OFF.ToString());
            Assert.AreEqual(chSwitch2.State, SwitchStates.OFF.ToString());

            mockupBus.Publish(new SecuritySystemStateMessage(AlarmSystemCommand.arm, currentSensitivity.ToString(), string.Empty));
            //Thread.Sleep(1);
            Assert.AreEqual(zone.Sensitivity, currentSensitivity);
            Assert.AreEqual(zone.Armed, true);
            

            //Alert from Pir
            mockupBus.Publish(new ChannelStateReceived(string.Empty, chTrigger, true, ContactStates.OPEN));
            
            Thread.Sleep(100);   //Make sure message is published
            Assert.AreEqual(chTrigger.State, ContactStates.OPEN.ToString());
            Assert.AreEqual(chSwitch1.State, SwitchStates.ON.ToString());
            Assert.AreEqual(chSwitch2.State, SwitchStates.ON.ToString());
            

            Thread.Sleep(3000);
            Assert.AreEqual(chTrigger.State, ContactStates.OPEN.ToString());
            Assert.AreEqual(chSwitch1.State, SwitchStates.ON.ToString());
            Assert.AreEqual(chSwitch2.State, SwitchStates.ON.ToString());

            Thread.Sleep(3000);
            Assert.AreEqual(chTrigger.State, ContactStates.OPEN.ToString());
            Assert.AreEqual(chSwitch1.State, SwitchStates.OFF.ToString());
            Assert.AreEqual(chSwitch2.State, SwitchStates.ON.ToString());

            Thread.Sleep(5000);
            Assert.AreEqual(chTrigger.State, ContactStates.OPEN.ToString());
            Assert.AreEqual(chSwitch1.State, SwitchStates.OFF.ToString());
            Assert.AreEqual(chSwitch2.State, SwitchStates.OFF.ToString());

            mockupBus.Publish(new ChannelStateReceived(string.Empty, chTrigger, true, ContactStates.CLOSED));

            Thread.Sleep(100);   //Make sure message is published
            Assert.AreEqual(chTrigger.State, ContactStates.CLOSED.ToString());
            Assert.AreEqual(chSwitch1.State, SwitchStates.OFF.ToString());
            Assert.AreEqual(chSwitch2.State, SwitchStates.OFF.ToString());

        }

        [TestMethod]
        public void ManualOnOverridesAutomatic1()
        {
            AlarmZone zone = SecurityAlarmMgr.Instance.GetAlarmZoneById("zone1");
            Channel chPir = ThingRegistry.GetChannel("testpir1", "state");
            Channel chSwitch1 = ThingRegistry.GetChannel("testswitch1", "state1");
            Channel chSwitch2 = ThingRegistry.GetChannel("testswitch1", "state2");

            ZoneConfig configZone = currentSecurityConfig.Zones.First<ZoneConfig>(controlZone => controlZone.Id == zone.Id);
            Assert.AreEqual(zone.Id, configZone.Id);

            mockupBus.Publish(new ChannelStateReceived(string.Empty, chPir, true, ContactStates.CLOSED));
            Assert.AreEqual(chPir.State, ContactStates.CLOSED.ToString());
            Assert.AreEqual(chSwitch1.State, SwitchStates.OFF.ToString());
            Assert.AreEqual(chSwitch2.State, SwitchStates.OFF.ToString());

            mockupBus.Publish(new SecuritySystemStateMessage(AlarmSystemCommand.arm, "10", string.Empty));
            //Thread.Sleep(1);
            Assert.AreEqual(zone.Sensitivity, 10);
            Assert.AreEqual(zone.Armed, true);


            //"Manual" turn on switch1
            mockupBus.Publish(new ChannelStateReceived(string.Empty, chSwitch1, true, SwitchStates.ON));
            Assert.AreEqual(chPir.State, ContactStates.CLOSED.ToString());
            Assert.AreEqual(chSwitch1.State, SwitchStates.ON.ToString());
            Assert.AreEqual(chSwitch2.State, SwitchStates.OFF.ToString());

            //Alert from Pir
            mockupBus.Publish(new ChannelStateReceived(string.Empty, chPir, true, ContactStates.OPEN));

            Thread.Sleep(100);   //Make sure message is published

            Assert.AreEqual(chPir.State, ContactStates.OPEN.ToString());
            Assert.AreEqual(chSwitch1.State, SwitchStates.ON.ToString());
            Assert.AreEqual(chSwitch2.State, SwitchStates.ON.ToString());

            //"Manual" turn on switch1
            mockupBus.Publish(new ChannelStateReceived(string.Empty, chSwitch1, true, SwitchStates.OFF));

            Thread.Sleep(100);   //Make sure message is published
            Assert.AreEqual(chPir.State, ContactStates.OPEN.ToString());
            Assert.AreEqual(chSwitch1.State, SwitchStates.OFF.ToString());
            Assert.AreEqual(chSwitch2.State, SwitchStates.ON.ToString());


            Thread.Sleep(11000);
            Assert.AreEqual(chPir.State, ContactStates.OPEN.ToString());
            Assert.AreEqual(chSwitch1.State, SwitchStates.OFF.ToString());
            Assert.AreEqual(chSwitch2.State, SwitchStates.OFF.ToString());


            mockupBus.Publish(new ChannelStateReceived(string.Empty, chPir, true, ContactStates.CLOSED));

            Thread.Sleep(100);   //Make sure message is published
            Assert.AreEqual(chPir.State, ContactStates.CLOSED.ToString());
            Assert.AreEqual(chSwitch1.State, SwitchStates.OFF.ToString());
            Assert.AreEqual(chSwitch2.State, SwitchStates.OFF.ToString());

        }

        /*
        public void TestTriggerTimeout()
        {
            string triggerThingId = (string)TestContext.DataRow["Trigger-ThingId"];
            string triggerChannelId = (string)TestContext.DataRow["Trigger-ChannelId"];
            int currentSensitivity = 5;
            Zone zone = SecurityAlarmMgr.Instance.GetZoneById("zone1");
            //Channel chTrigger = ThingRegistry.GetChannel("testpir1", "state");
            Channel chTrigger = ThingRegistry.GetChannel(triggerThingId, triggerChannelId);
            Channel chSwitch1 = ThingRegistry.GetChannel("testswitch1", "state1");
            Channel chSwitch2 = ThingRegistry.GetChannel("testswitch1", "state2");

            ZoneConfig configZone = currentSecurityConfig.Zones.First<ZoneConfig>(controlZone => controlZone.Id == zone.Id);
            Assert.AreEqual(zone.Id, configZone.Id);

            mockupBus.Publish(new ChannelStateReceived(string.Empty, chTrigger, true, ContactStates.CLOSED));
            Assert.AreEqual(chTrigger.State, ContactStates.CLOSED.ToString());
            Assert.AreEqual(chSwitch1.State, SwitchStates.OFF.ToString());
            Assert.AreEqual(chSwitch2.State, SwitchStates.OFF.ToString());

            mockupBus.Publish(new SecuritySystemStateMessage(AlarmSystemCommand.arm, currentSensitivity.ToString(), string.Empty));
            //Thread.Sleep(1);
            Assert.AreEqual(zone.Sensitivity, currentSensitivity);
            Assert.AreEqual(zone.Armed, true);


            //Alert from Pir
            mockupBus.Publish(new ChannelStateReceived(string.Empty, chTrigger, true, ContactStates.OPEN));

            Thread.Sleep(10);   //Make sure message is published
            Assert.AreEqual(chTrigger.State, ContactStates.OPEN.ToString());
            Assert.AreEqual(chSwitch1.State, SwitchStates.ON.ToString());
            Assert.AreEqual(chSwitch2.State, SwitchStates.ON.ToString());


            Thread.Sleep(3000);
            Assert.AreEqual(chTrigger.State, ContactStates.OPEN.ToString());
            Assert.AreEqual(chSwitch1.State, SwitchStates.ON.ToString());
            Assert.AreEqual(chSwitch2.State, SwitchStates.ON.ToString());

            Thread.Sleep(3000);
            Assert.AreEqual(chTrigger.State, ContactStates.OPEN.ToString());
            Assert.AreEqual(chSwitch1.State, SwitchStates.OFF.ToString());
            Assert.AreEqual(chSwitch2.State, SwitchStates.ON.ToString());

            Thread.Sleep(5000);
            Assert.AreEqual(chTrigger.State, ContactStates.OPEN.ToString());
            Assert.AreEqual(chSwitch1.State, SwitchStates.OFF.ToString());
            Assert.AreEqual(chSwitch2.State, SwitchStates.OFF.ToString());

            mockupBus.Publish(new ChannelStateReceived(string.Empty, chTrigger, true, ContactStates.CLOSED));

            Thread.Sleep(10);   //Make sure message is published
            Assert.AreEqual(chTrigger.State, ContactStates.CLOSED.ToString());
            Assert.AreEqual(chSwitch1.State, SwitchStates.OFF.ToString());
            Assert.AreEqual(chSwitch2.State, SwitchStates.OFF.ToString());

        }
        */
        [TestMethod]
        public void TestSensitivity()
        {
            AlarmZone zone = SecurityAlarmMgr.Instance.GetAlarmZoneById("zone1");
            
            Channel chPir = ThingRegistry.GetChannel("testpir1", "state");
            Channel chWindow = ThingRegistry.GetChannel("testpir1", "state");
            Channel chSwitch1 = ThingRegistry.GetChannel("testswitch1", "state1");
            Channel chSwitch2 = ThingRegistry.GetChannel("testswitch1", "state2");

            ZoneConfig configZone = currentSecurityConfig.Zones.First<ZoneConfig>(controlZone => controlZone.Id == zone.Id);
            Assert.AreEqual(zone.Id, configZone.Id);

            mockupBus.Publish(new ChannelStateReceived(string.Empty, chPir, true, ContactStates.CLOSED));
            mockupBus.Publish(new ChannelStateReceived(string.Empty, chWindow, true, ContactStates.CLOSED));

            Assert.AreEqual(chPir.State, ContactStates.CLOSED.ToString());
            Assert.AreEqual(chWindow.State, ContactStates.CLOSED.ToString());
            Assert.AreEqual(chSwitch1.State, SwitchStates.OFF.ToString());
            Assert.AreEqual(chSwitch2.State, SwitchStates.OFF.ToString());


            mockupBus.Publish(new SecuritySystemStateMessage(AlarmSystemCommand.disarm, string.Empty, string.Empty));
            //Thread.Sleep(1);
            Assert.AreEqual(0, zone.Sensitivity);
            Assert.AreEqual(false, zone.Armed);

            for (int idx = 1; idx <= 10; idx++)
            {
                mockupBus.Publish(new SecuritySystemStateMessage(AlarmSystemCommand.arm, idx.ToString(), string.Empty));
                Thread.Sleep(100);
                Assert.AreEqual(idx, zone.Sensitivity);
                Assert.AreEqual(true, zone.Armed);
            }
            mockupBus.Publish(new SecuritySystemStateMessage(AlarmSystemCommand.disarm, string.Empty, string.Empty));
            Thread.Sleep(2000);

            Assert.AreEqual(0, SecurityAlarmMgr.Instance.Sensitivity);
            Assert.AreEqual(false, SecurityAlarmMgr.Instance.Armed);

            Assert.AreEqual(0, zone.Sensitivity);
            Assert.AreEqual(false, zone.Armed);
            
        }


        [TestMethod]
        public void ManualOnOverridesAutomatic2()
        {
            AlarmZone zone = SecurityAlarmMgr.Instance.GetAlarmZoneById("zone1");
            Channel chPir = ThingRegistry.GetChannel("testpir1", "state");
            Channel chSwitch1 = ThingRegistry.GetChannel("testswitch1", "state1");
            Channel chSwitch2 = ThingRegistry.GetChannel("testswitch1", "state2");

            ZoneConfig configZone = currentSecurityConfig.Zones.First<ZoneConfig>(controlZone => controlZone.Id == zone.Id);
            Assert.AreEqual(zone.Id, configZone.Id);

            mockupBus.Publish(new ChannelStateReceived(string.Empty, chPir, true, ContactStates.CLOSED));
            mockupBus.Publish(new ChannelStateReceived(string.Empty, chSwitch1, true, SwitchStates.OFF));
            mockupBus.Publish(new ChannelStateReceived(string.Empty, chSwitch2, true, SwitchStates.OFF));
            Assert.AreEqual(chPir.State, ContactStates.CLOSED.ToString());
            Assert.AreEqual(chSwitch1.State, SwitchStates.OFF.ToString());
            Assert.AreEqual(chSwitch2.State, SwitchStates.OFF.ToString());


            mockupBus.Publish(new SecuritySystemStateMessage(AlarmSystemCommand.arm, "10", string.Empty));
            Thread.Sleep(100);   //Make sure message is published
            Assert.AreEqual(zone.Sensitivity, 10);
            Assert.AreEqual(zone.Armed, true);
            Assert.AreEqual(chPir.State, ContactStates.CLOSED.ToString());
            Assert.AreEqual(chSwitch1.State, SwitchStates.OFF.ToString());
            Assert.AreEqual(chSwitch2.State, SwitchStates.OFF.ToString());
            
            
            //Alert from Pir
            mockupBus.Publish(new ChannelStateReceived(string.Empty, chPir, true, ContactStates.OPEN));
            Thread.Sleep(100);   //Make sure message is published
            Assert.AreEqual(chPir.State, ContactStates.OPEN.ToString());
            Assert.AreEqual(chSwitch1.State, SwitchStates.ON.ToString());
            Assert.AreEqual(chSwitch2.State, SwitchStates.ON.ToString());

            //"Manual" turn on switch1
            mockupBus.Publish(new ChannelStateReceived(string.Empty, chSwitch1, true, SwitchStates.ON));
            Assert.AreEqual(chPir.State, ContactStates.OPEN.ToString());
            Assert.AreEqual(chSwitch1.State, SwitchStates.ON.ToString());
            Assert.AreEqual(chSwitch2.State, SwitchStates.ON.ToString());

            //"Manual" turn off switch1
            mockupBus.Publish(new ChannelStateReceived(string.Empty, chSwitch1, true, SwitchStates.OFF));
            Assert.AreEqual(chPir.State, ContactStates.OPEN.ToString());
            Assert.AreEqual(chSwitch1.State, SwitchStates.OFF.ToString());
            Assert.AreEqual(chSwitch2.State, SwitchStates.ON.ToString());

            //"Manual" turn on switch1
            mockupBus.Publish(new ChannelStateReceived(string.Empty, chSwitch1, true, SwitchStates.ON));
            Assert.AreEqual(chPir.State, ContactStates.OPEN.ToString());
            Assert.AreEqual(chSwitch1.State, SwitchStates.ON.ToString());
            Assert.AreEqual(chSwitch2.State, SwitchStates.ON.ToString());
            
            //Wait until timeout
            Thread.Sleep(11000);

            //Check states
            Assert.AreEqual(chPir.State, ContactStates.OPEN.ToString());
            Assert.AreEqual(chSwitch1.State, SwitchStates.ON.ToString());
            Assert.AreEqual(chSwitch2.State, SwitchStates.OFF.ToString());

            //Turn off switch 1 also
            mockupBus.Publish(new ChannelStateReceived(string.Empty, chSwitch1, true, SwitchStates.OFF));
            Assert.AreEqual(chPir.State, ContactStates.OPEN.ToString());
            Assert.AreEqual(chSwitch1.State, SwitchStates.OFF.ToString());
            Assert.AreEqual(chSwitch2.State, SwitchStates.OFF.ToString());
            
            //Release Pir sennsor
            mockupBus.Publish(new ChannelStateReceived(string.Empty, chPir, true, ContactStates.CLOSED));
            Assert.AreEqual(chPir.State, ContactStates.CLOSED.ToString());
            Assert.AreEqual(chSwitch1.State, SwitchStates.OFF.ToString());
            Assert.AreEqual(chSwitch2.State, SwitchStates.OFF.ToString());

        }
    }
}

