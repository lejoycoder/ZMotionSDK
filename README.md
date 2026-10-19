# ZMotionSDK - 专业C#运动控制SDK

[![.NET 8.0](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![NuGet](https://img.shields.io/nuget/v/ZMotionSDK.svg)](https://www.nuget.org/packages/ZMotionSDK)
[![平台](https://img.shields.io/badge/平台-Windows-lightgrey.svg)](https://www.microsoft.com/windows)

ZMotionSDK是一个专业的C#运动控制器软件开发包，为正运动ZMotion系列运动控制器提供完整的编程接口。该SDK基于.NET 8.0开发，提供高性能、易用的工业级运动控制解决方案。

## 🌟 核心特性

### 🔌 多样化连接方式
- **以太网通信**: 支持TCP/IP网络连接，快速稳定
- **串口通信**: 兼容传统串口通信方式
- **超时控制**: 可配置连接超时时间，确保连接可靠性

### ⚡ 高性能运动控制
- **插补功能**: 支持直线、圆弧插补运动
- **缓存运动**: 运动指令缓存机制，实现平滑连续运动
- **实时监控**: 位置、速度、状态的实时反馈

### 📡 完整IO控制系统
- **数字输入输出**: 高速DI/DO读写操作
- **模拟输入输出**: 读取 AI、读取和设置 AO，直接访问控制器模拟量通道
- **批量操作**: 支持多点IO的批量读写，提高效率
- **信号反转**: 智能信号反转配置，适应不同硬件接线
- **Modbus支持**: 原生支持Modbus协议的IO操作

### 🏠 智能回零系统
- **多种回零模式**: 支持8种标准回零模式
- **参数可配置**: 回零速度、偏移量、等待时间等全可配置
- **总线回零**: 支持EtherCAT等总线驱动器的回零功能
- **状态监控**: 实时回零状态监控和异常处理

### 🔄 IOSugar（协议糖化）框架
- 类型安全映射：通过 `[Address]` 将 DI/DO 地址映射到协议结构体的 `bool` 属性。
- 批量读写与缓存：提供同步 `ReadDI`、`ReadDO`、`Write` 方法，保留最近读取的协议数据和原始数据。
- 链式写入：使用 `CreateWriteMapping()` 组合多个输出修改，再通过 `Commit()` 提交。

详细用法见 [ZMotionIOClient 使用指南](docs/ZMotionIOClient_Guide.md)。AI/AO 使用下方的 `ZMotion` 接口直接访问。

### 🛡️ 安全与异常处理
- **完善的异常体系**: 专门的ZMotionException异常处理
- **错误码解析**: 详细的错误码说明和处理建议
- **轴状态监控**: 全面的轴运行状态、限位、告警监控
- **安全机制**: 软硬限位保护，急停减速度配置

## 🔧 快速开始

### 安装

通过NuGet包管理器安装：

```bash
Install-Package ZMotionSDK
```

或通过.NET CLI：

```bash
dotnet add package ZMotionSDK
```

运行环境为 Windows 和 .NET 8.0 或更高版本。请将正运动官方提供的、与应用进程位数匹配的 `zauxdll.dll` 和 `zmotion.dll` 放入应用输出目录；当前 NuGet 包不包含这两个原生 DLL。

### 基础使用示例

```csharp
using ZMotionSDK;
using ZMotionSDK.Models;

class Program
{
    static async Task Main()
    {
        var zmotion = new ZMotion();
        
        try
        {
            // 连接控制器
            zmotion.Open("192.168.1.100", 3000);
            Console.WriteLine("控制器连接成功");
            
            // 配置轴参数
            var axisParam = new AxisParam
            {
                Speed = 100.0f,              // 运行速度
                Acceleration = 1000.0f,      // 加速度
                Deceleration = 1000.0f,      // 减速度
                SmoothingFactor = 0.1f,      // 平滑因子
                Units = 1000.0f,             // 脉冲当量
                PositiveLimit = 10000.0f,    // 正向软限位
                NegativeLimit = -10000.0f,   // 负向软限位
                EmergencyDeceleration = 5000.0f // 急停减速度
            };
            
            // 应用轴参数
            zmotion.SetAxisParam(0, axisParam);
            
            // 执行绝对位置移动
            zmotion.Move_Absolute(0, 5000.0f);
            Console.WriteLine("开始移动到位置5000");
            
            // 异步等待运动完成
            var result = await zmotion.WaitMoveCompleteAsync(0, 10000);
            if (result.IsSuccess)
            {
                Console.WriteLine($"运动完成，耗时: {result.ElapsedTime:F2}ms");
            }
            else
            {
                Console.WriteLine("运动超时");
            }
            
            // 读取当前状态
            var motionState = zmotion.GetAxisMotionState(1)[0];
            Console.WriteLine($"当前位置: {motionState.CurrentPosition}");
            Console.WriteLine($"当前速度: {motionState.CurrentSpeed}");
            Console.WriteLine($"运行状态: {motionState.IsRunning}");
        }
        catch (ZMotionException ex)
        {
            Console.WriteLine($"控制器错误: {ex.Message}");
        }
        finally
        {
            zmotion.Close();
            Console.WriteLine("连接已关闭");
        }
    }
}
```

## 📚 详细功能说明

### 1. 连接管理

```csharp
// 方式1：带超时的快速连接
zmotion.Open("192.168.1.100", 3000);

// 方式2：标准以太网连接
zmotion.Open_Eth("192.168.1.100");

// 关闭连接
zmotion.Close();
```

### 2. 数字IO控制

#### 基础IO操作
```csharp
// 读取单个数字输入
bool diValue = zmotion.GetDI(0);

// 批量读取数字输入
bool[] diArray = zmotion.GetDI_Multi(0, 15);

// 设置单个数字输出
zmotion.SetDO(0, true);

// 批量设置数字输出
bool[] doValues = { true, false, true, false };
zmotion.SetDO_Multi(0, doValues);
```

#### 协议糖化IO操作
```csharp
using MessageToolkit.Attributes;
using ZMotionSDK;

// 定义输入协议结构
public struct DIProtocol
{
    [Address(0)] public bool StartButton { get; set; }
    [Address(1)] public bool StopButton { get; set; }
    [Address(2)] public bool EmergencyStop { get; set; }
    [Address(3)] public bool SafetyDoor { get; set; }
}

// 定义输出协议结构
public struct DOProtocol
{
    [Address(0)] public bool MotorEnable { get; set; }
    [Address(1)] public bool AlarmLight { get; set; }
    [Address(2)] public bool RunningLight { get; set; }
    [Address(3)] public bool CompletedLight { get; set; }
}

// 创建 IOSugar 客户端，ZMotion 是 required 属性
var builder = new ZMotionIOClient<DIProtocol, DOProtocol>
{
    ZMotion = zmotion
};

// 读取结构化的输入数据
var inputs = builder.ReadDI();
if (inputs.StartButton && !inputs.EmergencyStop)
{
    // 设置结构化的输出数据并写回
    var outputs = new DOProtocol
    {
        MotorEnable = true,
        RunningLight = true,
        AlarmLight = false,
        CompletedLight = false
    };
    builder.Write(outputs);
}
```

#### 模拟量 AI/AO 操作（2.2.0 新增）

| 接口 | 功能 | 底层接口 |
| --- | --- | --- |
| `float GetAI(int axis, int ionum)` | 读取模拟量输入通道 | `ZAux_Direct_GetAD` |
| `float GetAO(int axis, int ionum)` | 读取模拟量输出通道的值 | `ZAux_Direct_GetDA` |
| `void SetAO(int axis, int ionum, float value)` | 设置模拟量输出通道的值 | `ZAux_Direct_SetDA` |

三个接口均为同步调用，需要先连接控制器。当前 `axis` 参数未使用，传入 `0` 即可；实际通道由 `ionum` 指定。AI 是输入，当前仅提供读取接口。

```csharp
using ZMotionSDK;

var zmotion = new ZMotion();
zmotion.Open("192.168.1.100", 3000);
try
{
    float input = zmotion.GetAI(axis: 0, ionum: 0);
    float output = zmotion.GetAO(axis: 0, ionum: 0);
    Console.WriteLine($"AI[0] = {input}, AO[0] = {output}");

    // 确认硬件允许该值后再写入；此处 0 是控制器数值，不代表安全状态。
    zmotion.SetAO(axis: 0, ionum: 0, value: 0.0f);
}
catch (ZMotionException ex)
{
    Console.WriteLine($"模拟量操作失败: {ex.Message}");
}
finally
{
    zmotion.Close();
}
```

通道编号、数值范围、单位及校准方式以控制器/扩展模块手册为准。SDK 原样传递 `float` 数值，不进行电压/电流换算、缩放或限幅；不要将数值直接理解为 V 或 mA。底层接口注释中的 `0–4095` 仅适用于其注明的 4 系列以下产品，不能作为所有设备的统一量程。`GetAO` 返回控制器提供的输出值，不替代外部仪表对实际电压或电流的测量。控制器返回非零错误码时，接口抛出 `ZMotionException`。

### 3. 运动控制

#### 基本运动控制
```csharp
// 单轴绝对移动
zmotion.Move_Absolute(0, 1000.0f);

// 单轴相对移动
zmotion.Move_Relative(0, 500.0f);

// 连续运动（点动）
zmotion.Jog(0, true);  // 正向点动
zmotion.Jog(0, false); // 反向点动

// 停止运动
zmotion.Stop(0, CancelMode.取消当前运动和缓冲运动);
```

#### 回零操作
```csharp
// 配置回零参数
zmotion.SetGoHomeCreepSpeed(0, 10.0f);     // 设置慢速回零速度
zmotion.SetGoHomeWaitTime(0, 100);         // 设置回零等待时间
zmotion.SetGoHomeOffpos(0, 50.0f);         // 设置回零偏移量

// 执行回零
zmotion.GoHome(0, 1);  // 模式1回零

// 检查回零状态
bool homeStatus = zmotion.GetHomeStatus(0);
```

#### 缓存运动模式
```csharp
// 构建连续运动序列
zmotion.Move_Absolute(0, 1000.0f);
zmotion.MoveDelay(0, 500);                // 延时500ms
zmotion.Move_Relative(0, 500.0f);
zmotion.MoveOp(0, 1, 1);                  // 运动过程中控制输出
zmotion.Move_Absolute(0, 0.0f);

// 查询缓存状态
int bufferedMoves = zmotion.GetMovesBuffered(0);
int remainBuffer = zmotion.GetRemainBuffer(0);
```

### 4. 参数配置与监控

#### 轴参数配置
```csharp
// 基本运动参数
zmotion.SetSpeed(0, 100.0f);           // 设置运行速度
zmotion.SetAccel(0, 1000.0f);          // 设置加速度
zmotion.SetDecel(0, 1000.0f);          // 设置减速度
zmotion.SetSpeed_L(0, 10.0f);          // 设置起始速度
zmotion.SetSramp(0, 0.1f);             // 设置S曲线参数

// 脉冲参数
zmotion.SetUnits(0, 1000.0f);          // 设置脉冲当量
zmotion.SetAxisType(0, AxisType.脉冲方向方式的步进和伺服);// 设置轴类型

// 安全参数
zmotion.SetLimit(0, 10000.0f, true);   // 设置正向软限位
zmotion.SetLimit(0, -10000.0f, false); // 设置负向软限位
zmotion.SetDecel_Fast(0, 5000.0f);     // 设置急停减速度
```

#### 状态监控
```csharp
// 获取详细的轴运动状态
var motionStates = zmotion.GetAxisMotionState(4); // 获取4轴状态
foreach (var state in motionStates)
{
    Console.WriteLine($"轴 {Array.IndexOf(motionStates, state)}:");
    Console.WriteLine($"  当前位置: {state.CurrentPosition}");
    Console.WriteLine($"  规划位置: {state.PlanPosition}");
    Console.WriteLine($"  当前速度: {state.CurrentSpeed}");
    Console.WriteLine($"  运行状态: {state.IsRunning}");
    Console.WriteLine($"  缓存数量: {state.MovesBuffered}");
    Console.WriteLine($"  轴状态: {state.Status}");
}

// 获取轴信号状态
var signal = zmotion.GetAxisSignal(0);
Console.WriteLine($"原点信号: {signal.HomeSignal}");
Console.WriteLine($"正限位: {signal.PositiveLimitSignal}");
Console.WriteLine($"负限位: {signal.NegativeLimitSignal}");
Console.WriteLine($"报警信号: {signal.AlarmSignal}");
```

### 5. 总线功能

```csharp
// 总线初始化
zmotion.Init_Bus();

// 检查初始化状态
bool initStatus = zmotion.GetBusInitStatus();

// 获取总线节点数量
int nodeCount = zmotion.GetBusNodeNum(0);

// 总线轴使能控制
zmotion.AxisEnable_Bus(0, true);   // 使能
zmotion.AxisEnable_Bus(0, false);  // 失能

// 清除总线驱动器报警
zmotion.ClearAlarm_Bus(0, 0);      // 清除当前告警
```

### 6. 高级功能

#### 电子齿轮
```csharp
// 设置电子齿轮比例
zmotion.Connect(0, 1, 2.0f);       // 轴0作为主轴，轴1随动，比例2:1

// 设置连接速率
zmotion.SetClutchRate(1, 1000000.0f);

// 设置编码器比例
zmotion.SetEncoderRatio(0, 1000, 1000);
```

#### 在线命令执行
```csharp
// 执行缓存命令
string result1 = zmotion.Execute_Buffer("DPOS(0)");

// 执行直接命令
string result2 = zmotion.Execute_Direct("?DPOS(0)");

// 程序下载
zmotion.BasDown(@"C:\Program\test.bas", BasDownMode.ROM);
```

### 设计模式

1. **分部类设计**: ZMotion类采用分部类设计，将不同功能模块分离
2. **策略模式**: 不同的运动模式和控制策略
3. **建造者模式**: ZMotionIOClient用于构建复杂的IO操作
4. **工厂模式**: 协议配置和数据映射的创建

## 🧪 测试与示例

项目包含 WPF 硬件测试应用程序，位于 `tests/ZMotionTest` 目录：

### 测试应用功能
- **连接管理**: TCP/IP连接配置和状态监控
- **轴控制界面**: 完整的单轴和多轴控制界面
- **IO控制面板**: 数字输入输出的实时控制和监控
- **参数测试**: 轴参数的配置和实时测试
- **缓存运动**: 缓存运动指令的管理和执行
- **协议测试**: IOSugar 数字 IO 协议测试

## 📈 性能优化

### 高性能特性
- **反射缓存**: 使用ConcurrentDictionary缓存反射操作，避免重复反射
- **批量操作**: 支持多轴、多IO的批量读写，减少通信开销
- **异步支持**: 关键API提供异步版本，避免UI线程阻塞
- **内存优化**: 优化P/Invoke调用，减少内存分配和GC压力

**注意**: 本SDK专为正运动ZMotion系列控制器设计，使用前请确认硬件兼容性。如有技术问题，请参考官方文档或联系技术支持。

## 📦 版本与发布

### 2.2.0

- 新增 `GetAI`、`GetAO` 和 `SetAO`，支持模拟量输入读取及输出读写。
- 补充模拟量调用示例、参数含义与量程说明，修正 IOSugar 示例。

### Trusted Publishing

发布工作流位于 [`.github/workflows/nuget.yml`](.github/workflows/nuget.yml)。推送与项目版本一致的 `v` 前缀标签（例如 `v2.2.0`）后，GitHub Actions 构建 NuGet 包，通过 `NuGet/login` 交换 OIDC 短期凭据并发布，无需保存长期 NuGet API Key。

首次发布前，NuGet 账号 `lejoyke` 的 Trusted Publishing 策略需配置为：

| 配置项 | 值 |
| --- | --- |
| GitHub 仓库所有者 | `lejoycoder` |
| 仓库 | `ZMotionSDK` |
| 工作流文件名 | `nuget.yml` |
| Environment | 留空（工作流未使用 GitHub Environment） |
| Scopes | `Push only new package versions` |
| Glob Patterns and Packages | `ZMotionSDK`（仅允许发布该包） |

更新项目文件中的 `Version` 和发布说明后，在仓库根目录执行：

```powershell
dotnet pack src/ZMotionSDK/ZMotionSDK.csproj -c Release -p:GeneratePackageOnBuild=false -o artifacts/nuget
git push origin main
git tag v2.2.0
git push origin v2.2.0
```

请在 GitHub Actions 中确认发布任务成功，再在 [NuGet](https://www.nuget.org/packages/ZMotionSDK) 检查版本是否已完成索引。若策略配置后需要重试，可重新运行失败的工作流。
