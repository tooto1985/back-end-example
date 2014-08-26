using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web;
using Microsoft.Ajax.Utilities;

/// <summary>
/// AutoMinify 的摘要描述
/// </summary>
public class AutoMinify : IHttpModule
{
    #region IHttpModule 成員

    public void Dispose()
    {

    }

    public void Init(HttpApplication context)
    {
        if (ConfigurationManager.AppSettings["AutoMinify"] == "True")
        {
            context.BeginRequest += Application_BeginRequest;
        }
    }

    #endregion

    private void Application_BeginRequest(Object source, EventArgs e)
    {
        HttpApplication application = (HttpApplication)source;
        HttpContext context = application.Context;
        var file = context.Request.Path.ToLower();
        if (file.Contains(".css") || file.Contains(".js"))
        {
            Minify(context, file);
        }
    }

    private void Minify(HttpContext context, string file)
    {
        var ifNoneMatch = context.Request.Headers["If-None-Match"];
        var eTag = GetETag(context.Server.MapPath(file));
        var fileType = FileType.JS;
        if (file.Contains(".css"))
        {
            fileType = FileType.CSS;
        }
        context.Response.ContentType = fileType == FileType.JS ? "text/javascript" : "text/css";
        context.Response.AppendHeader("Cache-Control", "public, max-age=3600");
        if (ifNoneMatch == eTag)
        {
            context.Response.StatusCode = 304;
        }
        else
        {
            var path = context.Server.MapPath(file);
            var cache = context.Cache[path];
            if (cache == null || ((KeyValuePair<string, string>) cache).Key != eTag)
            {
                var sb = new StringBuilder();
                using (var sr = new StreamReader(path))
                {
                    if (fileType == FileType.JS)
                    {
                        var cs = new CodeSettings
                        {
                            MinifyCode = true,
                            OutputMode = OutputMode.SingleLine,
                            InlineSafeStrings = true,
                            MacSafariQuirks = true,
                            RemoveUnneededCode = true,
                            LocalRenaming = LocalRenaming.CrunchAll,
                            EvalTreatment = EvalTreatment.MakeAllSafe,
                            PreserveFunctionNames = false
                        };
                        sb.Append((new Minifier()).MinifyJavaScript(sr.ReadToEnd(), cs));
                    }
                    else
                    {
                        var cs = new CssSettings
                        {
                            OutputMode = OutputMode.SingleLine
                        };
                        sb.Append((new Minifier()).MinifyStyleSheet(sr.ReadToEnd(), cs));
                    }
                    cache = new KeyValuePair<string, string>(eTag, sb.ToString());
                }
            }
            context.Response.Write(((KeyValuePair<string, string>) cache).Value);
            context.Response.AppendHeader("ETag", eTag);
            //context.Response.AppendHeader("Date",DateTime.Now.ToString("r"));
            //context.Response.AppendHeader("Expires", DateTime.Now.AddHours(1).ToString("r"));

        }
        context.Response.End();
    }

    private string GetETag(string file)
    {
        return File.GetLastWriteTime(file).Ticks.ToString("X");
    }

    private enum FileType
    {
        CSS,
        JS
    }
}