package com.dragon.proto;

import io.netty.bootstrap.Bootstrap;
import io.netty.channel.ChannelFuture;
import io.netty.channel.EventLoopGroup;
import io.netty.channel.nio.NioEventLoopGroup;
import io.netty.channel.socket.nio.NioSocketChannel;

public class ProtoClient {

	public static void main(String[] args) throws Exception {
		int port = 8081;
		EventLoopGroup loopGroup = new NioEventLoopGroup();
		Bootstrap bootstrap = new Bootstrap();

		try {
			bootstrap.group(loopGroup).channel(NioSocketChannel.class).handler(new MyChannelInitializer());
			ChannelFuture future = bootstrap.connect("127.0.0.1", port).sync();
			future.channel().closeFuture().sync();
		} finally {
			loopGroup.shutdownGracefully();
		}
	}

}
