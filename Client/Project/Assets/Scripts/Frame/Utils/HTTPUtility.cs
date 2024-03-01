using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using BestHTTP;
using BestHTTP.Forms;
using Cysharp.Threading.Tasks;
using System.Linq.Expressions;

namespace Huge.Utils
{
    public class HTTPHandler
    {
        public string msg = "";
        public bool isDone = false;
        public bool isError = false;
        public bool isTimeout = false;
        public byte[] buffer = null;
        public string text = "";

        public HTTPHandler()
        {
            msg = "";
            text = "";
            buffer = null;
            isDone = false;
            isError = false;
            isTimeout = false;
        }
    }

    public static class HTTPUtility
    {
        static readonly double HttpTimeout = 60.0f;
        static readonly double HttpConnectTimeout = 15.0f;

        public delegate void HttpCallback(HTTPHandler handler);

        public static void Get(string url, HttpCallback callback)
        {
            HTTPRequest request = new HTTPRequest(new Uri(url), (req, resp) =>
            {
                if (req.State == HTTPRequestStates.Finished)
                {
                    HTTPHandler handler = OnRequestFinished(req, resp);
                    callback.Invoke(handler);
                }
                else
                {
                    HTTPHandler handler = OnRequestFailed(req, resp);
                    callback.Invoke(handler);
                }
            });
            request.ConnectTimeout = TimeSpan.FromSeconds(HttpConnectTimeout);
            request.Timeout = TimeSpan.FromSeconds(HttpTimeout);
            request.Send();
        }

        public static async UniTask<HTTPHandler> Get(string url)
        {
            HTTPHandler handler = new HTTPHandler();
            try
            {
                HTTPRequest request = new HTTPRequest(new Uri(url), HTTPMethods.Get);
                request.ConnectTimeout = TimeSpan.FromSeconds(HttpConnectTimeout);
                request.Timeout = TimeSpan.FromSeconds(HttpTimeout);
                await request.Send();
                if (request.State == HTTPRequestStates.Finished)
                {
                    handler = OnRequestFinished(request, request.Response);
                }
                else
                {
                    handler = OnRequestFailed(request, request.Response);
                }
            }
            catch(Exception ex)
            {
                handler.isDone = true;
                handler.isError = true;
                handler.isTimeout = false;
                handler.msg = ex.ToString();
            }
            return handler;
        }

        public static void Post(string url, HTTPFormBase form, HttpCallback callback)
        {
            HTTPRequest request = new HTTPRequest(new Uri(url), HTTPMethods.Post, (req, resp) =>
            {
                if (req.State == HTTPRequestStates.Finished)
                {
                    HTTPHandler handler = OnRequestFinished(req, resp);
                    callback.Invoke(handler);
                }
                else
                {
                    HTTPHandler handler = OnRequestFailed(req, resp);
                    callback.Invoke(handler);
                }
            });
            request.ConnectTimeout = TimeSpan.FromSeconds(HttpConnectTimeout);
            request.Timeout = TimeSpan.FromSeconds(HttpTimeout);
            request.SetForm(form);
            request.Send();
        }

        public static async UniTask<HTTPHandler> Post(string url, HTTPFormBase form)
        {
            HTTPHandler handler = new HTTPHandler();
            try
            {
                HTTPRequest request = new HTTPRequest(new Uri(url), HTTPMethods.Post);
                request.ConnectTimeout = TimeSpan.FromSeconds(HttpConnectTimeout);
                request.Timeout = TimeSpan.FromSeconds(HttpTimeout);
                request.SetForm(form);
                await request.Send();
                if (request.State == HTTPRequestStates.Finished)
                {
                    handler = OnRequestFinished(request, request.Response);
                }
                else
                {
                    handler = OnRequestFailed(request, request.Response);
                }
            }
            catch(Exception ex)
            {
                handler.isDone = true;
                handler.isError = true;
                handler.isTimeout = false;
                handler.msg = ex.ToString();
            }
            return handler;
        }

        static HTTPHandler OnRequestFinished(HTTPRequest req, HTTPResponse resp)
        {
            Huge.Debug.Log(
                "Request Finished Successfully!\n" +
                resp.DataAsText);

            HTTPHandler handler = new HTTPHandler();
            handler.isDone = true;
            handler.isError = false;
            handler.isTimeout = false;
            handler.buffer = resp.Data;
            handler.text = resp.DataAsText;
            return handler;
        }

        static HTTPHandler OnRequestFailed(HTTPRequest req, HTTPResponse resp)
        {
            HTTPHandler handler = new HTTPHandler();
            handler.isError = true;
            handler.isDone = true;

            switch (req.State)
            {
                case HTTPRequestStates.Error:
                    handler.msg = "HTTP Request Error! " +
                        (req.Exception != null ?
                            (req.Exception.Message + "\n" +
                        req.Exception.StackTrace) :
                            "No Exception");
                    Huge.Debug.LogError(handler.msg);
                    break;
                case HTTPRequestStates.Aborted:
                    handler.msg = "Request Aborted!";
                    Huge.Debug.LogWarning("Request Aborted!");
                    break;
                case HTTPRequestStates.ConnectionTimedOut:
                    handler.isTimeout = true;
                    handler.msg = "Connection Timed Out!";
                    Huge.Debug.LogWarning("Connection Timed Out!");
                    break;
                case HTTPRequestStates.TimedOut:
                    handler.msg = "Processing the request Timed Out!";
                    Huge.Debug.LogWarning("Processing the request Timed Out!");
                    break;
            }

            return handler;
        }
    }
}
