using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

using DevExpress.ExpressApp;

namespace WinSample.Module.Win {
    [ToolboxItemFilter("Xaf.Platform.Win")]
    public sealed partial class WinSampleWindowsFormsModule : ModuleBase {
        public WinSampleWindowsFormsModule() {
            InitializeComponent();
        }
    }
}
