using System;
using Conscripts.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Conscripts.Views
{
    public sealed partial class RegistrationLayout : UserControl
    {
        private readonly MainViewModel _viewModel = null;

        private Action _closeRegistrationAction = null;

        public RegistrationLayout(MainViewModel viewModel, Action closeRegistrationAction)
        {
            _viewModel = viewModel;
            _closeRegistrationAction = closeRegistrationAction;

            this.InitializeComponent();

            this.Loaded += (_, _) =>
            {
                ResetLayout();
            };
        }

        /// <summary>
        /// 点击"开始使用"按钮，保存用户名并完成注册
        /// </summary>
        private void OnClickGetStarted(object sender, RoutedEventArgs e)
        {
            CompleteRegistration();
        }

        /// <summary>
        /// 按下回车键时完成注册
        /// </summary>
        private void RegistrationNameTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                CompleteRegistration();
            }
        }

        /// <summary>
        /// 跳过注册
        /// </summary>
        private void OnClickSkip(object sender, RoutedEventArgs e)
        {
            try
            {
                _viewModel.AppSettings.IsRegistered = true;
                _closeRegistrationAction?.Invoke();
            }
            catch (Exception ex) { System.Diagnostics.Trace.WriteLine(ex); }
        }

        /// <summary>
        /// 完成注册流程，保存用户名
        /// </summary>
        private void CompleteRegistration()
        {
            try
            {
                string name = RegistrationNameTextBox.Text?.Trim() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(name))
                {
                    RegistrationNameErrorTextBlock.Visibility = Visibility.Visible;
                    RegistrationNameTextBox.Focus(FocusState.Programmatic);
                    return;
                }

                RegistrationNameErrorTextBlock.Visibility = Visibility.Collapsed;
                _viewModel.AppSettings.UserName = name;
                _viewModel.AppSettings.IsRegistered = true;
                _closeRegistrationAction?.Invoke();
            }
            catch (Exception ex) { System.Diagnostics.Trace.WriteLine(ex); }
        }

        /// <summary>
        /// 重置布局
        /// </summary>
        public void ResetLayout()
        {
            try
            {
                RegistrationNameTextBox.Text = string.Empty;
                RegistrationNameErrorTextBlock.Visibility = Visibility.Collapsed;
                RegistrationScrollViewer.ChangeView(0, 0, null, true);
                RegistrationNameTextBox.Focus(FocusState.Programmatic);
            }
            catch (Exception ex) { System.Diagnostics.Trace.WriteLine(ex); }
        }
    }
}
