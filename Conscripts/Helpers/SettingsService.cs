using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Windows.Storage;

namespace Conscripts.Helpers
{
    public class SettingsService : ObservableObject
    {
        private const string SETTING_NAME_APPEARANCEINDEX = "AppearanceIndex";
        private const string SETTING_NAME_BACKDROPINDEX = "BackdropIndex";
        private const string SETTING_NAME_ONESHOTMODE = "IsOneShotModeEnabled";
        private const string SETTING_NAME_USERNAME = "UserName";
        private const string SETTING_NAME_ISREGISTERED = "IsRegistered";

        private ApplicationDataContainer _localSettings = ApplicationData.Current.LocalSettings;

        public Action<int> OnAppearanceSettingChanged { get; set; } = null;
        public Action<int> OnBackdropSettingChanged { get; set; } = null;

        private int _appearanceIndex = -1;

        private int _backdropIndex = -1;

        private bool? _oneShotEnabled = null;

        private string _userName = null;

        private bool? _isRegistered = null;

        /// <summary>
        /// 设置的应用程序的主题 0-System 1-Dark 2-Light
        /// </summary>
        public int AppearanceIndex
        {
            get
            {
                try
                {
                    if (_appearanceIndex < 0)
                    {
                        if (_localSettings.Values[SETTING_NAME_APPEARANCEINDEX] == null)
                        {
                            _appearanceIndex = 0;
                        }
                        else if (_localSettings.Values[SETTING_NAME_APPEARANCEINDEX]?.ToString() == "0")
                        {
                            _appearanceIndex = 0;
                        }
                        else if (_localSettings.Values[SETTING_NAME_APPEARANCEINDEX]?.ToString() == "1")
                        {
                            _appearanceIndex = 1;
                        }
                        else if (_localSettings.Values[SETTING_NAME_APPEARANCEINDEX]?.ToString() == "2")
                        {
                            _appearanceIndex = 2;
                        }
                        else
                        {
                            _appearanceIndex = 0;
                        }
                    }
                }
                catch (Exception ex) { System.Diagnostics.Trace.WriteLine(ex); }
                if (_appearanceIndex < 0) _appearanceIndex = 0;
                return _appearanceIndex < 0 ? 0 : _appearanceIndex;
            }
            set
            {
                SetProperty(ref _appearanceIndex, value);
                ApplicationData.Current.LocalSettings.Values[SETTING_NAME_APPEARANCEINDEX] = _appearanceIndex;
                OnAppearanceSettingChanged?.Invoke(_appearanceIndex);
            }
        }

        /// <summary>
        /// 设置的应用程序的背景材质 0-Mica 1-Acrylic
        /// </summary>
        public int BackdropIndex
        {
            get
            {
                try
                {
                    if (_backdropIndex < 0)
                    {
                        if (_localSettings.Values[SETTING_NAME_BACKDROPINDEX] == null)
                        {
                            _backdropIndex = 0;
                        }
                        else if (_localSettings.Values[SETTING_NAME_BACKDROPINDEX]?.ToString() == "0")
                        {
                            _backdropIndex = 0;
                        }
                        else if (_localSettings.Values[SETTING_NAME_BACKDROPINDEX]?.ToString() == "1")
                        {
                            _backdropIndex = 1;
                        }
                        else
                        {
                            _backdropIndex = 0;
                        }
                    }
                }
                catch (Exception ex) { System.Diagnostics.Trace.WriteLine(ex); }
                if (_backdropIndex < 0) _backdropIndex = 0;
                return _backdropIndex < 0 ? 0 : _backdropIndex;
            }
            set
            {
                SetProperty(ref _backdropIndex, value);
                ApplicationData.Current.LocalSettings.Values[SETTING_NAME_BACKDROPINDEX] = _backdropIndex;
                OnBackdropSettingChanged?.Invoke(_backdropIndex);
            }
        }

        /// <summary>
        /// 是否启用一次性模式
        /// </summary>
        public bool OneShotEnabled
        {
            get
            {
                try
                {
                    if (_oneShotEnabled is null)
                    {
                        if (_localSettings.Values[SETTING_NAME_ONESHOTMODE] == null)
                        {
                            _oneShotEnabled = false;
                        }
                        else if (_localSettings.Values[SETTING_NAME_ONESHOTMODE]?.ToString() == "True")
                        {
                            _oneShotEnabled = true;
                        }
                        else
                        {
                            _oneShotEnabled = false;
                        }
                    }
                }
                catch (Exception ex) { System.Diagnostics.Trace.WriteLine(ex); }
                _oneShotEnabled ??= false;
                return _oneShotEnabled ?? false;
            }
            set
            {
                SetProperty(ref _oneShotEnabled, value);
                ApplicationData.Current.LocalSettings.Values[SETTING_NAME_ONESHOTMODE] = _oneShotEnabled;
            }
        }

        /// <summary>
        /// 用户注册的显示名称
        /// </summary>
        public string UserName
        {
            get
            {
                try
                {
                    if (_userName is null)
                    {
                        _userName = _localSettings.Values[SETTING_NAME_USERNAME]?.ToString() ?? string.Empty;
                    }
                }
                catch (Exception ex) { System.Diagnostics.Trace.WriteLine(ex); }
                return _userName ?? string.Empty;
            }
            set
            {
                SetProperty(ref _userName, value ?? string.Empty);
                ApplicationData.Current.LocalSettings.Values[SETTING_NAME_USERNAME] = _userName;
            }
        }

        /// <summary>
        /// 是否已完成用户注册
        /// </summary>
        public bool IsRegistered
        {
            get
            {
                try
                {
                    if (_isRegistered is null)
                    {
                        if (_localSettings.Values[SETTING_NAME_ISREGISTERED]?.ToString() == "True")
                        {
                            _isRegistered = true;
                        }
                        else
                        {
                            _isRegistered = false;
                        }
                    }
                }
                catch (Exception ex) { System.Diagnostics.Trace.WriteLine(ex); }
                return _isRegistered ?? false;
            }
            set
            {
                SetProperty(ref _isRegistered, value);
                ApplicationData.Current.LocalSettings.Values[SETTING_NAME_ISREGISTERED] = _isRegistered;
            }
        }
    }

}
