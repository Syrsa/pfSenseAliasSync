using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace Firewall
{
    public class Profile
    {
        
        public List<Alias> aliases;
         

        public Profile()
        { 
            aliases = new List<Alias>();
        }

        public void GetAliasesFromFile(string path)
        {
            string pattern1 = @"aliasName=([^;]+);descr=([^;]+)";
            string pattern2 = @"addr=([^;]+);detail=([^;]+)";

            Alias alias;
            using (StreamReader sr = new StreamReader(path))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Match match = Regex.Match(line, pattern1);
                    if (match.Success)
                    {
                        alias = new Alias(match.Groups[1].Value, match.Groups[2].Value);

                        match = Regex.Match(line, pattern2);
                        while (match.Success)
                        {
                            alias.AddHost(match.Groups[1].Value, match.Groups[2].Value);
                            match = match.NextMatch();
                        }
                        aliases.Add(alias);
                    }
                }
            }
        }

        public void SaveProfile(string filepath)
        { 
            XmlHandler.XmlWriter(this, filepath);
        }

        /// <summary> 
        /// This method accepts two strings the represent two files to compare.
        /// </summary> 
        /// <param name="file1">Path to first file.</param> 
        /// <param name="file2">Path to second file.</param> 
        /// <returns>
        ///  A return value of TRUE indicates that the contents of the files are the same. 
        ///  A return value of FALSE indicates that the files are not the same.
        /// </returns> 
        private bool FileCompare(string file1, string file2)
        {
            int file1byte;
            int file2byte;
            FileStream fs1;
            FileStream fs2;

            // Determine if the same file was referenced two times.
            if (file1 == file2)
            {
                // Return true to indicate that the files are the same.
                return true;
            }

            // Open the two files.
            fs1 = new FileStream(file1, FileMode.Open, FileAccess.Read);
            fs2 = new FileStream(file2, FileMode.Open, FileAccess.Read);

            // Check the file sizes. If they are not the same, the files are not the same.
            if (fs1.Length != fs2.Length)
            {
                // Close the file
                fs1.Close();
                fs2.Close();

                // Return false to indicate files are different
                return false;
            }

            // Read and compare a byte from each file until either a non-matching set of bytes is found or until the end of file1 is reached.
            do
            {
                // Read one byte from each file.
                file1byte = fs1.ReadByte();
                file2byte = fs2.ReadByte();
            }
            while ((file1byte == file2byte) && (file1byte != -1));

            // Close the files.
            fs1.Close();
            fs2.Close();

            // Return the success of the comparison. "file1byte" is equal to "file2byte" at this point only if the files are the same.
            return ((file1byte - file2byte) == 0);
        }

     }
}
