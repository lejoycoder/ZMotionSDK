using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ZMotionTest.Pages;
using ZMotionTest.ViewModels;

namespace ZMotionTest;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public static MainWindow Instance { get; private set; } = null!;
    public MainWindowViewModel ViewModel { get; private set; } = null!;

    private readonly Dictionary<string, Button> _navigationButtons;

    public MainWindow()
    {
        Instance = this;
        InitializeComponent();
        ViewModel = new MainWindowViewModel();
        DataContext = ViewModel;

        _navigationButtons = new Dictionary<string, Button>
        {
            ["Connection"] = ConnectionButton,
            ["AxisMonitor"] = AxisMonitorButton,
            ["AxisControl"] = AxisControlButton,
            ["IOControl"] = IOControlButton,
            ["ParameterTest"] = ParameterTestButton,
            ["MotionBuffer"] = MotionBufferButton
        };

        NavigateToConnectionPage();
    }

    private void NavigateToPage(object sender, RoutedEventArgs e)
    {
        if (sender is not Button button || button.Tag is not string pageTag)
        {
            return;
        }

        switch (pageTag)
        {
            case "Connection":
                NavigateToConnectionPage();
                break;
            case "AxisMonitor":
                NavigateToAxisMonitorPage();
                break;
            case "AxisControl":
                NavigateToAxisControlPage();
                break;
            case "IOControl":
                NavigateToIOControlPage();
                break;
            case "ParameterTest":
                NavigateToParameterTestPage();
                break;
            case "MotionBuffer":
                NavigateToMotionBufferPage();
                break;
        }
    }

    private void SetActiveNavigation(string pageTag, string pageTitle)
    {
        foreach (var pair in _navigationButtons)
        {
            pair.Value.Style = (Style)FindResource(
                pair.Key == pageTag ? "NavigationButtonActiveStyle" : "NavigationButtonStyle");
        }

        ViewModel.CurrentPageTitle = pageTitle;
    }

    private void NavigateToConnectionPage()
    {
        ContentFrame.Navigate(new ConnectionPage());
        SetActiveNavigation("Connection", "连接管理");
    }

    private void NavigateToAxisMonitorPage()
    {
        ContentFrame.Navigate(new AxisMonitorPage());
        SetActiveNavigation("AxisMonitor", "轴状态监控");
    }

    private void NavigateToAxisControlPage()
    {
        ContentFrame.Navigate(new AxisControlPage());
        SetActiveNavigation("AxisControl", "轴运动控制");
    }

    private void NavigateToIOControlPage()
    {
        ContentFrame.Navigate(new IOControlPage());
        SetActiveNavigation("IOControl", "IO 控制");
    }

    private void NavigateToParameterTestPage()
    {
        ContentFrame.Navigate(new ParameterTestPage());
        SetActiveNavigation("ParameterTest", "参数测试");
    }

    private void NavigateToMotionBufferPage()
    {
        ContentFrame.Navigate(new MotionBufferPage());
        SetActiveNavigation("MotionBuffer", "运动缓存测试");
    }
}
