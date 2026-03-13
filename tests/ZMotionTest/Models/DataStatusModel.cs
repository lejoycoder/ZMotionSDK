using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ZMotionTest.Models;

/// <summary>
/// 协议位状态模型，支持显示与按位写入命令。
/// </summary>
public partial class DataStatusModel<TAddress> : ObservableObject
{
    [ObservableProperty] private string name = string.Empty;

    [ObservableProperty] private TAddress? address;

    [ObservableProperty] private bool status;

    [ObservableProperty] private bool visibility = true;

    public ICommand? Command { get; set; }

    public string DisplayAddress => Address?.ToString() ?? "-";

    public string StatusText => Status ? "ON" : "OFF";

    public string ActionText => Status ? "置0" : "置1";

    partial void OnAddressChanged(TAddress? value)
    {
        OnPropertyChanged(nameof(DisplayAddress));
    }

    partial void OnStatusChanged(bool value)
    {
        OnPropertyChanged(nameof(StatusText));
        OnPropertyChanged(nameof(ActionText));
    }
}
