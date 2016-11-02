using HomeMonitor.Generic.xml;
using HomeMonitor.xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace HomeMonitor.config
{
    public class ConfigFactory
    {
        public static T LoadConfigFromXMLFile<T>(string filename)
        {
            //Console.WriteLine("Building Thing Registry.....");

            String appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            String configDir = Path.Combine(appDir, "config");
            String fullFilename = Path.Combine(configDir, filename);

            // A FileStream is needed to read the XML document.
            FileStream fs = new FileStream(fullFilename, FileMode.Open);


            return LoadConfigFromStream<T>(fs);
        }


        public static T LoadConfigFromStream< T > (Stream stream) {

            // Create an instance of the XmlSerializer specifying type and namespace.
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            XmlReader reader = XmlReader.Create(stream);

            // Declare an object variable of the type to be deserialized.
            T config;

            // Use the Deserialize method to restore the object's state.
            config = (T)serializer.Deserialize(reader);
            stream.Close();

            return config;
        }

            public static ApplicationConfig LoadAppConfig(string filename)
        {
            ApplicationConfig config = null;
            try
            {
                // Create an instance of the XmlSerializer specifying type and namespace.
                XmlSerializer serializer = new XmlSerializer(typeof(ApplicationConfig));


                string appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string configDir = Path.Combine(appDir, "config");
                string fullPath = Path.Combine(configDir, filename);

                // A FileStream is needed to read the XML document.
                FileStream fs = new FileStream(fullPath, FileMode.Open);
                XmlReader reader = XmlReader.Create(fs);


                // Use the Deserialize method to restore the object's state.

                config = (ApplicationConfig)serializer.Deserialize(reader);

                fs.Close();
                
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return config;
        }
    }
}
