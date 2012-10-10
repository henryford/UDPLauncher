namespace UDPLauncher.Properties
{
    using System;
    using System.CodeDom.Compiler;
    using System.Configuration;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [CompilerGenerated, GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed class Settings : ApplicationSettingsBase
    {
        private static Settings defaultInstance = ((Settings) SettingsBase.Synchronized(new Settings()));

        public static Settings Default
        {
            get
            {
                return defaultInstance;
            }
        }

        [DebuggerNonUserCode, ApplicationScopedSetting, DefaultSettingValue("False")]
        public bool ExitIfOpen
        {
            get
            {
                return (bool) this["ExitIfOpen"];
            }
        }

        [ApplicationScopedSetting, DebuggerNonUserCode, DefaultSettingValue("9")]
        public int PortToListenTo
        {
            get
            {
                return (int) this["PortToListenTo"];
            }
        }

        [ApplicationScopedSetting, DebuggerNonUserCode, DefaultSettingValue(@"C:\Program Files (x86)\XBMC\XBMC.exe")]
        public string ProgramToLaunch
        {
            get
            {
                return (string) this["ProgramToLaunch"];
            }
        }

        [DefaultSettingValue("True"), DebuggerNonUserCode, ApplicationScopedSetting]
        public bool UseXBMCEvent
        {
            get
            {
                return (bool) this["UseXBMCEvent"];
            }
        }

        [DefaultSettingValue(""), DebuggerNonUserCode, ApplicationScopedSetting]
        public string XBMC_Password
        {
            get
            {
                return (string) this["XBMC_Password"];
            }
        }

        [DebuggerNonUserCode, DefaultSettingValue("8181"), ApplicationScopedSetting]
        public int XBMC_Port
        {
            get
            {
                return (int) this["XBMC_Port"];
            }
        }

        [DefaultSettingValue("localhost"), ApplicationScopedSetting, DebuggerNonUserCode]
        public string XBMC_Server
        {
            get
            {
                return (string) this["XBMC_Server"];
            }
        }

        [ApplicationScopedSetting, DebuggerNonUserCode, DefaultSettingValue("")]
        public string XBMC_Username
        {
            get
            {
                return (string) this["XBMC_Username"];
            }
        }

        [DefaultSettingValue("ExecBuiltIn(ActivateWindow(VideoLibrary,TvShowTitles))"), ApplicationScopedSetting, DebuggerNonUserCode]
        public string XBMCEvent
        {
            get
            {
                return (string) this["XBMCEvent"];
            }
        }
    }
}

