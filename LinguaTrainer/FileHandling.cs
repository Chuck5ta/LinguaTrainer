using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.IO; // for file handling
using System.Collections;
using System.Collections.Specialized;

namespace LinguaTrainer
{
    class FileHandling
    {

        // Load words
        // this is now being used to load both the words list and the words/wav list
        public static SortedList<string, string> loadWords(string fileName)
        {
            SortedList<string, string> wordList = new SortedList<string, string>();

            // We are saving the configuration
            // Create new file and save the data
            try
            {
                string[] fileLine;
                // this will replace any existing file
                FileStream fs = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                //               using (StreamReader fileReader = new StreamReader(fs, Encoding.GetEncoding("iso-8859-1")))
                using (StreamReader fileReader = new StreamReader(fs, Encoding.GetEncoding(1252)))
                {
                    while (!fileReader.EndOfStream)
                    {
                        fileLine = fileReader.ReadLine().Split(',');
                        wordList.Add(fileLine[0], fileLine[1]);
                    }
                }
                fs.Close();
            }
            catch
            {
                //              MessageBox.Show("ERROR - accessing file!");
            }

            return wordList;
        } // END OF loadWords(string fileName)

        // Load wav list
        // this is now being used to load both the words list and the words/wav list
        public static Dictionary<string, string> loadWordsWavList(string fileName)
        {
            Dictionary<string, string> wordList = new Dictionary<string, string>();

            // We are saving the configuration
            // Create new file and save the data
            try
            {
                string[] fileLine;
                // this will replace any existing file
                FileStream fs = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                using (StreamReader fileReader = new StreamReader(fs, Encoding.GetEncoding(1252)))
                {
                    while (!fileReader.EndOfStream)
                    {
                        fileLine = fileReader.ReadLine().Split(',');
                        wordList.Add(fileLine[0], fileLine[1]);
                    }
                }
                fs.Close();
            }
            catch
            {
                //              MessageBox.Show("ERROR - accessing file!");
            }

            return wordList;
        } // END OF loadWords(string fileName)


        /*
         * Retrieve the list of files names
         */
        public static string[] getListOfFiles(string filePath)
        {
            string[] filename = Directory.GetFiles(@filePath);

            return filename;
        }


    }
}
