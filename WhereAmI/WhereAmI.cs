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
    /// Adornment class that draws a square box in the top right hand corner of the viewport
    /// </summary>
    
    class WhereAmIAdornment
    {
        private TextBlock _text;
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

            _text = new TextBlock();

            ITextDocument textDoc;
            if (view.TextBuffer.Properties.TryGetProperty<ITextDocument>(typeof(ITextDocument), out textDoc)) 
            {
                string fileName = System.IO.Path.GetFileName(textDoc.FilePath);

                Project proj = GetContainingProject(fileName);
                string container = proj.Name;

                _text.Text = fileName + Environment.NewLine + container;
            }

            
            // Brush penBrush = new SolidColorBrush(new System.Windows.Media.Color() { A = Settings.Default.Color.A, B = Settings.Default.Color.B, G = Settings.Default.Color.G, R = Settings.Default.Color.R });
            Brush penBrush = new SolidColorBrush(Color.FromArgb(255,255,255,255));

            _text.FontFamily = new FontFamily("Consolas");
            _text.FontSize = 40;
            _text.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            _text.TextAlignment = System.Windows.TextAlignment.Right;
            _text.Foreground = penBrush;
 
            // Force to have an ActualWidth
            System.Windows.Rect finalRect = new System.Windows.Rect();
            _text.Arrange(finalRect);

            //Grab a reference to the adornment layer that this adornment should be added to
            _adornmentLayer = view.GetAdornmentLayer("WhereAmIAdornment");

            _view.ViewportHeightChanged += delegate { this.onSizeChange(); };
            _view.ViewportWidthChanged += delegate { this.onSizeChange(); };
        }

        public void onSizeChange()
        {
            //clear the adornment layer of previous adornments
            _adornmentLayer.RemoveAllAdornments();
            _adornmentLayer.Opacity = 0.2;
            // _adornmentLayer.
            
            //Place the image in the top right hand corner of the Viewport
            Canvas.SetLeft(_text, _view.ViewportRight - (_text.ActualWidth + 15));
            Canvas.SetTop(_text, _view.ViewportTop + 15);

            //add the image to the adornment layer and make it relative to the viewport
            _adornmentLayer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative, null, null, _text, null);

            // _adornmentLayer.Elements[1].Adornment.Visibility;
        }

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
