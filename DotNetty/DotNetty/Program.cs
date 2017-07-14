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

            string cmd;

            do
            {
                cmd = Console.ReadLine();
                if (cmd == "A")
                {
                    t.SendMessage(BuildObject("A"));
                }
            } while (cmd != null && cmd.ToLower() != "q");

            t.Disconnect();
        }

        private static void ReceiveMessage(object message)
        {
            Console.WriteLine(message.ToString());
        }

        private static Message BuildObject(string extmsg = "")
        {
            var msg = new Message();
            msg.Type = Message.Types.Type.Bar;
            msg.Bar = new Bar();
            msg.Bar.Name = "Bar~~~~~~~~~~~~" + extmsg;

            return msg;
        }
    }
}
