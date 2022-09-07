#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
# 暴露端口
EXPOSE 80
# 复制当前目录下的所有内容，到工作目录（使用 “COPY . .” 会使工作目录下多一级目录）
COPY ./ .
ENV ASPNETCORE_URLS https://*
ENV TZ=Asia/Shanghai
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
ENTRYPOINT ["dotnet", "MyGrpcService.dll"]