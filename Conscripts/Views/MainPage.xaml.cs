using System;
using System.IO;
using Conscripts.Helpers;
using Conscripts.Models;
using Conscripts.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Conscripts.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MainViewModel _viewModel = null;

        private AddingLayout _addingLayout = null;

        private WhatsNewLayout _whatsNewLayout = null;

        private SettingsLayout _settingsLayout = null;

        private PropertyLayout _propertyLayout = null;

        private RegistrationLayout _registrationLayout = null;

        public MainPage()
        {
            _viewModel = MainViewModel.Instance;

            this.InitializeComponent();

            _viewModel.DispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();

            this.Loaded += OnPageLoaded;
        }

        /// <summary>
        /// 页面加载后，如果用户尚未注册则显示注册界面
        /// </summary>
        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            if (!_viewModel.AppSettings.IsRegistered)
            {
                ShowRegistrationLayout();
            }
        }

        /// <summary>
        /// 显示注册界面
        /// </summary>
        private void ShowRegistrationLayout()
        {
            RegistrationGrid.Visibility = Visibility.Visible;
            RegistrationBorder.Child ??= _registrationLayout = new RegistrationLayout(_viewModel, CloseRegistrationLayout);
        }

        /// <summary>
        /// 关闭注册界面
        /// </summary>
        private void CloseRegistrationLayout()
        {
            RegistrationGrid.Visibility = Visibility.Collapsed;
            _registrationLayout?.ResetLayout();
        }

        /// <summary>
        /// �����������Ӧ�Ľű����߹�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is ShortcutModel shortcut)
            {
                if (shortcut.ShortcutType == ShortcutTypeEnum.None)
                {
                    if (shortcut.Category == "add")
                    {
                        AddingGrid.Visibility = Visibility.Visible;
                        AddingBorder.Child ??= _addingLayout = new AddingLayout(_viewModel, CloseAddingLayout);
                    }
                    else if (shortcut.Category == "whatsnew")
                    {
                        WhatsNewGrid.Visibility = Visibility.Visible;
                        WhatsNewBorder.Child ??= _whatsNewLayout = new WhatsNewLayout(_viewModel);
                    }
                    else if (shortcut.Category == "settings")
                    {
                        SettingsGrid.Visibility = Visibility.Visible;
                        SettingsBorder.Child ??= _settingsLayout = new SettingsLayout(_viewModel);
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(shortcut.ScriptFilePath) ||
                        !File.Exists(shortcut.ScriptFilePath))
                    {
                        await new ContentDialog
                        {
                            Title = "DialogTitleCannotLaunch".GetLocalized(),
                            Content = $"{"DialogContentCannotLaunch".GetLocalized()} {shortcut.ScriptFilePath}",
                            CloseButtonText = "DialogButtonGotIt".GetLocalized(),
                            XamlRoot = this.XamlRoot,
                            RequestedTheme = this.ActualTheme,
                        }.ShowAsync();
                    }
                    else
                    {
                        _viewModel.LaunchShortcut(shortcut);
                    }
                }
            }
        }

        /// <summary>
        /// ���ֱ�Ӹ�Button������ContextFlyout�Ҽ��˵����򲻻ᴥ������¼�
        /// ���Ҫʹ����Դ�ֵ�ķ�ʽ�������Ҽ��˵���������¼����洦������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Button_ContextRequested(UIElement sender, ContextRequestedEventArgs args)
        {
            if (sender is Button btn && btn.DataContext is ShortcutModel shortcut)
            {
                if (shortcut.ShortcutType == ShortcutTypeEnum.None)
                {
                    args.Handled = true;
                }
                else
                {
                    if (shortcut.Running)
                    {
                        args.Handled = true;
                    }
                    else
                    {
                        MenuFlyout flyout = (MenuFlyout)btn.Resources["ShortcutMenuFlyout"];
                        flyout.ShowAt(btn);
                    }
                }
            }
        }

        /// <summary>
        /// ���ű�������ǰ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrontMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem menuItem && menuItem.DataContext is ShortcutModel shortcut)
            {
                _viewModel.PlaceShortcutFront(shortcut);
            }
        }

        /// <summary>
        /// �鿴�ű���Ϣ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InfoMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem menuItem && menuItem.DataContext is ShortcutModel shortcut)
            {
                PropertyGrid.Visibility = Visibility.Visible;
                PropertyBorder.Child ??= _propertyLayout = new PropertyLayout(_viewModel, ClosePropertyLayout);
                _propertyLayout.SetLayout(shortcut);
            }
        }

        /// <summary>
        /// ɾ���ű���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem menuItem && menuItem.DataContext is ShortcutModel shortcut)
            {
                ContentDialogResult result = await new ContentDialog
                {
                    Title = "DialogTitleDeleteScript".GetLocalized(),
                    Content = $"{"DialogContentDeleteConfirm1".GetLocalized()} \"{shortcut?.ShortcutName}\" {"DialogContentDeleteConfirm2".GetLocalized()}",
                    PrimaryButtonText = "DialogButtonDelete".GetLocalized(),
                    CloseButtonText = "DialogButtonCancel".GetLocalized(),
                    XamlRoot = this.XamlRoot,
                    RequestedTheme = this.ActualTheme,
                    DefaultButton = ContentDialogButton.Close
                }.ShowAsync();

                if (result == ContentDialogResult.Primary)
                {
                    _viewModel.DeleteShortcut(shortcut);
                }
            }
        }


        private void CloseSettings_Click(object sender, RoutedEventArgs e)
        {
            CloseSettingsLayout();
        }

        private void CloseWhatsNew_Click(object sender, RoutedEventArgs e)
        {
            CloseWhatsNewLayout();
        }

        private void CloseAdding_Click(object sender, RoutedEventArgs e)
        {
            CloseAddingLayout();
        }

        private void CloseProperty_Click(object sender, RoutedEventArgs e)
        {
            ClosePropertyLayout();
        }

        private void CloseSettingsLayout()
        {
            SettingsGrid.Visibility = Visibility.Collapsed;
            _settingsLayout?.ResetLayout();
        }

        private void CloseWhatsNewLayout()
        {
            WhatsNewGrid.Visibility = Visibility.Collapsed;
            _whatsNewLayout?.ResetLayout();
        }

        private void CloseAddingLayout()
        {
            AddingGrid.Visibility = Visibility.Collapsed;
            _addingLayout?.ResetLayout();
        }

        private void ClosePropertyLayout()
        {
            PropertyGrid.Visibility = Visibility.Collapsed;
            _propertyLayout?.ResetLayout();
        }

    }
}
