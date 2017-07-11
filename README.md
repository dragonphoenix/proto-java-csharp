# proto-java-csharp

This is a protobuf example project. Server uses netty with java code, and client uses [DotNetty](https://github.com/Azure/DotNetty) with c# code.

## Notice:
> maven protobuf message compile: **protobuf:compile**

> c# protobuf message compile: **python $(ProjectDir)\proto\protobuf.py -o$(ProjectDir)\GenCode -i$(ProjectDir)\proto**