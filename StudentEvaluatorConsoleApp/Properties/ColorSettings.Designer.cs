﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34003
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Zcu.StudentEvaluator.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
    internal sealed partial class ColorSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static ColorSettings defaultInstance = ((ColorSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new ColorSettings())));
        
        public static ColorSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("DarkGreen")]
        public global::System.ConsoleColor MessageColor {
            get {
                return ((global::System.ConsoleColor)(this["MessageColor"]));
            }
            set {
                this["MessageColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Blue")]
        public global::System.ConsoleColor SelectionColor {
            get {
                return ((global::System.ConsoleColor)(this["SelectionColor"]));
            }
            set {
                this["SelectionColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("DarkYellow")]
        public global::System.ConsoleColor WarningColor {
            get {
                return ((global::System.ConsoleColor)(this["WarningColor"]));
            }
            set {
                this["WarningColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Black")]
        public global::System.ConsoleColor ErrorColor {
            get {
                return ((global::System.ConsoleColor)(this["ErrorColor"]));
            }
            set {
                this["ErrorColor"] = value;
            }
        }
    }
}