using HomeMonitor.Generic.xml;
using MemBus;
//HEST using HomeMonitor.Notification.logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace HomeMonitor.model
{
    //http://csharpindepth.com/Articles/General/Singleton.aspx
    public /*sealed*/ class ThingRegistry
    {
        //HEST private static readonly log4net.ILog alarmLog = log4net.LogManager.GetLogger(typeof(AlarmExtensions));
        
        private Dictionary<String, Thing> registryThings = new Dictionary<String, Thing>();
        private Dictionary<String, Channel> registryChannels = new Dictionary<String, Channel>();
        //HEST private Dictionary<String, Zone> registryZones = new Dictionary<String, Zone>();

        private static readonly ThingRegistry instance = new ThingRegistry();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static ThingRegistry()
        {
        }

        private ThingRegistry()
        {
        }

        /*
        public static ThingsConfig LoadThingsFromXML(String fileName)
        {
            //Console.WriteLine("Building Thing Registry.....");

            // Create an instance of the XmlSerializer specifying type and namespace.
            XmlSerializer serializer = new
            XmlSerializer(typeof(ThingsConfig));


            String appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            String configDir = Path.Combine(appDir, "config");
            String filename = Path.Combine(configDir, fileName);

            // A FileStream is needed to read the XML document.
            FileStream fs = new FileStream(filename, FileMode.Open);
            */
            /*            XmlReader reader = XmlReader.Create(fs);

                        // Declare an object variable of the type to be deserialized.
                        ThingsConfig things;

                        // Use the Deserialize method to restore the object's state.
                        things = (ThingsConfig)serializer.Deserialize(reader);
                        fs.Close();
                        return things;
                        */
            //return LoadThingsFromStream<ThingsConfig
        //}


        protected static T LoadThingsFromStream<T>(Stream stream)
        {
        
            // Create an instance of the XmlSerializer specifying type and namespace.
            XmlSerializer serializer = new
            XmlSerializer(typeof(T));


            XmlReader reader = XmlReader.Create(stream);

            // Declare an object variable of the type to be deserialized.
            T things;

            // Use the Deserialize method to restore the object's state.
            things = (T)serializer.Deserialize(reader);
            stream.Close();

            return things;
        }


        public static String CalculateUniqueChannelId(String ThingId, String ChannelId)
        {
            return (ThingId + ":" + ChannelId).ToLower();
        }

        public static bool Initialize(IBus bus, ThingsConfig things)
        {
            return instance._Initialize(bus, things);
        }

        public static bool ClearData()
        {
            return instance._ClearData();
        }

        public static Thing GetThing(String Id)
        {
            return instance._GetThing(Id);
        }

        public static Channel GetChannel(String ThingId, String ChannelId)
        {
            return instance._GetChannel(ThingId, ChannelId);
        }


        private bool _ClearData()
        {
            registryThings.Clear();
            registryChannels.Clear();
            return true;
        }

        private void _AddThing(String Id, Thing thing)
        {
            registryThings.Add(Id, thing);
            foreach (Channel channel in thing.Channels)
            {
                registryChannels.Add(channel.UniqueId, channel);
            }
        }
        

        private Thing _GetThing(String Id)
        {
            Thing thing = null;

            if (instance.registryThings.TryGetValue(Id, out thing)==true)
            {
                return thing;
            }
            return null;
        }

        private Channel _GetChannel(String ThingId, String ChannelId)
        {
            Channel channel = null;
            

            if (instance.registryChannels.TryGetValue(ThingRegistry.CalculateUniqueChannelId(ThingId, ChannelId), out channel) == true)
            {
                return channel;
            }
            return null;
        }


        private bool _Initialize(IBus bus, ThingsConfig things)
        {
            //Console.WriteLine("Building Thing Registry.....");
           
            foreach (ThingConfig thingConfig in things.Things)
            {
                //HEST alarmLog.AlarmDebugFormat(thingConfig.Id, "Processing thing Id='{0}'", thingConfig.Id);
                Thing thing = new Thing(thingConfig);


                foreach (ChannelConfig channelConfig in thingConfig.Channels)
                {
                    Channel ch = Channel.Create(thing.Id, thingConfig, channelConfig, bus);
                    
                    if (ch != null)
                    {
                        
                        thing.addChannel(ch);
                    }
                    else
                        Console.WriteLine("  OOOOOOOPPPPS");
                    //HEST alarmLog.AlarmDebugFormat(thingConfig.GetId(), "Sensor '{0}' - '{1}':'{2}' (Perimeter='{3}')", thingConfig.Id, channelConfig.Id, channelConfig.ChannelType, channelConfig.Perimeter);
                }
                _AddThing(thing.Id, thing);
            }
            return true;
        }
    }
}
