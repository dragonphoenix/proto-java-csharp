using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DotNetty.Codecs.Protobuf;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Google.Protobuf.Examples.AddressBook;
using Google.Protobuf.Multimessage;

namespace DotNetty
{
    class Program
    {
        static void Main(string[] args)
        {
            Connect().Wait();
        }

        private static async Task Connect()
        {
            var group = new MultithreadEventLoopGroup();
            try
            {
                var bootstrap = new Bootstrap();
                bootstrap.Group(group)
                    .Channel<TcpSocketChannel>()
                    .Option(ChannelOption.TcpNodelay, true)
                    .Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
                    {
                        var pipeline = channel.Pipeline;
                        pipeline.AddLast(new ProtobufVarint32FrameDecoder());
                        pipeline.AddLast(new ProtobufDecoder(Message.Parser));

                        pipeline.AddLast(new ProtobufVarint32LengthFieldPrepender());
                        pipeline.AddLast(new ProtobufEncoder());

                        pipeline.AddLast(new MyChannelHandler());
                    }));

                var clientChannel = await bootstrap.ConnectAsync("127.0.0.1", 8081);

                while (true)
                {
                    Thread.Sleep(0);
                }
                await clientChannel.CloseAsync();
            }
            finally
            {
                await group.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1));
            }
        }
    }

    class MyChannelHandler : ChannelHandlerAdapter
    {
        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            Console.WriteLine(message.ToString());


            context.WriteAndFlushAsync(BuildObject());
            //context.WriteAndFlushAsync(BuildPerson());
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
