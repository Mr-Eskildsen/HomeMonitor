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

        //HEST public override void StateChangedEvent(object sender, StateChangedEventArgs args)
        //HEST {
        //HEST Console.WriteLine("Something happened to '" + args.ThingId + "' - '" + args.ChannelId + "'");
        //HEST }

        //HEST protected override GenericItem[] GetChildren()
        //HEST {
        //HEST             return Channels.ToArray<GenericItem>();

        //HEST     }
    }

}
