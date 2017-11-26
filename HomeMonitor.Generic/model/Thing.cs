using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//HEST using SecurityMonitor.events;
using HomeMonitor.Generic.xml;

namespace HomeMonitor.model
{
    public class Thing : GenericItem
    {
        private Dictionary<String, Channel> channels = new Dictionary<String, Channel>();
        
        public String Description { get; set; }


        public Thing(ThingConfig config)
        {
            Id = config.Id;
            Name = config.Name;
            Description = config.Description;
        }

        public List<Channel> Channels { get { return channels.Values.ToList(); } }

        public Boolean addChannel(Channel ch)
        {
            channels.Add(ch.Id.ToLower(), ch);
            return true;
        }

        public Channel getChannel(String channelId)
        {
            Channel ch = null;
            if (channels.TryGetValue(channelId, out ch)==true)
            {
                return ch;
            }
            return null;
        }
        
    }

}
