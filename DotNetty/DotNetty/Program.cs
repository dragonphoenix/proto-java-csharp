using System;
using DotNetty.Transport.Channels;
using Google.Protobuf.Examples.AddressBook;
using Google.Protobuf.Multimessage;

namespace DotNetty
{
    class Program
    {
        static void Main(string[] args)
        {
            var t = new ProtoBufSocket();
            t.ReceiveMessage += ReceiveMessage;
            t.Connect();
            t.SendMessage(BuildObject());

            Console.ReadKey();
        }

        private static void ReceiveMessage(object message)
        {
            Console.WriteLine(message.ToString());
        }

        private static Message BuildObject()
        {
            var msg = new Message();
            msg.Type = Message.Types.Type.Bar;
            msg.Bar = new Bar();
            msg.Bar.Name = "Bar~~~~~~~~~~~~";

            return msg;
        }
    }

    class MyChannelHandler : ChannelHandlerAdapter
    {
        public event ReceiveMessageEvent ReceiveMessageEvent;

        public MyChannelHandler(ReceiveMessageEvent receiveMessageEvent)
        {
            ReceiveMessageEvent = receiveMessageEvent;
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            if (ReceiveMessageEvent != null)
            {
                ReceiveMessageEvent(message);
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
