using System.Reflection;
using ZMotionSDK;
using ZMotionSDK.Helper;

// 无需控制器：只验证应在原生调用前结束的分支。
var motion = new ZMotion();
foreach (var index in new[] { -1, -2, int.MinValue })
{
    ExpectInvalidIndex(() => motion.GetDI(index), index);
}

var signals = new (string Cache, Func<int, bool> Read, SignalType Type)[]
{
    ("_signalIndex_Homes", motion.GetDI_Home, SignalType.HomeSignal),
    ("_signalIndex_ForwardLimits", motion.GetDI_ForwardLimit, SignalType.PositiveLimit),
    ("_signalIndex_BackwardLimits", motion.GetDI_BackwardLimit, SignalType.NegativeLimit)
};

foreach (var (cacheName, read, type) in signals)
{
    // 预填控制器索引缓存，避免为配置测试数据访问硬件。
    var cache = (Dictionary<int, int>)typeof(ZMotion)
        .GetField(cacheName, BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(motion)!;
    cache[0] = -1;
    foreach (var inverted in new[] { false, true })
    {
        var config = new DIDOInverterConfig();
        if (inverted)
            config.SetInverted(type, 0);
        motion.ApplyDIDOInverter(config);
        if (read(0))
            throw new Exception($"{cacheName}: 未配置的信号必须返回false，反转={inverted}。");
    }

    cache[0] = -2;
    ExpectInvalidIndex(() => read(0), -2);
}

Console.WriteLine("PASS: GetDI负数校验；三种轴信号未配置时跳过读取与反转；其他负数报错。");

static void ExpectInvalidIndex(Func<bool> read, int index)
{
    try
    {
        read();
    }
    catch (ArgumentOutOfRangeException ex) when (
        ex.ParamName == "index" && Equals(ex.ActualValue, index) &&
        ex.Message.Contains("DI输入索引不能为负数"))
    {
        return;
    }

    throw new Exception($"索引{index}应抛出包含参数名、实际值和明确提示的异常。");
}
