# Hangfire 任务管理系统

## 项目简介

基于 **.NET 6** 和 **Hangfire** 实现的后端任务管理系统，支持多种任务调度方式，使用 MySQL 作为持久化存储。

## 功能特性

- **即发即忘任务 (Fire-and-Forget Jobs)**：立即执行的任务
- **延迟任务 (Delayed Jobs)**：在指定时间后执行的任务
- **周期性任务 (Recurring Jobs)**：基于 CRON 表达式执行的定时任务
- **任务监控仪表盘**：内置 Hangfire Dashboard
- **RESTful API**：完整的任务管理接口

## 技术栈

- **.NET 6** + **Hangfire** + **MySQL 8.0**
- **Docker & Docker Compose**：容器化部署，支持 ARM64 和 AMD64 架构

## 快速启动

### 前置条件

- Docker 和 Docker Compose

### 启动服务

```bash
# 构建并启动
docker-compose up -d --build

# 查看状态
docker-compose ps

# 查看日志
docker-compose logs -f backend

# 停止服务
docker-compose down
```

### 跨平台构建（Apple Silicon / Intel）

项目已配置支持多架构，Docker 会自动选择合适的镜像：

```bash
# 验证镜像支持 ARM64
docker pull --platform linux/arm64 mcr.microsoft.com/dotnet/aspnet:6.0

# 手动指定平台构建
docker-compose build --no-cache
```

## 访问地址

| 服务 | 地址 |
|------|------|
| Hangfire Dashboard | http://localhost:9999/hangfire |
| Swagger API 文档 | http://localhost:9999/swagger |
| API 基础路径 | http://localhost:9999/api |

## 项目结构

```
├── backend/                    # 后端项目
│   ├── Controllers/           # API 控制器
│   ├── Models/                # 数据模型
│   ├── Services/              # 业务服务
│   ├── Dockerfile             # 多架构 Docker 构建文件
│   └── Program.cs             # 程序入口
├── docs/                      # 文档
├── docker-compose.yml         # Docker Compose 配置
├── schema.sql                 # 数据库初始化脚本
└── README.md
```

## API 接口

### 即发即忘任务

```http
POST /api/jobs/fire-and-forget
Content-Type: application/json

{
  "taskType": "示例任务",
  "payload": "任务数据"
}
```

### 延迟任务

```http
POST /api/jobs/delayed
Content-Type: application/json

{
  "taskType": "延迟任务",
  "payload": "任务数据",
  "delaySeconds": 60
}
```

### 周期性任务

```http
POST /api/jobs/recurring
Content-Type: application/json

{
  "jobId": "daily-report",
  "taskType": "每日报告",
  "payload": "报告数据",
  "cronExpression": "0 0 9 * * ?"
}
```

### 删除周期性任务

```http
DELETE /api/jobs/recurring/{jobId}
```

## 数据库连接

- 主机：`localhost`
- 端口：`3307`
- 用户名：`root`
- 密码：`root`
- 数据库：`hangfire_db`

## 故障排查

1. **端口冲突**：确保 9999 和 3307 端口未被占用
2. **数据库连接失败**：等待 MySQL 健康检查通过（约 30 秒）
3. **构建失败**：执行 `docker-compose build --no-cache`
