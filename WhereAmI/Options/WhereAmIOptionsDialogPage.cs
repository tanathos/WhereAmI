using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Recoding.WhereAmI;

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

            var componentModel = (IComponentModel)(Site.GetService(typeof(SComponentModel)));
            IWhereAmISettings settings = componentModel.DefaultExportProvider.GetExportedValue<IWhereAmISettings>();

            optionsDialogControl.txtColorFileName.Text = settings.FilenameColor;
            optionsDialogControl.txtFoldersColor.Text = settings.FoldersColor;
            optionsDialogControl.txtProjectColor.Text = settings.ProjectColor;
        }

        protected override void OnApply(PageApplyEventArgs args)
        {
            if (args.ApplyBehavior == ApplyKind.Apply)
            {
                var componentModel = (IComponentModel)(Site.GetService(typeof(SComponentModel)));
                IWhereAmISettings settings = componentModel.DefaultExportProvider.GetExportedValue<IWhereAmISettings>();

                settings.FilenameColor = optionsDialogControl.txtColorFileName.Text;

                settings.Store();
            }

            base.OnApply(args);
        }
    }
}