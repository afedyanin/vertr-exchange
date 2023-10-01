# vertr

Algorithmic Trading Engine

## Exchange Emulator Engine 

Inspired by

- [exchange-core](https://github.com/exchange-core/exchange-core)
- [LMAX Disruptor](https://github.com/LMAX-Exchange/disruptor)
- [Aeron](https://github.com/real-logic/aeron)


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
- 
