using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recoding.WhereAmI
{
    /// <summary>
    /// Settings for this extension, to be stored at user-level
    /// </summary>
    public interface IWhereAmISettings
    {
        /// <summary>
        /// The color of the current filename (hexadecimal)
        /// </summary>
        string FilenameColor { get; set; }

        /// <summary>
        /// The color of the folders string (hexadecimal)
        /// </summary>
        string FoldersColor { get; set; }

        /// <summary>
        /// The color of the project name (hexadecimal)
        /// </summary>
        string ProjectColor { get; set; }

        /// <summary>
        /// Indicates if the filename has to be visible
        /// </summary>
        bool ViewFilename { get; set; }

        /// <summary>
        /// Indicates if the folders structure has to be visible
        /// </summary>
        bool ViewFolders { get; set; }

        /// <summary>
        /// Indicates if the project name has to be visible
        /// </summary>
        bool ViewProject { get; set; }

        /// <summary>
        /// The size of the text for the filename
        /// </summary>
        double FilenameSize { get; set; }

        /// <summary>
        /// The size of the text for the folders
        /// </summary>
        double FoldersSize { get; set; }

        /// <summary>
        /// The size of the text for the project
        /// </summary>
        double ProjectSize { get; set; }

        /// <summary>
        /// Performs the store of the instance of this interface to the user's settings
        /// </summary>
        void Store();
    }
}
