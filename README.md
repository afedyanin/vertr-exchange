# vertr

Algorithmic Trading Engine

## Exchange Emulator Engine 

Inspired by

- [exchange-core](https://github.com/exchange-core/exchange-core)
- [LMAX Disruptor](https://github.com/LMAX-Exchange/disruptor)
- [Aeron](https://github.com/real-logic/aeron)


## GRPC

## Protobuf

-[Language Guide (proto 3)](https://protobuf.dev/programming-guides/proto3/)

## Basic

- [Tutorial: Create a gRPC client and server in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/tutorials/grpc/grpc-start?view=aspnetcore-7.0&tabs=visual-studio)
- [Введение в gRPC](https://metanit.com/sharp/grpc/1.1.php)
- [Службы gRPC на языке C#](https://learn.microsoft.com/ru-ru/aspnet/core/grpc/basics?view=aspnetcore-7.0)

- [Test gRPC services with Postman or gRPCurl in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/grpc/test-tools?view=aspnetcore-7.0)
- [Manage Protobuf references with dotnet-grpc](https://learn.microsoft.com/en-us/aspnet/core/grpc/dotnet-grpc?view=aspnetcore-7.0)
- [Troubleshoot gRPC on .NET](https://learn.microsoft.com/en-us/aspnet/core/grpc/troubleshoot?view=aspnetcore-7.0&preserve-view=true)

## Decimal Type
- [.NET Decimal DataType in gRPC](https://itnext.io/net-decimal-datatype-in-grpc-51c2ddb1c153)
- [C# Decimals in gRPC](https://visualrecode.com/blog/csharp-decimals-in-grpc/)
- [Создание настраиваемого десятичного типа для protobuf](https://learn.microsoft.com/ru-ru/dotnet/architecture/grpc-for-wcf-developers/protobuf-data-types)

## Auth
- [Streaming and Authentication in gRPC (ASP.Net Core)](https://dotnetcorecentral.com/blog/streaming-and-authentication-in-grpc/)
- [Authentication](https://grpc.io/docs/guides/auth/)

## NATS

### NATS and Docker

- [NATS Server Containerization](https://docs.nats.io/running-a-nats-service/nats_docker)

```
docker pull nats:latest

docker run -p 4222:4222 -ti nats:latest

```

### NATS resources

- [GitHub](https://github.com/nats-io)
- [Learn NATS by Example](https://natsbyexample.com/)
- [NATS Tools](https://docs.nats.io/using-nats/nats-tools)
- [NATS CLI](https://docs.nats.io/using-nats/nats-tools/nats_cli)

### Dotnet Clients
- [NATS.NET V2](https://github.com/nats-io/nats.net.v2)
- [NATS - .NET C# Client](https://github.com/nats-io/nats.net)

### Check NATS

```
nats sub msg.test

nats pub msg.test nats-message-1
```

### C# examples

- [Core Publish-Subscribe in Messaging](https://natsbyexample.com/examples/messaging/pub-sub/dotnet2)
- [GitHub](https://github.com/ConnectEverything/nats-by-example)
 
