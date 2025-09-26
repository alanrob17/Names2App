using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Names.Models
{
    public class ArgList
    {
        public ArgList(bool subFolder, bool changeFileName, bool properCase)
        {
            SubFolder = subFolder;
            ChangeFileName = changeFileName;
            ProperCase = properCase;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to search sub folders.
        /// </summary>
        public bool SubFolder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to change file names.
        /// </summary>
        public bool ChangeFileName { get; set; }

        public bool ProperCase { get; set; }
    }
}
