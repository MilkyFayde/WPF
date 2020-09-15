using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdoNet1
{
    public class TxtReader
    {
        Dictionary<string, int> words = new Dictionary<string, int>();
        string dirPath;
        string[] wordEndings;
        char[] commas = new char[] { ' ', ',', '.', '?', '!', ')', ':', ';' };
        public TxtReader(string dirPath, string[] wordEndings)
        {
            this.dirPath = dirPath;
            this.wordEndings = wordEndings;
        }

        public Dictionary<string, int> GetWords()
        {
            ScanFolder(dirPath);
            return words;
        }

        void ScanFolder(string path)
        {
            DirectoryInfo dinfo = new DirectoryInfo(path);

            if (!dinfo.Exists) return;

            try
            {
                FileInfo[] files = dinfo.GetFiles();

                foreach (FileInfo current in files)
                    if (current.Extension == ".txt")
                        ReadFile(current.FullName); // read text from file and add words to "words"

                DirectoryInfo[] dirs = dinfo.GetDirectories();
                foreach (DirectoryInfo current in dirs)
                    ScanFolder(current.FullName);
            } // try
            catch
            {
            } // catch
        } // CheckFolder

        private void ReadFile(string fileName)
        {
            StreamReader reader = new StreamReader(fileName, Encoding.Default);

            string line = "";
            while (true)
            {
                line = reader.ReadLine();
                if (line == null) break;

                var tempWords = line.Split(commas, StringSplitOptions.RemoveEmptyEntries); // split text by comma
                tempWords.Where(x => x.EndsWith(wordEndings));

                foreach (var word in tempWords)
                {
                    if (!IsEndingWith(word)) continue; // if word ends with "wordEndings"
                    if (words.ContainsKey(word.ToString())) words[word.ToString()]++; // if word already exists cnt++
                    else words.Add(word.ToString(), 1); // if word is new
                } // foreach
            } // while
            reader.Close();
        } // ReadFile

        bool IsEndingWith(string word)
        {
            foreach (var end in wordEndings)
            {
                if (word.EndsWith(end)) return true;
            }
            return false;
        } // IsEndingWith
    } // class TxtReader
}
