# proto-java-csharp

This is a protobuf example project. Server uses [Netty](https://github.com/netty/netty) with java code, and client uses [DotNetty](https://github.com/Azure/DotNetty) with c# code.

## Notice:
> maven protobuf message compile: **protobuf:compile**

> c# (vs2015) protobuf message compile: **python $(ProjectDir)\proto\protobuf.py -o$(ProjectDir)\GenCode -i$(ProjectDir)\proto**