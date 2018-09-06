using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MimeTypes;

namespace BarnardTechnology.WebServer
{
    public class MIME
    {
        public static string GetMimeType(string extension)
        {
            return MimeTypeMap.GetMimeType(extension);
        }
    }
}
