﻿using System;
using System.Configuration;
using System.IO;
using System.Web;

namespace NorthPole
{
    public class Santa : IHttpHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.HttpMethod != "POST")
                return;

            var userkey = context.Request.QueryString.Get("key");
            var usertimestamp = context.Request.QueryString.Get("timestamp");
            var userhmac = context.Request.QueryString.Get("hmac");

            var minage = TimeSpan.Parse(ConfigurationManager.AppSettings["minage"]);

            var timestamp = DateTime.FromBinary(long.Parse(usertimestamp));

            var age = DateTime.UtcNow - timestamp;

            if (age >= minage)
            {
                var secretkey = userkey + ConfigurationManager.AppSettings["secretkey"];

                string input = string.Empty;
                using (var sr = new StreamReader(context.Request.InputStream))
                {
                    input = sr.ReadToEnd();
                }

                input += string.Format("{0}{1}", Environment.NewLine, usertimestamp);

                var hmac = Rudolph.GenerateHMAC(input, secretkey);

                context.Response.Output.WriteLine(userhmac == hmac ? "NICE" : "NAUGHTY");
            }
            else
            {
                context.Response.Output.WriteLine("NOTREADY " + (minage - age).TotalMilliseconds);
            }
            
            context.Response.Output.Flush();   
        }
    }
}
