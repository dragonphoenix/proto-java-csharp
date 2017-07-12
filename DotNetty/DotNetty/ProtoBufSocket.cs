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
using Google.Protobuf.Multimessage;

namespace DotNetty
{
    public delegate void ReceiveMessageEvent(object message);


    class ProtoBufSocket
    {
        public event ReceiveMessageEvent ReceiveMessage;

        private AutoResetEvent ChannelInitilizedEvent = new AutoResetEvent(false);
        private bool ChannelInitlized = false;
        private IChannel Channel;

        public void Connect()
        {
            var thread = new Thread(new ThreadStart(RunConnect().Wait));
            thread.Start();
        }

        public void SendMessage(object message)
        {
            if (!ChannelInitlized)
                ChannelInitlized = ChannelInitilizedEvent.WaitOne();

            Channel.WriteAndFlushAsync(message);
        }

        private async Task RunConnect()
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

                        pipeline.AddLast(new MyChannelHandler(ReceiveMessage));
                    }));

                var clientChannel = await bootstrap.ConnectAsync("127.0.0.1", 8081);
                Channel = clientChannel;
                ChannelInitilizedEvent.Set();

                while (true)
                {
                    Thread.Sleep(0);
                }
                //await clientChannel.CloseAsync();
            }
            finally
            {
                await group.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1));
            }
        }
    }
}
