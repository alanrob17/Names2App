using Names.Models;
using Names.Services.Output;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Names.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly IOutputService _output;

        public FileRepository(IOutputService output)
        {
            _output = output;
        }

        public IEnumerable<string> GetFileList(string rootFolder, ArgList argList)
        {
            var dir = new DirectoryInfo(rootFolder);
            var fileList = new List<string>();

            if (argList.SubFolder)
            {
                GetFiles(dir, fileList);
            }
            else
            {
                GetFiles(dir, fileList, false);
            }

            return fileList;
        }

        public IEnumerable<Item> GetItemList(IEnumerable<string> fileList)
        {
            IEnumerable<Item> items = new List<Item>();
            var count = 0;

            foreach (var file in fileList)
            {
                var fileName = Path.GetFileNameWithoutExtension(file);
                var item = new Item
                {
                    ItemId = count++,
                    Name = fileName,
                    ChangeName = fileName,
                    Changed = false,
                    Path = Path.GetDirectoryName(file),
                    Extension = Path.GetExtension(file)
                };

                items = items.Append(item);
            }

            return items;
        }

        public string RemovePhrase(string filename, ICollection<string> phrases)
        {
            foreach (var phrase in phrases.Where(phrase => filename.ToLowerInvariant().Contains(phrase)))
            {
                filename = ReplaceEx(filename, phrase, string.Empty);
            }

            return filename.Trim();
        }

        public void ModifyStatus(IEnumerable<Item> items)
        {
            foreach (var item in items.Where(item => item.Name != item.ChangeName))
            {
                item.Changed = true;
            }
        }

        public void ChangeFileNames(IEnumerable<Item> items)
        {
            foreach (var item in items.Where(item => item.Changed))
            {
                try
                {
                    var oldName = Path.Combine(item.Path ?? string.Empty, item.Name + item.Extension);
                    var newName = Path.Combine(item.Path ?? string.Empty, item.ChangeName + item.Extension.ToLowerInvariant());

                    File.Move(oldName, newName);
                }
                catch (IOException ex)
                {
                    _output.WriteLineAsync(ex + $"\n{item.ChangeName}");
                }
            }
        }

        public string ModifyName(string filename, string pattern)
        {
            var replacement = " ";

            var regex = new Regex(pattern, RegexOptions.IgnoreCase);

            if (!string.IsNullOrEmpty(filename))
            {
                var number = regex.Matches(filename).Count;

                filename = Regex.Replace(filename, pattern, replacement);

                if (pattern == "\\s+")
                {
                    filename = Regex.Replace(filename, pattern, replacement);
                }
            }

            return filename;
        }

        public string CleanFileName(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
                return filename;

            // Characters to trim from start and end
            char[] trimChars = { '.', '-', '_', ' ', '!' };

            // Trim repeatedly until no more changes occur
            string previous;
            do
            {
                previous = filename;
                filename = filename.Trim(trimChars);
            }
            while (filename != previous);

            return filename;
        }

        public string AddSpaces(string filename)
        {
            // var s = "TheSamsungGalaxyBookVol3RevisedEdition2014";
            return Regex.Replace(filename, @"([a-z])([A-Z])", "$1 $2");
        }

        public string FixTerms(string filename)
        {
            var replacements = new (string find, string replace)[]
            {
                ("asp net", "ASP.Net"),
                ("iphone", "iPhone"),
                ("ipad", "iPad"),
                ("ipod", "iPod"),
                ("dotnet", ".Net"),
                ("javascript", "JavaScript"),
                ("jquery", "jQuery"),
                ("csharp", "C#"),
                ("docbook", "DocBook"),
                ("powershell", "PowerShell"),
                ("couchdb", "CouchDB"),
                ("node js", "Node.js"),
                ("node.js", "Node.js"),
                ("asp.net", "ASP.Net"),
                ("ibook", "iBook"),
                ("cplusplus", "C++"),
                (" ios ", " IOS "),
                ("l i n q", "LINQ"),
                ("s q l", "SQL"),
                ("r e a d m e", "README"),
                ("u k ", "UK "),
                ("p c ", "PC "),
                ("oreilly", "O'Reilly"),
                ("1ST", "1st"),
                ("2ND","2nd"),
                ("3RD","3rd"),
                ("4TH","4th"),
                ("5TH","5th"),
                ("6TH","6th"),
                ("7TH","7th"),
                ("8TH","8th"),
                ("8TH","9th")
            };

            foreach (var (find, replace) in replacements)
            {
                if (filename.Contains(find, StringComparison.OrdinalIgnoreCase))
                {
                    filename = ReplaceEx(filename, find, replace);
                }
            }

            return filename;
        }

        public string FixCase(string filename)
        {
            var textInfo = new CultureInfo("en-AU", false).TextInfo;
            filename = textInfo.ToTitleCase(filename);

            return filename;
        }

        public string RemoveDiacritics(string filename)
        {
            var normalizedString = filename.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public string RemoveEmojis(string filename)
        {
            var emojisToRemove = new[] { "🎃", "🍑", "💦", "🍆", "❌" };

            foreach (var emoji in emojisToRemove)
            {
                filename = filename.Replace(emoji, string.Empty);
            }

            return filename;
        }

        public void WriteReport(IEnumerable<Item> items)
        {
            var outFile = Environment.CurrentDirectory + "\\alan.log";
            var outStream = File.Create(outFile);
            var sw = new StreamWriter(outStream);

            // TODO: delete the log file if it exists
            foreach (var item in items.Where(item => item.Changed))
            {
                var originalName = $"{item.Path}\\{item.Name}{item.Extension}";
                var newName = $"{item.Path}\\{item.ChangeName}{item.Extension}";
                sw.WriteLine($"{originalName}\nto\n{newName}\n\n");
            }

            // flush and close
            sw.Flush();
            sw.Close();
        }

        public string RemoveNames(string filename)
        {
            var names = new Test().GetNames();

            foreach (string line in names)
            {
                if (filename.ToLowerInvariant().Contains(line))
                {
                    filename = ReplaceEx(filename, line, string.Empty);
                }
            }

            return filename;
        }

        private static string ReplaceEx(string original, string pattern, string replacement)
        {
            int position0, position1;
            var count = position0 = position1 = 0;
            var upperString = original.ToUpper();
            var upperPattern = pattern.ToUpper();
            var inc = (original.Length / pattern.Length) * (replacement.Length - pattern.Length);
            var chars = new char[original.Length + Math.Max(0, inc)];
            while ((position1 = upperString.IndexOf(upperPattern, position0, StringComparison.Ordinal)) != -1)
            {
                for (var i = position0; i < position1; ++i)
                {
                    chars[count++] = original[i];
                }

                for (var i = 0; i < replacement.Length; ++i)
                {
                    chars[count++] = replacement[i];
                }

                position0 = position1 + pattern.Length;
            }

            if (position0 == 0)
            {
                return original;
            }

            for (var i = position0; i < original.Length; ++i)
            {
                chars[count++] = original[i];
            }

            return new string(chars, 0, count);
        }

        private void GetFiles(DirectoryInfo d, ICollection<string> fileList, bool recursive = true)
        {
            var files = d.GetFiles("*.*");
            foreach (var fileName in files.Select(file => file.FullName))
            {
                var ext = Path.GetExtension(fileName.ToLowerInvariant());
                if (ext != ".exe" && ext != ".bak" && ext != ".log" && ext != ".rar" && ext != ".zip")
                {
                    fileList.Add(fileName);
                }
            }

            if (recursive)
            {
                var dirs = d.GetDirectories("*.*");
                foreach (var dir in dirs)
                {
                    GetFiles(dir, fileList, true);
                }
            }
        }
    }
}
