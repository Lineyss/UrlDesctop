using System;
using Microsoft.Office.Interop.Word;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace UrlDesctopLinux.Models
{
    public class FileReader
    {
        private string PathFile;
        public FileReader(string pathFile)
        {
            PathFile = pathFile;
        }

        public string Read()
        {
            string extends = System.IO.Path.GetExtension(PathFile).ToLower();

            if (extends == ".txt" || extends == ".css" || extends == ".html" || extends == ".ini" || extends == ".xml" || extends == ".js")
            {
                return MainReadingFile();
            }
            else if (extends == ".docx" || extends == ".doc")
            {
                return ReadingWordFile();
            }
            else if (extends == ".pdf")
            {
                return ReadingPDF();
            }
            else
            {
                return "Ошибка";
            }
        }

        private string MainReadingFile()
        {
            string Text = "";
            using (StreamReader reader = new StreamReader(PathFile, System.Text.Encoding.UTF8))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Text += line;
                }
            }

            return Text;
        }
        
        private string ReadingWordFile()
        {
            string Text = "";

            Application application = new Application();
            Document document = application.Documents.Open(PathFile);

            // Loop through all words in the document.
            int count = document.Words.Count;
            for (int i = 1; i <= count; i++)
            {
                // Write the word.
                Text += document.Words[i].Text;
            }   
            // Close word.
            application.Quit();

            return Text;
        }

        private string ReadingPDF()
        {
            PdfReader reader = new PdfReader(PathFile);
            string text = string.Empty;
            for (int page = 1; page <= reader.NumberOfPages; page++)
            {
                text += PdfTextExtractor.GetTextFromPage(reader, page);
            }
            reader.Close();
            return text;
        }
    }
}
