using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text;
using System;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using EnvDTE;
using Microsoft.Internal.VisualStudio.PlatformUI;
// using WhereAmI.Options;

namespace Recoding.WhereAmI
{
    /// <summary>
    /// Adornment class that writes current file's position informations
    /// </summary>
    class WhereAmIAdornment
    {
        private TextBlock _fileName;
        private TextBlock _projectName;
        private IWpfTextView _view;
        private IAdornmentLayer _adornmentLayer;

        /// <summary>
        /// Creates a square image and attaches an event handler to the layout changed event that
        /// adds the the square in the upper right-hand corner of the TextView via the adornment layer
        /// </summary>
        /// <param name="view">The <see cref="IWpfTextView"/> upon which the adornment will be drawn</param>
        public WhereAmIAdornment(IWpfTextView view)
        {
            _view = view;

            _fileName = new TextBlock();
            _projectName = new TextBlock();

            ITextDocument textDoc;
            object obj;
            if (view.TextBuffer.Properties.TryGetProperty<ITextDocument>(typeof(ITextDocument), out textDoc)) 
            {
                // Retrieved the ITextDocument from the first level
            }
            else if (view.TextBuffer.Properties.TryGetProperty<object>("IdentityMapping", out obj))
            {
                // Try to get the ITextDocument from the second level (e.g. Razor files)
                if ((obj as ITextBuffer) != null) 
                {
                    (obj as ITextBuffer).Properties.TryGetProperty<ITextDocument>(typeof(ITextDocument), out textDoc);
                }
            }

            // If I found an ITextDocument, access to its FilePath prop to retrieve informations about Proj
            if (textDoc != null) 
            {
                string fileName = System.IO.Path.GetFileName(textDoc.FilePath);

                Project proj = GetContainingProject(fileName);
                string projectName = proj.Name;

                _fileName.Text = fileName;
                _projectName.Text = projectName;
            }

            // Write the textes
            Brush fileNameBrush = new SolidColorBrush(Color.FromRgb(39,39,39));

            _fileName.FontFamily = new FontFamily("Consolas");
            _fileName.FontSize = 70;
            _fileName.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            _fileName.TextAlignment = System.Windows.TextAlignment.Right;
            _fileName.Foreground = fileNameBrush;


            Brush projectNameBrush = new SolidColorBrush(Color.FromRgb(34, 34, 34));

            _projectName.FontFamily = new FontFamily("Consolas");
            _projectName.FontSize = 54;
            _projectName.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            _projectName.TextAlignment = System.Windows.TextAlignment.Right;
            _projectName.Foreground = projectNameBrush;
 


            // Force to have an ActualWidth
            System.Windows.Rect finalRect = new System.Windows.Rect();
            _fileName.Arrange(finalRect);
            _projectName.Arrange(finalRect);

            //Grab a reference to the adornment layer that this adornment should be added to
            _adornmentLayer = view.GetAdornmentLayer("WhereAmIAdornment");

            _view.ViewportHeightChanged += delegate { this.onSizeChange(); };
            _view.ViewportWidthChanged += delegate { this.onSizeChange(); };
        }

        public void onSizeChange()
        {
            _adornmentLayer.RemoveAllAdornments();
            _adornmentLayer.Opacity = 1;
            
            // Place the textes in the appropriate position
            Canvas.SetLeft(_fileName, _view.ViewportRight - (_fileName.ActualWidth + 15));
            Canvas.SetTop(_fileName, _view.ViewportTop + 15);

            Canvas.SetLeft(_projectName, _view.ViewportRight - (_projectName.ActualWidth + 15));
            Canvas.SetTop(_projectName, _view.ViewportTop + 82);

            // Place the textes in the layer
            _adornmentLayer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative, null, null, _fileName, null);
            _adornmentLayer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative, null, null, _projectName, null);
        }

        /// <summary>
        /// Given a filename, retrieve the Project container
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Project GetContainingProject(string fileName)
        {
            if (!String.IsNullOrEmpty(fileName))
            {
                
                var dte2 = (EnvDTE80.DTE2)Package.GetGlobalService(typeof(SDTE));
                if (dte2 != null)
                {
                    var prjItem = dte2.Solution.FindProjectItem(fileName);
                    if (prjItem != null)
                        return prjItem.ContainingProject;
                }
            }
            return null;
        }
    }
}
