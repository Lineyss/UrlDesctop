using System;
using System.Reflection.Metadata;

namespace UrlDesctopLinux.Models
{
    public class UrlWorker
    {
        private string currentUrl;
        public UrlWorker(string Url)
        {
            currentUrl = Url;
        }

        public static string? GetParentDirectory(string Url)
        {
            if(!String.IsNullOrWhiteSpace(Url))
            {
                return Directory.GetParent(Url)?.Name;
            }
            return null;
        }

        public string? GetParentDirectory()
        {
            if(!String.IsNullOrWhiteSpace(currentUrl))
            {
                return Directory.GetParent(currentUrl)?.Name;
            }
            return null;
        }

        public string GetUrl()
        {
            return Folder.IsUnix ? GetUrlLinux() : GetUrlWindows();
        }

        private string GetUrlWindows()
        {
            List<string> array = RemoveUslessWords();
            if (array.Count == 0)
            {
                array.Add("C:/");
            }
            else if (array[0] == "")
            {
                array[0] = "C:/";
            }
            return string.Join("/", array);
        }

        private string GetUrlLinux()
        {
            var array = RemoveUslessWords();
            if (array[0] == "")
            {
                return "/";
            }
            currentUrl = string.Join("/",array); 

            if(Path.HasExtension(currentUrl))
            {
                return $"/{currentUrl}";
            }

            return $"/{currentUrl}/";
        }

        private List<string> RemoveUslessWords()
        {
            List<string> arr = currentUrl.Split("/").ToList();

            arr.RemoveAt(0);
            arr.RemoveAt(0);
            arr.RemoveAt(0);
            arr.RemoveAt(0);

            return arr;
        }
    }
}
