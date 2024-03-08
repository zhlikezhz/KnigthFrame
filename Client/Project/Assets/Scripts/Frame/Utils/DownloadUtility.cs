using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
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
        public static async UniTask Download(string url, string filePath, int timeout = 60)
        {
            var handler = await HTTPUtility.Get(url);
            if (handler.isError)
            {
                throw new Exception(handler.msg);
            }
            await File.WriteAllBytesAsync(filePath, handler.buffer);
        }
    }
}
