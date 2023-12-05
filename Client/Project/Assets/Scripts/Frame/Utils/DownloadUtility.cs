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

namespace Huge.Utils
{
    public class DownloadHandler    
    {
        public string msg = "";
        public bool isDone = false;
        public bool isError = false;
        public bool isTimeout = false;
        public byte[] buffer = null;
        public float progress = 0.0f;

        public DownloadHandler()
        {
            msg = "";
            buffer = null;
            isDone = false;
            isError = false;
            isTimeout = false;
            progress = 0.0f;
        }
    }

    public static class DownloadUtility
    {
        public static void Download(string url, HTTPUtility.HttpCallback callback)
        {
            HTTPUtility.Get(url, callback);
        }
    }
}
