﻿using System.IO;

namespace FlatoutCMS.Core.Context
{
    class FileReader
    {
        public static string Read(string file)
        {
            var data = string.Empty;
            using (var stream = File.OpenRead(file))
            {
                data = new StreamReader(stream).ReadToEnd();
            }
            return data;
        }
    }
}
