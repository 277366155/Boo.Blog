using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boo.Blog.Middleware
{
    public class SerilogHandlerMiddleware
    {
        static string MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms. ";
        static readonly ILogger Log = Serilog.Log.ForContext<SerilogHandlerMiddleware>();
        readonly RequestDelegate _next;
        public SerilogHandlerMiddleware(RequestDelegate next)
        {
            if (next == null)
                throw new ArgumentNullException(nameof(next));
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var start = Stopwatch.GetTimestamp();
            try
            {
                await _next.Invoke(context);

                var statusCode = context.Response?.StatusCode;

                var level = LogEventLevel.Information;
                if (statusCode > 399 && statusCode < 500)
                {
                    level = LogEventLevel.Warning;
                }
                else if (statusCode >= 500)
                {
                    level = LogEventLevel.Error;
                }

                var log = level == LogEventLevel.Error ? LogForErrorContext(context) : Log;
                log.Write(level, MessageTemplate, context.Request.Method, GetPath(context), statusCode, GetElapsedMilliseconds(start, Stopwatch.GetTimestamp()));
            }
            catch (Exception ex) when (LogException(context, start, ex)) { }

        }

        static bool LogException(HttpContext context, long start, Exception ex)
        {
            //LogForErrorContext(context).Error(ex, MessageTemplate, context.Request.Method, context.Request.Path, StatusCodes.Status500InternalServerError, GetElapsedMilliseconds(start, Stopwatch.GetTimestamp()));
            LogForErrorContext(context).Error(ex, MessageTemplate, context.Request.Method, context.Request.Path, StatusCodes.Status500InternalServerError, GetElapsedMilliseconds(start, Stopwatch.GetTimestamp()));
            return false;
        }

        static ILogger LogForErrorContext(HttpContext context)
        {
            var result = Log.ForContext("RequestHeaders", context.Request.Headers.ToDictionary(a => a.Key, a => a.Value.ToString()), destructureObjects: true)
                .ForContext("RequestHost", context.Request.Host)
                .ForContext("RequestProtocol", context.Request.Protocol);

            if (context.Request.HasFormContentType)
                result = result.ForContext("RequestForm", context.Request.Form.ToDictionary(a => a.Key, a => a.Value.ToString()));
            return result;
        }

        static double GetElapsedMilliseconds(long start, long stop)
        {
            return (stop - start) * 1000 / (double)Stopwatch.Frequency;
        }
        static string GetPath(HttpContext context)
        {
            return context.Features.Get<IHttpRequestFeature>()?.RawTarget ?? context.Request.Path.ToString();
        }

    }
}
