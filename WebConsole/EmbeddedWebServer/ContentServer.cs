using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using BarnardTechnology.WebServer;

namespace BarnardTechnology.EmbeddedWebServer
{
    public class ContentServer
    {
        Server _wServer;

        public ContentServer(string[] prefixes, Type contentLocation, Func<HttpListenerRequest, Server.ResponseInformation> method = null)
        {
            ContentGenerator gen = new ContentGenerator(contentLocation);

            _wServer = new Server(prefixes, (request) =>
            {
                Server.ResponseInformation resp;
                if(method != null)
                {
                    resp = method(request);
                    if(resp != null)
                    {
                        if(resp.Content != null)
                        {
                            return resp;
                        }
                    }
                }
                resp = new Server.ResponseInformation();

                resp.Content = gen.GetContent(request.Url.AbsolutePath);
                resp.ContentType = MIME.GetMimeType(System.IO.Path.GetExtension(request.Url.AbsoluteUri));

                return resp;
            });

            _wServer.Run();
        }
    }
}