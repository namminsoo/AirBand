﻿#pragma checksum "C:\Users\SangWook\Documents\Visual Studio 2015\Projects\상욱2\Arduino2WP8\Arduino2WP8\Tutorial\Page3.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "5E1C6B4A4FA9640D080943DC61082F3B"
//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.42000
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace Arduino2WP8.Tutorial {
    
    
    public partial class Page3 : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBox emergencyNo1;
        
        internal System.Windows.Controls.TextBox emergencyNo2;
        
        internal System.Windows.Controls.TextBox emergencyNo3;
        
        internal System.Windows.Controls.Button searchNo;
        
        internal System.Windows.Controls.ListBox ContactResultsData;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton backBtn;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton saveBtn;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/Arduino2WP8;component/Tutorial/Page3.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.emergencyNo1 = ((System.Windows.Controls.TextBox)(this.FindName("emergencyNo1")));
            this.emergencyNo2 = ((System.Windows.Controls.TextBox)(this.FindName("emergencyNo2")));
            this.emergencyNo3 = ((System.Windows.Controls.TextBox)(this.FindName("emergencyNo3")));
            this.searchNo = ((System.Windows.Controls.Button)(this.FindName("searchNo")));
            this.ContactResultsData = ((System.Windows.Controls.ListBox)(this.FindName("ContactResultsData")));
            this.backBtn = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("backBtn")));
            this.saveBtn = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("saveBtn")));
        }
    }
}

