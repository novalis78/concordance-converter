using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace ConcordanceConverter
{
    

    class Program
    {
        const string HTML_TAG_PATTERN = "<.*?>";


        static void Main(string[] args)
        {
            
            //PrepareForConcordance();

            //PrepareForWebConcordance();

            //ColloquialPali();

            buildSQL();

        }

        #region conc
        private static void PrepareForConcordance()
        {
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            //string[] files = Directory.GetFiles(startupPath + "\\cscd");
            string[] files = Directory.GetFiles(startupPath + "\\cscd_mul");
            foreach (string file in files)
            {
                string line = "";
                string newtext = "";
                StreamReader fileObject = new System.IO.StreamReader(file, Encoding.Default);
                line = fileObject.ReadToEnd();
                //newtext = StripHTML(line);
                newtext = StripUnneccessaryCharacters(newtext);
                
                fileObject.Close();
                string filename = file;
                //filename = filename.Replace("cscd", "cscdclean");
                filename = filename.Replace("cscd", "cscd_mul_clean");
                StreamWriter tt = new StreamWriter(filename, false, Encoding.UTF8);
                tt.Write(newtext);
                tt.Flush();
                tt.Close();
            }
        }

        private static string StripUnneccessaryCharacters(string newtext)
        {
            string a = newtext;
            a = a.Replace("\"", " ");
            a = a.Replace("---", " ");
            a = a.Replace("--", " ");
            a = a.Replace("pe0", " ");
            a = a.Replace("a0", " ");
            a = a.Replace("â", "");
            a = a.Replace("0", "");
            a = a.Replace("1", "");
            a = a.Replace("2", "");
            a = a.Replace("3", "");
            a = a.Replace("4", "");
            a = a.Replace("5", "");
            a = a.Replace("6", "");
            a = a.Replace("7", "");
            a = a.Replace("8", "");
            a = a.Replace("9", "");
            a = a.Replace("{", " ");
            a = a.Replace("}", " ");
            a = a.Replace("[", " ");
            a = a.Replace("]", " ");
            a = a.Replace("``", " ");
            a = a.Replace("''", " ");    
            
            return a;
        }

        private static void PrepareForWebConcordance()
        {
            //Read all files from a given subfolder
            //read all lines in each file
            //make them lower case
            //make them replace symbols for unicode characters
            //save file back in utf-8 encoding
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            int no = 0;
            string[] files = Directory.GetFiles(startupPath + "\\cscd_mul_clean_mul");
            foreach (string file in files)
            {
                if ((!file.EndsWith("txt")) && (!file.EndsWith("htm")))
                    continue;
                string line = "";
                int counting = 0;
                
                List<string> newtext = new List<string>();
                StreamReader fileObject = new System.IO.StreamReader(file, Encoding.Default);
                while ((line = fileObject.ReadLine()) != null)
                {
                    counting++;

                    newtext.Add(ConvertToNewFormat(line));
                    

                    if (counting == 1)
                    {
                        if (line.Contains("HTML"))
                        {
                            newtext.Add("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
                        }
                    }
                }
                no++;
                Console.WriteLine("converted " + no + " from " + files.Length);
                fileObject.Close();
                string filename = file;
                filename = filename.Replace("cscd_mul_clean_mul", "cscd_mul_clean_mul_final");
                StreamWriter tt = new StreamWriter(filename, false, Encoding.UTF8);
                foreach (string l in newtext)
                {
                    tt.WriteLine(l);
                }
                tt.Flush();
                tt.Close();

            }
        }

        private static string ConvertToNewFormat(string line)
        {
            string a = line;
            
            a = a.Replace("±", "ā");
            a = a.Replace("²", "ī");
            a = a.Replace("&sup2;", "ī");
            a = a.Replace("³", "ū");
            a = a.Replace("&sup3;", "ū"); 
            a = a.Replace("ª", "ṅ");
            a = a.Replace("µ", "ṭ");
            a = a.Replace("&micro;", "ṭ"); 
            a = a.Replace("¹", "ḍ");
            a = a.Replace("&sup1;", "ḍ");
            a = a.Replace("º", "ṇ");
            a = a.Replace("&ordf;", "ṇ");  
            a = a.Replace("&ordm;", "ṇ");
            a = a.Replace("ý", "Ḷ");
            a = a.Replace("þ", "Ṃ");
            a = a.Replace("ð", "Ṇ");
            a = a.Replace("Þ", "Ḍ");
            a = a.Replace("&ntilde;", "ñ"); 
            a = a.Replace("Ý", "Ṭ");
            a = a.Replace("Ð", "Ū");
            a = a.Replace("Æ", "S.");
            a = a.Replace("æ", "S'");
            a = a.Replace("½", "ṃ");
            a = a.Replace("&frac12;", "ṃ");
            
            a = a.Replace("¼", "ḷ");
            a = a.Replace("&frac14;", "ḷ");
            a = a.Replace("¾", "Ā");
            a = a.Replace("&frac34;", "Ā");
            a = a.Replace("¿", "Ī");
            a = a.Replace("©", "Ṅ");
            a = a.Replace("¥", "ṛ");
            a = a.Replace("œ", "s'");
            a = a.Replace("Œ", "s.");
            a = a.Replace("&Acirc;", "");
            a = a.Replace("cellspacing=0", "cellspacing=15");
            a = a.Replace("VriRomanPali CB", "Arial");
            //ãā -> ñ
            a = a.ToLower();
            a = a.Replace("â", "");
            a = a.Replace("ãā", "ñ");
            
            return a;
        }
        #endregion


        private static void ColloquialPali()
        {
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            string[] files = Directory.GetFiles(startupPath + "\\cscd_mul");
            foreach (string file in files)
            {
                string text = "";
                StreamReader fileObject = new System.IO.StreamReader(file, Encoding.Default);
                text = fileObject.ReadToEnd();

                System.Text.RegularExpressions.MatchCollection mc = System.Text.RegularExpressions.Regex.Matches(text, "‘‘(.{1,100})’’");

                fileObject.Close();
            }
        }
 
        static string StripHTML (string inputString)
        {
           //return Regex.Replace(inputString, HTML_TAG_PATTERN, string.Empty);
            return Regex.Replace(inputString, @"<(.|\n)*?>", string.Empty);
        }

        static void buildSQL()
        {
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            string line = "";
            int counting = 0;
            List<string> newtext = new List<string>();
            StreamReader fileObject = new System.IO.StreamReader(startupPath + "\\sortedCanonList.txt", Encoding.Default);
            while ((line = fileObject.ReadLine()) != null)
            {
                counting++;
                string a = "INSERT INTO  `palidb2010`.`pali_terms` (`id` ,`name`)VALUES (NULL ,  '"+line+"');";
                newtext.Add(a);
                Console.WriteLine("Counter: " + counting);

            }
            fileObject.Close();
            string filename = "sortedReady.txt";
            StreamWriter tt = new StreamWriter(filename, false, Encoding.UTF8);
            foreach (string l in newtext)
            {
                tt.WriteLine(l);
            }
            tt.Flush();
            tt.Close();

        }
    }
}
