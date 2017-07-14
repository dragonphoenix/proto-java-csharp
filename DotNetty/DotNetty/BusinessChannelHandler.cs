using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using Google.Protobuf.Examples.AddressBook;
using Google.Protobuf.Multimessage;

namespace DotNetty
{
    class BusinessChannelHandler : ChannelHandlerAdapter
    {
        public event ReceiveMessageEvent ReceiveMessageEvent;
        private ProtoBufSocket protoBufSocket;

        public BusinessChannelHandler(ProtoBufSocket protoBufSocket, ReceiveMessageEvent receiveMessageEvent)
        {
            ReceiveMessageEvent = receiveMessageEvent;
            this.protoBufSocket = protoBufSocket;
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            if (ReceiveMessageEvent != null)
            {
                ReceiveMessageEvent(message);
            }
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            if (protoBufSocket != null)
            {
                context.Channel.DisconnectAsync();
                Console.WriteLine("=================Channel Inactive");
                protoBufSocket.Connect();
            }
        }

        private Message BuildObject()
        {
            var msg = new Message();
            msg.Type = Message.Types.Type.Bar;
            msg.Bar = new Bar();
            msg.Bar.Name = "Bar";

            return msg;
        }
        private Person BuildObject1()
        {
            var person = new Person();
            person.Email = "p@p.com";
            person.Id = 123456;
            person.Name = "p";

            return person;
        }

    }
}
