using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text;
using System;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using EnvDTE;
using Microsoft.Internal.VisualStudio.PlatformUI;
using System.ComponentModel.Composition;
// using WhereAmI.Options;

namespace Recoding.WhereAmI
{
    /// <summary>
    /// Adornment class that writes current file's position informations
    /// </summary>
    class WhereAmIAdornment
    {
        private TextBlock _fileName;
        private TextBlock _folderStructure;
        private TextBlock _projectName;

        private IWpfTextView _view;
        private IAdornmentLayer _adornmentLayer;

        /// <summary>
        /// Settings of the extension, injected by the provider
        /// </summary>
        readonly IWhereAmISettings _settings;

        /// <summary>
        /// Creates a square image and attaches an event handler to the layout changed event that
        /// adds the the square in the upper right-hand corner of the TextView via the adornment layer
        /// </summary>
        /// <param name="view">The <see cref="IWpfTextView"/> upon which the adornment will be drawn</param>
        public WhereAmIAdornment(IWpfTextView view, IWhereAmISettings settings)
        {
            _view = view;
            _settings = settings;

            _fileName = new TextBlock();
            _folderStructure = new TextBlock();
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
                if (proj != null) 
                {
                    string projectName = proj.Name;

                    if (_settings.ViewFilename)
                    {
                        _fileName.Text = fileName;

                        Brush fileNameBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom(_settings.FilenameColor));
                        _fileName.FontFamily = new FontFamily("Consolas");
                        _fileName.FontSize = _settings.FilenameSize;
                        _fileName.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                        _fileName.TextAlignment = System.Windows.TextAlignment.Right;
                        _fileName.Foreground = fileNameBrush;
                    }

                    if (_settings.ViewFolders)
                    {
                        _folderStructure.Text = GetFolderDiffs(textDoc.FilePath, proj.FullName);

                        Brush foldersBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom(_settings.FoldersColor));
                        _folderStructure.FontFamily = new FontFamily("Consolas");
                        _folderStructure.FontSize = _settings.FoldersSize;
                        _folderStructure.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                        _folderStructure.TextAlignment = System.Windows.TextAlignment.Right;
                        _folderStructure.Foreground = foldersBrush;
                    }

                    if (_settings.ViewProject)
                    {
                        _projectName.Text = projectName;

                        Brush projectNameBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom(_settings.ProjectColor));
                        _projectName.FontFamily = new FontFamily("Consolas");
                        _projectName.FontSize = _settings.ProjectSize;
                        _projectName.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                        _projectName.TextAlignment = System.Windows.TextAlignment.Right;
                        _projectName.Foreground = projectNameBrush;
                    }
                }
            }

            // Force to have an ActualWidth
            System.Windows.Rect finalRect = new System.Windows.Rect();
            _fileName.Arrange(finalRect);
            _folderStructure.Arrange(finalRect);
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

            double foldersTopOffset = _view.ViewportTop;
            double projectTopOffset = _view.ViewportTop;

            // Place the textes in the layer
            if (_settings.ViewFilename) 
            {
                Canvas.SetLeft(_fileName, _view.ViewportRight - (_fileName.ActualWidth + 15));
                Canvas.SetTop(_fileName, _view.ViewportTop + 5);

                _adornmentLayer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative, null, null, _fileName, null);

                foldersTopOffset += _fileName.ActualHeight;
                projectTopOffset += _fileName.ActualHeight;
            }

            if (_settings.ViewFolders)
            {
                Canvas.SetLeft(_folderStructure, _view.ViewportRight - (_folderStructure.ActualWidth + 15));
                Canvas.SetTop(_folderStructure, foldersTopOffset);

                _adornmentLayer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative, null, null, _folderStructure, null);

                projectTopOffset += _folderStructure.ActualHeight;
            }

            if (_settings.ViewProject)
            {
                Canvas.SetLeft(_projectName, _view.ViewportRight - (_projectName.ActualWidth + 15));
                Canvas.SetTop(_projectName, projectTopOffset);

                _adornmentLayer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative, null, null, _projectName, null);
            }
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

        /// <summary>
        /// Given 2 absolute paths, returns the difference in folder structure.
        /// (The first should be nested in the second)
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        private static string GetFolderDiffs(string filePath, string folderPath) 
        {
            if (!String.IsNullOrEmpty(folderPath))
                return System.IO.Path.GetDirectoryName(filePath).Replace(System.IO.Path.GetDirectoryName(folderPath), "").Replace("\\", "/").ToLower();

            return "";
        }
    }
}
