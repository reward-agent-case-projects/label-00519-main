# =============================================================================
# Multi-stage Dockerfile for .NET 6 Backend API
# Supports both ARM64 (Apple Silicon) and AMD64 (x86_64) architectures
# =============================================================================

# Build Stage - 使用官方 SDK 镜像进行编译
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# 声明构建参数
ARG TARGETARCH

WORKDIR /src

# 复制项目文件并还原依赖
COPY Backend.Api.csproj .
RUN dotnet restore "Backend.Api.csproj"

# 复制所有源代码
COPY . .

# 根据目标架构编译项目
RUN dotnet publish "Backend.Api.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore

# =============================================================================
# Runtime Stage - 使用轻量级运行时镜像
# mcr.microsoft.com/dotnet/aspnet:6.0 官方镜像支持多架构 (amd64, arm64)
# =============================================================================
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final

# 设置工作目录
WORKDIR /app

# 从构建阶段复制发布文件
COPY --from=build /app/publish .

# 设置环境变量
ENV ASPNETCORE_URLS=http://+:80
ENV DOTNET_RUNNING_IN_CONTAINER=true

# 暴露端口
EXPOSE 80

# 设置健康检查
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
    CMD curl -f http://localhost:80/health || exit 1

# 启动应用
ENTRYPOINT ["dotnet", "Backend.Api.dll"]
