﻿#pragma checksum "..\..\CreateLinkWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "2384B02567E266DCF656C1241875AE2128BE38629B9E788CA17CE8F93A336FA9"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using GraphVisual;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace GraphVisual {
    
    
    /// <summary>
    /// CreateLinkWindow
    /// </summary>
    public partial class CreateLinkWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\CreateLinkWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox textBox;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\CreateLinkWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox textBox2;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\CreateLinkWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox comboBox1;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\CreateLinkWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox comboBox2;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/GraphVisual;component/createlinkwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\CreateLinkWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 8 "..\..\CreateLinkWindow.xaml"
            ((GraphVisual.CreateLinkWindow)(target)).KeyDown += new System.Windows.Input.KeyEventHandler(this.Window_KeyDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.textBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            
            #line 12 "..\..\CreateLinkWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Ok_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 13 "..\..\CreateLinkWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Cancel_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.textBox2 = ((System.Windows.Controls.TextBox)(target));
            
            #line 15 "..\..\CreateLinkWindow.xaml"
            this.textBox2.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.textBox2_TextChanged);
            
            #line default
            #line hidden
            return;
            case 6:
            this.comboBox1 = ((System.Windows.Controls.ComboBox)(target));
            
            #line 16 "..\..\CreateLinkWindow.xaml"
            this.comboBox1.Loaded += new System.Windows.RoutedEventHandler(this.ComboBox_Loaded);
            
            #line default
            #line hidden
            return;
            case 7:
            this.comboBox2 = ((System.Windows.Controls.ComboBox)(target));
            
            #line 17 "..\..\CreateLinkWindow.xaml"
            this.comboBox2.Loaded += new System.Windows.RoutedEventHandler(this.ComboBox_Loaded);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
