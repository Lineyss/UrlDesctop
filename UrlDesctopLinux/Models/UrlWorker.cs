﻿using System;
using System.Reflection.Metadata;

namespace UrlDesctopLinux.Models
{
    public class UrlWorker
    {
        private bool IsUnix = Environment.OSVersion.VersionString.Split(" ")[0] == "Unix";
        private string currentUrl;
        public UrlWorker(string Url)
        {
            currentUrl = Url;
        }

        public string GetUrl()
        {
            return IsUnix ? GetUrlLinux() : GetUrlWindows();
        }

        private string GetUrlWindows()
        {
            var array = RemoveUslessWords();
            if (array[0] == "")
            {
                array[0] = "C:";
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

            return arr;
        }
    }
}
