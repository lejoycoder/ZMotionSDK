using System.Windows.Media;
using MaterialDesignThemes.Wpf;

namespace ZMotionTest.Models;

public enum UiStatusLevel
{
    Info,
    Success,
    Warning,
    Error
}

public sealed class UiConnectionStateModel
{
    public string Text { get; init; } = "未连接";
    public PackIconKind IconKind { get; init; } = PackIconKind.CloseNetworkOutline;
    public Brush Brush { get; init; } = Brushes.IndianRed;

    public static UiConnectionStateModel Connected() => new()
    {
        Text = "已连接",
        IconKind = PackIconKind.Connection,
        Brush = Brushes.MediumSeaGreen
    };

    public static UiConnectionStateModel Disconnected() => new()
    {
        Text = "未连接",
        IconKind = PackIconKind.CloseNetworkOutline,
        Brush = Brushes.IndianRed
    };
}

public sealed class UiOperationStateModel
{
    public UiStatusLevel Level { get; init; } = UiStatusLevel.Info;
    public string Message { get; init; } = "就绪";

    public static UiOperationStateModel Create(UiStatusLevel level, string message) => new()
    {
        Level = level,
        Message = message
    };
}
