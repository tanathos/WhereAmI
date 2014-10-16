using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Settings;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.Text.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using System.Diagnostics;

namespace Recoding.WhereAmI
{
    [Export(typeof(IWhereAmISettings))]
    public class WhereAmISettings : IWhereAmISettings
    {
        /// <summary>
        /// The real store in which the settings will be saved
        /// </summary>
        readonly WritableSettingsStore writableSettingsStore;

        const string CollectionPath = "WhereAmI";

        public WhereAmISettings()
        {

        }

        [ImportingConstructor]
        public WhereAmISettings(SVsServiceProvider vsServiceProvider) : this()
        {
            var shellSettingsManager = new ShellSettingsManager(vsServiceProvider);
            writableSettingsStore = shellSettingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);

            LoadSettings();
        }

        public string FilenameColor { get { return _FilenameColor; } set { _FilenameColor = value; } }
        private string _FilenameColor;

        public string FoldersColor { get { return _FoldersColor; } set { _FoldersColor = value; } }
        private string _FoldersColor;

        public string ProjectColor { get { return _ProjectColor; } set { _ProjectColor = value; } }
        private string _ProjectColor;

        public bool ViewFilename { get { return _ViewFilename; } set { _ViewFilename = value; } }
        private bool _ViewFilename = true;

        public bool ViewFolders { get { return _ViewFolders; } set { _ViewFolders = value; } }
        private bool _ViewFolders = true;

        public bool ViewProject { get { return _ViewProject; } set { _ViewProject = value; } }
        private bool _ViewProject = true;

        public double FilenameSize { get { return _FilenameSize; } set { _FilenameSize = value; } }
        private double _FilenameSize;

        public double FoldersSize { get { return _FoldersSize; } set { _FoldersSize = value; } }
        private double _FoldersSize;

        public double ProjectSize { get { return _ProjectSize; } set { _ProjectSize = value; } }
        private double _ProjectSize;

        public void Store() 
        {
            try
            {
                if (!writableSettingsStore.CollectionExists(CollectionPath))
                {
                    writableSettingsStore.CreateCollection(CollectionPath);
                }

                writableSettingsStore.SetString(CollectionPath, "FilenameColor", this.FilenameColor);
                writableSettingsStore.SetString(CollectionPath, "FoldersColor", this.FoldersColor);
                writableSettingsStore.SetString(CollectionPath, "ProjectColor", this.ProjectColor);

                writableSettingsStore.SetString(CollectionPath, "ViewFilename", this.ViewFilename.ToString());
                writableSettingsStore.SetString(CollectionPath, "ViewFolders", this.ViewFolders.ToString());
                writableSettingsStore.SetString(CollectionPath, "ViewProject", this.ViewProject.ToString());

                writableSettingsStore.SetString(CollectionPath, "FilenameSize", this.FilenameSize.ToString());
                writableSettingsStore.SetString(CollectionPath, "FoldersSize", this.FoldersSize.ToString());
                writableSettingsStore.SetString(CollectionPath, "ProjectSize", this.ProjectSize.ToString());
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
            }
        }

        public void Defaults() 
        {
            writableSettingsStore.DeleteCollection(CollectionPath);
            LoadSettings();
        }

        private void LoadSettings()
        {
            try
            {
                _FilenameSize = 70;
                _FoldersSize = _ProjectSize = 54;

                string visualStudioThemeId = writableSettingsStore.GetString("General", "CurrentTheme");

                switch (visualStudioThemeId)
                {
                    case "de3dbbcd-f642-433c-8353-8f1df4370aba": // Light
                    case "a4d6a176-b948-4b29-8c66-53c97a1ed7d0": // Blue
                        _FilenameColor = "#eaeaea";
                        _FoldersColor = _ProjectColor = "#f3f3f3";
                        break;

                    case "1ded0138-47ce-435e-84ef-9ec1f439b749": // Dark
                    default:
                        _FilenameColor = "#303030";
                        _FoldersColor = _ProjectColor = "#282828";
                        break;
                }

                if (writableSettingsStore.PropertyExists(CollectionPath, "FilenameColor"))
                {
                    this.FilenameColor = writableSettingsStore.GetString(CollectionPath, "FilenameColor", this.FilenameColor);
                }

                if (writableSettingsStore.PropertyExists(CollectionPath, "FoldersColor"))
                {
                    this.FoldersColor = writableSettingsStore.GetString(CollectionPath, "FoldersColor", this.FoldersColor);
                }

                if (writableSettingsStore.PropertyExists(CollectionPath, "ProjectColor"))
                {
                    this.ProjectColor = writableSettingsStore.GetString(CollectionPath, "ProjectColor", this.ProjectColor);
                }

                if (writableSettingsStore.PropertyExists(CollectionPath, "ViewFilename"))
                {
                    bool b = this.ViewFilename;
                    if (Boolean.TryParse(writableSettingsStore.GetString(CollectionPath, "ViewFilename"), out b))
                        this.ViewFilename = b;
                }

                if (writableSettingsStore.PropertyExists(CollectionPath, "ViewFolders"))
                {
                    bool b = this.ViewFolders;
                    if (Boolean.TryParse(writableSettingsStore.GetString(CollectionPath, "ViewFolders"), out b))
                        this.ViewFolders = b;
                }

                if (writableSettingsStore.PropertyExists(CollectionPath, "ViewProject"))
                {
                    bool b = this.ViewProject;
                    if (Boolean.TryParse(writableSettingsStore.GetString(CollectionPath, "ViewProject"), out b))
                        this.ViewProject = b;
                }

                if (writableSettingsStore.PropertyExists(CollectionPath, "FilenameSize"))
                {
                    double d = this.FilenameSize;
                    if (Double.TryParse(writableSettingsStore.GetString(CollectionPath, "FilenameSize"), out d))
                        this.FilenameSize = d;
                }

                if (writableSettingsStore.PropertyExists(CollectionPath, "FoldersSize"))
                {
                    double d = this.FoldersSize;
                    if (Double.TryParse(writableSettingsStore.GetString(CollectionPath, "FoldersSize"), out d))
                        this.FoldersSize = d;
                }

                if (writableSettingsStore.PropertyExists(CollectionPath, "ProjectSize"))
                {
                    double d = this.ProjectSize;
                    if (Double.TryParse(writableSettingsStore.GetString(CollectionPath, "ProjectSize"), out d))
                        this.ProjectSize = d;
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
            }
        }
    }
}
