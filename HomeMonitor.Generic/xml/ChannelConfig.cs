using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace HomeMonitor.Generic.xml
{

    [Flags]
    public enum ChannelSensitivity
    {
        [XmlEnum("normal")]
        normal = 0,

        [XmlEnum("pet")]
        pet = 1,


        [XmlEnum("pir")]
        pir = 5,

        [XmlEnum("perimeter")]
        perimeter = 10
    }


    public enum ChannelLocation
    {
        [XmlEnum("indoor")]
        state = 0,


        [XmlEnum("outdoor")]
        battery = 1

    }


    [Flags]
    public enum ChannelId
    {
        [XmlEnum("state")]
        State = 0,

        [XmlEnum("state1")]
        State1 = 1,

        [XmlEnum("state2")]
        State2 = 2,

        [XmlEnum("battery")]
        Battery = 98,

        [XmlEnum("tamper")]
        Tamper = 99
    }

    [Flags]
    public enum ChannelType
    {
        [XmlEnum("contact")]
        Contact = 0,

        [XmlEnum("switch")]
        Switch = 1,

        [XmlEnum("text")]
        Text = 2,

        [XmlEnum("number")]
        Number = 3,


        [XmlEnum("datetime")]
        DateTime = 3,


        [XmlEnum("battery")]
        Battery = 98,

        [XmlEnum("tamper")]
        Tamper = 99
    }



    [Serializable]
    [XmlRoot("channel")]
    public class ChannelConfig : GenericConfig
    {
        private string _name = String.Empty;
        public ChannelConfig()
        {
            //Perimeter = false;
        }

        [XmlAttribute("id")]
        public ChannelId Id { get; set; }


        [XmlAttribute("name"), DefaultValue("")]
        public string Name
        {
            get
            {
                if (String.IsNullOrEmpty(_name))
                    return Id.ToString();
                return _name;
            }
            set
            {
                _name = value;
            }
        }


        [XmlAttribute("type")]
        public ChannelType ChannelType { get; set; }

        
        [XmlAttribute("sensitivity"), DefaultValue(ChannelSensitivity.normal)]
        public ChannelSensitivity Sensitivity { get; set; }

        public Boolean AllowPet { get { return ((Sensitivity == ChannelSensitivity.pet) ? true : false); } }
        public Boolean Perimeter { get { return ((Sensitivity == ChannelSensitivity.perimeter) ? true : false); } }

        [XmlIgnore]
        public string Direction { get
            {
                switch(this.ChannelType)
                {
                    case ChannelType.Switch:
                        return "out";
                    default:
                        return "in";
            }
            }

        }
        public override string GetId()
        {
            return Id.ToString();
        }

        public override string GetName()
        {
            return Id.ToString();
        }

        

        
    }



}

