using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.VisualStudio.Shell;

namespace WhereAmI.Options
{
    /// <summary>
    /// Interaction logic for OptionsDialogPageControl.xaml
    /// </summary>
    public partial class OptionsDialogPageControl : UserControl
    {
        public OptionsDialogPageControl()
        {
            InitializeComponent();

            // encouragements.AddHandler(UIElementDialogPage.DialogKeyPendingEvent, new RoutedEventHandler(OnDialogKeyPendingEvent));
        }

        void OnDialogKeyPendingEvent(object sender, RoutedEventArgs e)
        {
            var args = e as DialogKeyEventArgs;
            if (args != null && args.Key == Key.Enter)
            {
                e.Handled = true;
            }
        }

        private void chkFilename_Checked(object sender, RoutedEventArgs e)
        {
            txtColorFileName.IsEnabled = true;
            txtFilenameSize.IsEnabled = true;
        }

        private void chkFilename_Unchecked(object sender, RoutedEventArgs e)
        {
            txtColorFileName.IsEnabled = false;
            txtFilenameSize.IsEnabled = false;
        }

        private void chkFolders_Checked(object sender, RoutedEventArgs e)
        {
            txtFoldersColor.IsEnabled = true;
            txtFoldersSize.IsEnabled = true;
        }

        private void chkFolders_Unchecked(object sender, RoutedEventArgs e)
        {
            txtFoldersColor.IsEnabled = false;
            txtFoldersSize.IsEnabled = false;
        }

        private void chkProject_Checked(object sender, RoutedEventArgs e)
        {
            txtProjectColor.IsEnabled = true;
            txtProjectSize.IsEnabled = true;
        }

        private void chkProject_Unchecked(object sender, RoutedEventArgs e)
        {
            txtProjectColor.IsEnabled = false;
            txtProjectSize.IsEnabled = false;
        }

        //public string Encouragements
        //{
        //    get { return encouragements.Text; }
        //    set { encouragements.Text = value; }
        //}
    }
}
