package com.dragon.proto;

import com.dragon.proto.message.Messages.Baz;
import com.dragon.proto.message.Messages.Foo;
import com.dragon.proto.message.Messages.Message;
import com.dragon.proto.message.Messages.Message.Type;
import com.dragon.proto.utils.Random;
import com.example.tutorial.AddressBookProtos.Person;
import com.example.tutorial.AddressBookProtos.Person.Builder;
import com.example.tutorial.AddressBookProtos.Person.PhoneNumber;
import com.example.tutorial.AddressBookProtos.Person.PhoneType;

import io.netty.channel.Channel;
import io.netty.channel.ChannelHandlerContext;
import io.netty.channel.ChannelInboundHandlerAdapter;
import io.netty.channel.ChannelInitializer;
import io.netty.channel.ChannelPipeline;
import io.netty.channel.group.ChannelGroup;
import io.netty.channel.group.DefaultChannelGroup;
import io.netty.channel.socket.SocketChannel;
import io.netty.handler.codec.protobuf.ProtobufDecoder;
import io.netty.handler.codec.protobuf.ProtobufEncoder;
import io.netty.handler.codec.protobuf.ProtobufVarint32FrameDecoder;
import io.netty.handler.codec.protobuf.ProtobufVarint32LengthFieldPrepender;
import io.netty.util.concurrent.GlobalEventExecutor;

public class MyChannelInitializer extends ChannelInitializer<SocketChannel> {

	@Override
	protected void initChannel(SocketChannel ch) throws Exception {
		System.out.println("===========================initChannel");
		ChannelPipeline pipeline = ch.pipeline();

		pipeline.addLast(new ProtobufVarint32FrameDecoder());
		// pipeline.addLast(new ProtobufDecoder(Person.getDefaultInstance()));
		pipeline.addLast(new ProtobufDecoder(Message.getDefaultInstance()));

		pipeline.addLast(new ProtobufVarint32LengthFieldPrepender());
		pipeline.addLast(new ProtobufEncoder());

		pipeline.addLast(new MyChannelHandler());
	}

	public static class MyChannelHandler extends ChannelInboundHandlerAdapter {

		//private static int _id = 0;
		private static ChannelGroup channelGroup = new DefaultChannelGroup("channel group", GlobalEventExecutor.INSTANCE);

		@Override
		public void handlerAdded(ChannelHandlerContext ctx) throws Exception {
			// TODO Auto-generated method stub
			//System.out.println("==========================handlerAdded");
			super.handlerAdded(ctx);
		}

		@Override
		public void handlerRemoved(ChannelHandlerContext ctx) throws Exception {
			// TODO Auto-generated method stub
			//System.out.println("==========================handlerRemoved");
			super.handlerRemoved(ctx);
		}

		@Override
		public void channelRegistered(ChannelHandlerContext ctx) throws Exception {
			// TODO Auto-generated method stub
			super.channelRegistered(ctx);
			//System.out.println("==========================channelRegistered");
			channelGroup.add(ctx.channel());
			
			Message message = Message.newBuilder().setType(Type.BAZ).setBaz(Baz.newBuilder().setName("foo")).build();

			for (Channel channel : channelGroup) {
				channel.writeAndFlush(message);
			}
		}

		@Override
		public void channelUnregistered(ChannelHandlerContext ctx) throws Exception {
			// TODO Auto-generated method stub
			super.channelUnregistered(ctx);
			//System.out.println("==========================channelUnregistered");
			channelGroup.remove(ctx.channel());
		}

		@Override
		public void channelActive(ChannelHandlerContext ctx) throws Exception {
			//int id = _id++;
			Message message = Message.newBuilder().setType(Type.FOO).setFoo(Foo.newBuilder().setName("foo")).build();

			ctx.writeAndFlush(message);

			System.out.println("==========================channelActive");
			// super.channelActive(ctx);
		}

		@Override
		public void channelInactive(ChannelHandlerContext ctx) throws Exception {
			// TODO Auto-generated method stub
			//System.out.println("==========================channelInactive");
			super.channelInactive(ctx);
		}

		@Override
		public void channelRead(ChannelHandlerContext ctx, Object msg) throws Exception {
			System.out.println(msg);
			//System.out.println("==========================channelRead");
			//System.exit(0);
		}

		@Override
		public void channelReadComplete(ChannelHandlerContext ctx) throws Exception {
			// TODO Auto-generated method stub
			super.channelReadComplete(ctx);
			//System.out.println("==========================channelReadComplete");
		}

		@Override
		public void userEventTriggered(ChannelHandlerContext ctx, Object evt) throws Exception {
			// TODO Auto-generated method stub
			super.userEventTriggered(ctx, evt);
			//System.out.println("==========================userEventTriggered");
		}

		@Override
		public void channelWritabilityChanged(ChannelHandlerContext ctx) throws Exception {
			// TODO Auto-generated method stub
			super.channelWritabilityChanged(ctx);
			//System.out.println("==========================channelWritabilityChanged");
		}

		@Override
		public void exceptionCaught(ChannelHandlerContext ctx, Throwable cause) throws Exception {

			ctx.close();
			//System.out.println("==========================exceptionCaught");
		}

	}

	public static class MyChannelHandler1 extends ChannelInboundHandlerAdapter {

		private static int _id = 0;

		@Override
		public void channelRegistered(ChannelHandlerContext ctx) throws Exception {
			// TODO Auto-generated method stub
			super.channelRegistered(ctx);
		}

		@Override
		public void channelUnregistered(ChannelHandlerContext ctx) throws Exception {
			// TODO Auto-generated method stub
			super.channelUnregistered(ctx);
		}

		@Override
		public void channelActive(ChannelHandlerContext ctx) throws Exception {
			int id = _id++;
			Builder builder = Person.newBuilder().setId(id).setEmail(String.format("%d@%d.com", id, id))
					.setName("Sam" + id);
			PhoneNumber.Builder pBuilder = PhoneNumber.newBuilder();
			for (int i = 0; i < 2; i++) {
				builder.addPhones(pBuilder.setNumber(Random.randPhoneNum()).setType(PhoneType.forNumber(i % 3)));
			}
			Person person = builder.build();

			ctx.writeAndFlush(person);

			// super.channelActive(ctx);
		}

		@Override
		public void channelInactive(ChannelHandlerContext ctx) throws Exception {
			// TODO Auto-generated method stub
			super.channelInactive(ctx);
		}

		@Override
		public void channelRead(ChannelHandlerContext ctx, Object msg) throws Exception {
			Person person = (Person) msg;
			if (!person.toString().trim().isEmpty())
				System.out.println(msg);
			// super.channelRead(ctx, msg);
			System.out.println("AAAAAAAAAAAA");
		}

		@Override
		public void channelReadComplete(ChannelHandlerContext ctx) throws Exception {
			// TODO Auto-generated method stub
			super.channelReadComplete(ctx);
		}

		@Override
		public void userEventTriggered(ChannelHandlerContext ctx, Object evt) throws Exception {
			// TODO Auto-generated method stub
			super.userEventTriggered(ctx, evt);
		}

		@Override
		public void channelWritabilityChanged(ChannelHandlerContext ctx) throws Exception {
			// TODO Auto-generated method stub
			super.channelWritabilityChanged(ctx);
		}

		@Override
		public void exceptionCaught(ChannelHandlerContext ctx, Throwable cause) throws Exception {

			ctx.close();
		}

	}
}
