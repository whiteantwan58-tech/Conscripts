using System;
using System.Diagnostics;
using Conscripts.Helpers;
using Conscripts.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Windows.ApplicationModel;
using Windows.System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Conscripts.Views
{
    public sealed partial class SettingsLayout : UserControl
    {
        private readonly MainViewModel _viewModel = null;
        private readonly string _appVersion = string.Empty;

        public SettingsLayout(MainViewModel viewModel)
        {
            this.InitializeComponent();

            _viewModel = viewModel;
            _appVersion = $"v{GetAppVersion()}";
        }

        /// <summary>
        /// 获取版本号
        /// </summary>
        private string GetAppVersion()
        {
            try
            {
                Package package = Package.Current;
                PackageId packageId = package.Id;
                PackageVersion version = packageId.Version;
                return string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }

            return "";
        }

        /// <summary>
        /// 去商店评分
        /// </summary>
        private async void OnClickGoToStoreRate(object sender, RoutedEventArgs e)
        {
            try
            {
                await Launcher.LaunchUriAsync(new Uri($"ms-windows-store:REVIEW?PFN={Package.Current.Id.FamilyName}"));
            }
            catch (Exception ex) { System.Diagnostics.Trace.WriteLine(ex); }
        }

        /// <summary>
        /// 查看数据目录
        /// </summary>
        private async void OnClickDbPath(object sender, RoutedEventArgs e)
        {
            try
            {
                var folder = await StorageFilesService.GetDataFolder();
                await Launcher.LaunchFolderAsync(folder);
            }
            catch (Exception ex) { System.Diagnostics.Trace.WriteLine(ex); }
        }

        /// <summary>
        /// 打开 GitHub
        /// </summary>
        private async void OnClickGoGitHub(object sender, RoutedEventArgs e)
        {
            try
            {
                await Launcher.LaunchUriAsync(new Uri("https://github.com/sh0ckj0ckey/Conscripts"));
            }
            catch (Exception ex) { System.Diagnostics.Trace.WriteLine(ex); }
        }

        /// <summary>
        /// 编辑用户名
        /// </summary>
        private void OnClickEditProfile(object sender, RoutedEventArgs e)
        {
            try
            {
                SettingsProfileNameEditTextBox.Text = _viewModel.AppSettings.UserName;
                SettingsProfileEditGrid.Visibility = Visibility.Visible;
                SettingsProfileEditButton.Visibility = Visibility.Collapsed;
                SettingsProfileNameEditTextBox.Focus(FocusState.Programmatic);
                SettingsProfileNameEditTextBox.SelectAll();
            }
            catch (Exception ex) { System.Diagnostics.Trace.WriteLine(ex); }
        }

        /// <summary>
        /// 保存用户名
        /// </summary>
        private void OnClickSaveProfile(object sender, RoutedEventArgs e)
        {
            SaveProfile();
        }

        /// <summary>
        /// 在名称输入框中按下回车键时保存
        /// </summary>
        private void SettingsProfileNameEditTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                SaveProfile();
            }
            else if (e.Key == Windows.System.VirtualKey.Escape)
            {
                CancelEditProfile();
            }
        }

        /// <summary>
        /// 取消编辑用户名
        /// </summary>
        private void OnClickCancelEditProfile(object sender, RoutedEventArgs e)
        {
            CancelEditProfile();
        }

        /// <summary>
        /// 保存用户名到设置
        /// </summary>
        private void SaveProfile()
        {
            try
            {
                string name = SettingsProfileNameEditTextBox.Text?.Trim() ?? string.Empty;
                if (!string.IsNullOrWhiteSpace(name))
                {
                    _viewModel.AppSettings.UserName = name;
                }
                CancelEditProfile();
            }
            catch (Exception ex) { System.Diagnostics.Trace.WriteLine(ex); }
        }

        /// <summary>
        /// 取消并隐藏编辑区域
        /// </summary>
        private void CancelEditProfile()
        {
            try
            {
                SettingsProfileEditGrid.Visibility = Visibility.Collapsed;
                SettingsProfileEditButton.Visibility = Visibility.Visible;
            }
            catch (Exception ex) { System.Diagnostics.Trace.WriteLine(ex); }
        }

        /// <summary>
        /// 重置UI
        /// </summary>
        public void ResetLayout()
        {
            try
            {
                CancelEditProfile();
                SettingsScrollViewer.ChangeView(0, 0, null, true);
            }
            catch (Exception ex) { System.Diagnostics.Trace.WriteLine(ex); }
        }
    }
}
