### 参考资料
[中文文档](https://doc.oschina.net/grpc)

[.net6+grpc文档](https://docs.microsoft.com/zh-cn/aspnet/core/grpc/?view=aspnetcore-6.0)

### 一、RPC允许定义四类方法：

1. 单向RPC，即客户端发送一个请求给服务端，从服务端获取一个应答，就像一次普通的函数调用
声明方式：
```
rpc SayHello(HelloRequest) returns (HelloResponse);
```

2. 服务端流式RPC（returns中使用stream修饰返回类型），即客户端发送一个请求给服务端，可获取一个数据流用来读取一系列消息。客户端从返回的数据流里一直读取，直到没有更多消息为止。【当客户端ip变化之后，是否有影响?】
声明方式：
```
rpc LotsOfReplies(HelloRequest) returns (stream HelloResponse);
```


3. 客户端流式RPC，即客户端提供的一个数据流写入（stream修饰入参类型），并发送一系列消息给服务端。一旦客户端完成消息写入，就等待服务端读取这些消息并返回应答。
声明方式：
```
rpc LotsOfGreetings(stream HelloRequest) returns (HelloResponse);
```


4. 双向流式RPC（stream修饰入参类型和返回类型），即两边都可以分别通过一个读写数据流来发送一系列消息。这两个数据流操作是相互独立的，所以客户端和服务端能按其希望的任意顺序读写。（例如，服务端可以在写应答钱等待所有的客户端消息，或者它可以先读一个消息再写一个消息，或者是读写相结合的其他方式。每个数据流利消息的顺序会被保持。）
声明方式：
```
rpc BidiHello(stream HelloRequest) returns (stream HelloResponse);
```

### 二、proto协议注意事项，参照以下模板中注释信息：
```
syntax = "proto3";
option csharp_namespace = "Test.Services";
package TestServices;

service Greeter {
  // Sends a greeting
  rpc SayHello (StringMessage) returns (StringMessage);
  rpc Bothway(stream StringMessage) returns (stream StringMessage);
}

service User{
	rpc GetUserNameById(LongMessage) returns (StringMessage);
	rpc GetUserInfoById(LongMessage) returns (stream StringMessage);
}

service TestSrv{
	rpc	GetPersonList(GetPersonRequest) returns(PersonListResponse){}
}

//begin 通用Message
message IntMessage {
  int32 Value = 1;
}

//void静态方法或者无参方法，返回值或请求参数需要声明一个空类
message EmptyMessage{}

message ListIntMessage{
  repeated int32 Items = 1;
}

message DobuleMessage{
  double Value=1;
}

message BoolMessage {
  bool Value = 1;
}

message StringMessage {
  string Value = 1;
}

message DoubleMessage{
  double Value=1;
}

message PagerRequest{
  int32 PageIndex=1;
  int32 PageSize=2;
  string OrderStr=3;
}

message PagerResponse{
  int32 PagerIndex=1;
  int32 PagerSize=2;
  int32 RowCount=3;
  int32 TotalPages=4;
}


message GetPersonRequest{
	string Name=1;
    string BrithdayFrom=2;
    string BrithdayTo=3;
	PagerRequest Pager=4;
}

message Person{
    string Name =1;
    int32 Age=2;
    string Brithday=3;
    double Balance=4;
}

message PersonListResponse{
	//repeated表示一个List列表
	repeated Person List=1;
	PagerResponse Pager=2;
}
```

### 三、grpc服务的部署问题：
1. 测试阶段可以使用http访问，发布包必须使用https，并配置.pfx证书路径。appsettigns.json根节点中添加以下配置：
```
  "Kestrel": {
    "Endpoints": {
      "HttpsInlineCertFile": {
        "Url": "https://*",
        "Protocols": "Http2",
        "Certificate": {
          "Path": "8405752__td365.com.cn.pfx",
          "Password": "BV40U3E9"
        }
      }
    }
  }
```
2. GRPC服务如果需要通过SLB负载均衡对外提供服务时，不能配置为https，要配置为TCP协议的443端口。（SLB提供的https证书可以下载后放入发布包）

