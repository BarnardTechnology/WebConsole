using System;
using System.Net;
using System.Threading;
using System.Linq;
using System.Text;

namespace BarnardTechnology.WebServer
{
    public class Server
    {
        public class ResponseInformation
        {
            public string ContentType;
            public byte[] Content;
        }

        private readonly HttpListener _listener = new HttpListener();
        private readonly Func<HttpListenerRequest, ResponseInformation> _responderMethod;

        public Server(string[] prefixes, Func<HttpListenerRequest, ResponseInformation> method)
        {
            if (!HttpListener.IsSupported)
                throw new NotSupportedException(
                    "Needs Windows XP SP2, Server 2003 or later.");

            // URI prefixes are required, for example 
            // "http://localhost:8080/index/".
            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");

            // A responder method is required
            if (method == null)
                throw new ArgumentException("method");

            foreach (string s in prefixes)
                _listener.Prefixes.Add(s);

            _responderMethod = method;
            _listener.Start();
        }

        public Server(Func<HttpListenerRequest, ResponseInformation> method, params string[] prefixes)
            : this(prefixes, method) { }

        public void Run()
        {
            ThreadPool.QueueUserWorkItem((o) =>
            {
                Console.WriteLine("BarnardTech Web Server is now running at:");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Cyan;
                foreach(string uri in _listener.Prefixes)
                {
                    Console.WriteLine("        " + uri);
                }
                Console.WriteLine();
                Console.ResetColor();
                
                try
                {
                    while (_listener.IsListening)
                    {
                        ThreadPool.QueueUserWorkItem((c) =>
                        {
                            var ctx = c as HttpListenerContext;
                            try
                            {
                                //string rstr = _responderMethod(ctx.Request);
                                //byte[] buf = Encoding.UTF8.GetBytes(rstr);
                                ctx.Response.AddHeader("Content-Type", MIME.GetMimeType(System.IO.Path.GetExtension(ctx.Request.Url.AbsolutePath)));
                                ResponseInformation r = _responderMethod(ctx.Request);
                                byte[] buf = r.Content;
                                if(r.ContentType != "")
                                {
                                    ctx.Response.AddHeader("Content-Type", r.ContentType);
                                }
                                else
                                {
                                    ctx.Response.AddHeader("Content-Type", MIME.GetMimeType(System.IO.Path.GetExtension(ctx.Request.Url.AbsolutePath)));
                                }

                                ctx.Response.ContentLength64 = buf.Length;
                                ctx.Response.OutputStream.Write(buf, 0, buf.Length);
                            }
                            catch { } // suppress any exceptions
                            finally
                            {
                                // always close the stream
                                ctx.Response.OutputStream.Close();
                            }
                        }, _listener.GetContext());
                    }
                }
                catch { } // suppress any exceptions
            });
        }

        public void Stop()
        {
            _listener.Stop();
            _listener.Close();
        }
    }
}