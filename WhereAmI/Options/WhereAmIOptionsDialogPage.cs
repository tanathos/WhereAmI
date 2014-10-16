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

        IWhereAmISettings settings 
        {
            get 
            {
                var componentModel = (IComponentModel)(Site.GetService(typeof(SComponentModel)));
                IWhereAmISettings s = componentModel.DefaultExportProvider.GetExportedValue<IWhereAmISettings>();

                return s;
            }
        }

        protected override UIElement Child
        {
            get 
            {
                return optionsDialogControl ?? (optionsDialogControl = new OptionsDialogPageControl(this)); 
            }
        }

        private void BindSettings() 
        {
            optionsDialogControl.txtColorFileName.Text = settings.FilenameColor;
            optionsDialogControl.txtFoldersColor.Text = settings.FoldersColor;
            optionsDialogControl.txtProjectColor.Text = settings.ProjectColor;

            optionsDialogControl.txtFilenameSize.Text = settings.FilenameSize.ToString();
            optionsDialogControl.txtFoldersSize.Text = settings.FoldersSize.ToString();
            optionsDialogControl.txtProjectSize.Text = settings.ProjectSize.ToString();

            optionsDialogControl.chkFilename.IsChecked = settings.ViewFilename;
            optionsDialogControl.chkFolders.IsChecked = settings.ViewFolders;
            optionsDialogControl.chkProject.IsChecked = settings.ViewProject;
        }

        public override void ResetSettings()
        {
            settings.Defaults();
            BindSettings();

            base.ResetSettings();
        }

        protected override void OnActivate(CancelEventArgs e)
        {
            base.OnActivate(e);

            BindSettings();
        }

        protected override void OnApply(PageApplyEventArgs args)
        {
            if (args.ApplyBehavior == ApplyKind.Apply)
            {
                // TODO: hex validation
                settings.FilenameColor = optionsDialogControl.txtColorFileName.Text;
                settings.FoldersColor = optionsDialogControl.txtFoldersColor.Text;
                settings.ProjectColor = optionsDialogControl.txtProjectColor.Text;

                double d = settings.FilenameSize;
                if (Double.TryParse(optionsDialogControl.txtFilenameSize.Text, out d)) 
                {
                    settings.FilenameSize = d;
                }

                d = settings.FoldersSize;
                if (Double.TryParse(optionsDialogControl.txtFoldersSize.Text, out d))
                {
                    settings.FoldersSize = d;
                }

                d = settings.ProjectSize;
                if (Double.TryParse(optionsDialogControl.txtProjectSize.Text, out d))
                {
                    settings.ProjectSize = d;
                }

                settings.ViewFilename = optionsDialogControl.chkFilename.IsChecked.Value;
                settings.ViewFolders = optionsDialogControl.chkFolders.IsChecked.Value;
                settings.ViewProject = optionsDialogControl.chkProject.IsChecked.Value;

                settings.Store();
            }

            base.OnApply(args);
        }
    }
}