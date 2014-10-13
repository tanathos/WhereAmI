using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;

namespace WhereAmI.Options
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [CLSCompliant(false)]
    [ComVisible(true)]
    [Guid("5eca1e6a-08de-4b46-9f0d-cee9440c3ccc")]
    public class OptionsDialogPage : UIElementDialogPage
    {
        OptionsDialogPageControl optionsDialogControl;

        protected override UIElement Child
        {
            get { return optionsDialogControl ?? (optionsDialogControl = new OptionsDialogPageControl()); }
        }

        protected override void OnActivate(CancelEventArgs e)
        {
            base.OnActivate(e);

            
        }

        protected override void OnApply(PageApplyEventArgs args)
        {
            if (args.ApplyBehavior == ApplyKind.Apply)
            {
                
            }

            base.OnApply(args);
        }
    }
}