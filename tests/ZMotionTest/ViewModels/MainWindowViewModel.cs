using System;
using System.Windows.Media;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using MaterialDesignThemes.Wpf;
using ZMotionTest.Models;
using ZMotionTest.Services;

namespace ZMotionTest.ViewModels;

/// <summary>
/// 主窗口ViewModel
/// </summary>
public partial class MainWindowViewModel : ObservableObject
{
    private readonly ZMotionManager _zMotionManager;

    public MainWindowViewModel()
    {
        _zMotionManager = ZMotionManager.Instance;
        StartTimeUpdater();
        UpdateConnectionStatus(_zMotionManager.IsConnected);
    }

    [ObservableProperty]
    private UiConnectionStateModel connectionStateModel = UiConnectionStateModel.Disconnected();

    [ObservableProperty]
    private UiOperationStateModel operationStateModel = UiOperationStateModel.Create(UiStatusLevel.Info, "就绪");

    [ObservableProperty]
    private string currentPageTitle = "连接管理";

    [ObservableProperty]
    private string connectionStatus = "未连接";

    [ObservableProperty]
    private PackIconKind connectionIconKind = PackIconKind.CloseNetworkOutline;

    [ObservableProperty]
    private Brush connectionStatusColor = Brushes.IndianRed;

    [ObservableProperty]
    private Brush connectionIconColor = Brushes.IndianRed;

    [ObservableProperty]
    private string statusText = "就绪";

    [ObservableProperty]
    private string timeText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

    public void UpdateConnectionStatus(bool isConnected)
    {
        ConnectionStateModel = isConnected ? UiConnectionStateModel.Connected() : UiConnectionStateModel.Disconnected();

        ConnectionStatus = ConnectionStateModel.Text;
        ConnectionIconKind = ConnectionStateModel.IconKind;
        ConnectionStatusColor = ConnectionStateModel.Brush;
        ConnectionIconColor = ConnectionStateModel.Brush;
    }

    public void UpdateStatus(string status)
    {
        OperationStateModel = UiOperationStateModel.Create(UiStatusLevel.Info, status);
        StatusText = OperationStateModel.Message;
    }

    private void StartTimeUpdater()
    {
        var timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };

        timer.Tick += (_, _) => TimeText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        timer.Start();
    }
}
