﻿#pragma checksum "C:\Users\gamin\OneDrive - Tomkovic\Backup\PC\Dokumenti\Works\Pronder\Pronder\Views\InbuildProjectDisplayPages\GeneralProjectDisplayPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "EA0DFD95718D8AE2983A54B43C3913FC33477C9E3F0BCF230650C650B92B129C"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Pronder.Views
{
    partial class GeneralProjectDisplayPage : 
        global::Microsoft.UI.Xaml.Controls.Page, 
        global::Microsoft.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 3.0.0.2406")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private static class XamlBindingSetters
        {
            public static void Set_Microsoft_UI_Xaml_Controls_TeachingTip_Target(global::Microsoft.UI.Xaml.Controls.TeachingTip obj, global::Microsoft.UI.Xaml.FrameworkElement value, string targetNullValue)
            {
                if (value == null && targetNullValue != null)
                {
                    value = (global::Microsoft.UI.Xaml.FrameworkElement) global::Microsoft.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(global::Microsoft.UI.Xaml.FrameworkElement), targetNullValue);
                }
                obj.Target = value;
            }
        };

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 3.0.0.2406")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private class GeneralProjectDisplayPage_obj1_Bindings :
            global::Microsoft.UI.Xaml.Markup.IComponentConnector,
            IGeneralProjectDisplayPage_Bindings
        {
            private global::Pronder.Views.GeneralProjectDisplayPage dataRoot;
            private bool initialized = false;
            private const int NOT_PHASED = (1 << 31);
            private const int DATA_CHANGED = (1 << 30);

            // Fields for each control that has bindings.
            private global::Microsoft.UI.Xaml.Controls.TeachingTip obj6;

            public GeneralProjectDisplayPage_obj1_Bindings()
            {
            }

            // IComponentConnector

            public void Connect(int connectionId, global::System.Object target)
            {
                switch(connectionId)
                {
                    case 6: // Views\InbuildProjectDisplayPages\GeneralProjectDisplayPage.xaml line 143
                        this.obj6 = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TeachingTip>(target);
                        break;
                    default:
                        break;
                }
            }
                        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 3.0.0.2406")]
                        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                        public global::Microsoft.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target) 
                        {
                            return null;
                        }

            // IGeneralProjectDisplayPage_Bindings

            public void Initialize()
            {
                if (!this.initialized)
                {
                    this.Update();
                }
            }
            
            public void Update()
            {
                this.Update_(this.dataRoot, NOT_PHASED);
                this.initialized = true;
            }

            public void StopTracking()
            {
            }

            public void DisconnectUnloadedObject(int connectionId)
            {
                throw new global::System.ArgumentException("No unloadable elements to disconnect.");
            }

            public bool SetDataRoot(global::System.Object newDataRoot)
            {
                if (newDataRoot != null)
                {
                    this.dataRoot = global::WinRT.CastExtensions.As<global::Pronder.Views.GeneralProjectDisplayPage>(newDataRoot);
                    return true;
                }
                return false;
            }

            public void Activated(object obj, global::Microsoft.UI.Xaml.WindowActivatedEventArgs data)
            {
                this.Initialize();
            }

            public void Loading(global::Microsoft.UI.Xaml.FrameworkElement src, object data)
            {
                this.Initialize();
            }

            // Update methods for each path node used in binding steps.
            private void Update_(global::Pronder.Views.GeneralProjectDisplayPage obj, int phase)
            {
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | (1 << 0))) != 0)
                    {
                        this.Update_row(obj.row, phase);
                    }
                }
            }
            private void Update_row(global::Microsoft.UI.Xaml.Controls.Button obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED )) != 0)
                {
                    // Views\InbuildProjectDisplayPages\GeneralProjectDisplayPage.xaml line 143
                    XamlBindingSetters.Set_Microsoft_UI_Xaml_Controls_TeachingTip_Target(this.obj6, obj, null);
                }
            }
        }

        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 3.0.0.2406")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // Views\InbuildProjectDisplayPages\GeneralProjectDisplayPage.xaml line 11
                {
                    this.ProjectBannerParent = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Border>(target);
                }
                break;
            case 3: // Views\InbuildProjectDisplayPages\GeneralProjectDisplayPage.xaml line 26
                {
                    this.ProjectBannerAfter = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Grid>(target);
                }
                break;
            case 4: // Views\InbuildProjectDisplayPages\GeneralProjectDisplayPage.xaml line 110
                {
                    global::Microsoft.UI.Xaml.Controls.SelectorBar element4 = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.SelectorBar>(target);
                    ((global::Microsoft.UI.Xaml.Controls.SelectorBar)element4).SelectionChanged += this.TabSwitch;
                }
                break;
            case 5: // Views\InbuildProjectDisplayPages\GeneralProjectDisplayPage.xaml line 131
                {
                    this.ContentFrame = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Frame>(target);
                }
                break;
            case 6: // Views\InbuildProjectDisplayPages\GeneralProjectDisplayPage.xaml line 143
                {
                    this.ToggleThemeTeachingTip1 = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TeachingTip>(target);
                }
                break;
            case 7: // Views\InbuildProjectDisplayPages\GeneralProjectDisplayPage.xaml line 118
                {
                    this.SubPageTabBarFirst = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.SelectorBarItem>(target);
                }
                break;
            case 8: // Views\InbuildProjectDisplayPages\GeneralProjectDisplayPage.xaml line 56
                {
                    this.ProjectExternalLinks = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Button>(target);
                }
                break;
            case 9: // Views\InbuildProjectDisplayPages\GeneralProjectDisplayPage.xaml line 76
                {
                    this.row = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Button>(target);
                }
                break;
            case 10: // Views\InbuildProjectDisplayPages\GeneralProjectDisplayPage.xaml line 84
                {
                    global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem element10 = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem>(target);
                    ((global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem)element10).Click += this.editData;
                }
                break;
            case 11: // Views\InbuildProjectDisplayPages\GeneralProjectDisplayPage.xaml line 99
                {
                    global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem element11 = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem>(target);
                    ((global::Microsoft.UI.Xaml.Controls.MenuFlyoutItem)element11).Click += this.deleteProject;
                }
                break;
            case 12: // Views\InbuildProjectDisplayPages\GeneralProjectDisplayPage.xaml line 69
                {
                    this.ProjectExternalLinksInsert = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.MenuFlyout>(target);
                }
                break;
            case 13: // Views\InbuildProjectDisplayPages\GeneralProjectDisplayPage.xaml line 50
                {
                    this.ProjectTitle = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBlock>(target);
                    ((global::Microsoft.UI.Xaml.Controls.TextBlock)this.ProjectTitle).Loaded += this.importData;
                }
                break;
            case 14: // Views\InbuildProjectDisplayPages\GeneralProjectDisplayPage.xaml line 51
                {
                    this.ProjectTag = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBlock>(target);
                }
                break;
            case 15: // Views\InbuildProjectDisplayPages\GeneralProjectDisplayPage.xaml line 42
                {
                    this.ProjectIcon = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Image>(target);
                    ((global::Microsoft.UI.Xaml.Controls.Image)this.ProjectIcon).Loaded += this.insertIcon;
                }
                break;
            case 16: // Views\InbuildProjectDisplayPages\GeneralProjectDisplayPage.xaml line 19
                {
                    this.ProjectBanner = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Image>(target);
                    ((global::Microsoft.UI.Xaml.Controls.Image)this.ProjectBanner).Loaded += this.insertBanner;
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 3.0.0.2406")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Microsoft.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Microsoft.UI.Xaml.Markup.IComponentConnector returnValue = null;
            switch(connectionId)
            {
            case 1: // Views\InbuildProjectDisplayPages\GeneralProjectDisplayPage.xaml line 1
                {                    
                    global::Microsoft.UI.Xaml.Controls.Page element1 = (global::Microsoft.UI.Xaml.Controls.Page)target;
                    GeneralProjectDisplayPage_obj1_Bindings bindings = new GeneralProjectDisplayPage_obj1_Bindings();
                    returnValue = bindings;
                    bindings.SetDataRoot(this);
                    this.Bindings = bindings;
                    element1.Loading += bindings.Loading;
                }
                break;
            }
            return returnValue;
        }
    }
}

