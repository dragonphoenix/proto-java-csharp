using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DotNetty.Codecs.Protobuf;
using DotNetty.Common.Internal.Logging;
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
        private Bootstrap SocketBootstrap = new Bootstrap();
        private MultithreadEventLoopGroup WorkGroup = new MultithreadEventLoopGroup();
        private volatile bool Connected = false;
        private IChannel Channel;

        public ProtoBufSocket()
        {
            InitBootstrap();
        }

        private void InitBootstrap()
        {
            SocketBootstrap.Group(WorkGroup)
                .Channel<TcpSocketChannel>()
                .Option(ChannelOption.TcpNodelay, true)
                .Option(ChannelOption.SoKeepalive, true)
                .Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
                {
                    var pipeline = channel.Pipeline;
                    pipeline.AddLast(new ProtobufVarint32FrameDecoder());
                    pipeline.AddLast(new ProtobufDecoder(Message.Parser));

                    pipeline.AddLast(new ProtobufVarint32LengthFieldPrepender());
                    pipeline.AddLast(new ProtobufEncoder());

                    pipeline.AddLast(new BusinessChannelHandler(this, ReceiveMessage));
                }));
        }


        public void Connect()
        {
            var thread = new Thread(new ThreadStart(DoConnect().Wait));
            thread.Start();
        }
        public void Disconnect()
        {
            WorkGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1));
        }

        public void SendMessage(object message)
        {
            if (!Connected)
                Connected = ChannelInitilizedEvent.WaitOne();

            Channel.WriteAndFlushAsync(message);
        }

        private async Task DoConnect()
        {
            Connected = false;
            var connected = false;
            do
            {
                try
                {
                    var clientChannel = await SocketBootstrap.ConnectAsync("127.0.0.1", 8081);
                    Channel = clientChannel;
                    ChannelInitilizedEvent.Set();
                    connected = true;
                }
                catch (Exception /*ce*/)
                {
                    //Console.WriteLine(ce.StackTrace);
                    Console.WriteLine("Reconnect server after 5 seconds...");
                    Thread.Sleep(5000);
                }
            } while (!connected);
        }

    }
}
