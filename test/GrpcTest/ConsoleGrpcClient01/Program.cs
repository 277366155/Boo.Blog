// The port number must match the port of the gRPC server.
using ConsoleGrpcClient01;

//await BothwayStreamClient.InitConnectAsync();
await ServerStreamClient.InitConnectAsync();
Console.ReadKey();

