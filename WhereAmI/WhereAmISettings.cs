using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Settings;
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

        [ImportingConstructor]
        public WhereAmISettings(SVsServiceProvider vsServiceProvider)
        {
            var shellSettingsManager = new ShellSettingsManager(vsServiceProvider);
            writableSettingsStore = shellSettingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);

            LoadSettings();
        }

        public string FilenameColor { get { return _FilenameColor; } set { _FilenameColor = value; } }
        private string _FilenameColor = "#303030";

        public string FoldersColor { get { return _FoldersColor; } set { _FoldersColor = value; } }
        private string _FoldersColor = "#282828";

        public string ProjectColor { get { return _ProjectColor; } set { _ProjectColor = value; } }
        private string _ProjectColor = "#282828";

        public bool ViewFilename
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool ViewFolders
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool ViewProject
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

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
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
            }
        }

        private void LoadSettings()
        {
            try
            {
                if (writableSettingsStore.PropertyExists(CollectionPath, "FilenameColor"))
                {
                    this.FilenameColor = writableSettingsStore.GetString(CollectionPath, "FilenameColor");
                }

                if (writableSettingsStore.PropertyExists(CollectionPath, "FoldersColor"))
                {
                    this.FoldersColor = writableSettingsStore.GetString(CollectionPath, "FoldersColor");
                }

                if (writableSettingsStore.PropertyExists(CollectionPath, "ProjectColor"))
                {
                    this.ProjectColor = writableSettingsStore.GetString(CollectionPath, "ProjectColor");
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
            }
        }
    }
}
