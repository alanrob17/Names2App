using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Names
{
    public class ArgList
    {
        public ArgList(bool subFolder, bool changeFileName, bool changeFolderName)
        {
            this.SubFolder = subFolder;
            this.ChangeFileName = changeFileName;
            this.ChangeFolderName = changeFolderName;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to search sub folders.
        /// </summary>
        public bool SubFolder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to change file names.
        /// </summary>
        public bool ChangeFileName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to change folder names.
        /// </summary>
        public bool ChangeFolderName { get; set; }
    }
}
