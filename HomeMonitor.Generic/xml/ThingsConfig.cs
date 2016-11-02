using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace HomeMonitor.Generic.xml
{
    [Serializable]
    [XmlRoot("things")]
    public class ThingsConfig : GenericConfig
    {
        //[XmlElement("purchase-date", IsNullable = true)]
        //public DateTime? PurchaseDate { get; set; }

        // for each subclass of ShoppingItem you need
        // to specify the correspondent XML element to generate
        //[XmlArrayItem("cd", typeof(CD))]
        //[XmlArrayItem("book", typeof(Book))]
        [XmlElement("thing", typeof(ThingConfig))]
        public List<ThingConfig> Things { get; set; }

        public override string GetId()
        {
            throw new NotImplementedException();
        }

        public override string GetName()
        {
            throw new NotImplementedException();
        }

        
        
    }
    
}
