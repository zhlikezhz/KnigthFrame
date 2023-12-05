using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Networking;
using ICSharpCode.SharpZipLib.Zip;

namespace Huge.Utils
{
    public static class FileUtility
    {
        public static bool CopyDirectory(string srcPath, string destPath, bool lowerPath = false, string dirPattern = "*", string filePattern = "*.*")
        {
            CreateDirectoryIfNotExist(destPath);
            try
            {
                foreach (string dirPath in Directory.GetDirectories(srcPath, dirPattern, SearchOption.AllDirectories))
                {
                    var realDirPath = dirPath.Replace(srcPath, destPath);
                    realDirPath = lowerPath ? realDirPath.ToLower() : realDirPath;
                    CreateDirectoryIfNotExist(realDirPath);
                }

                foreach (string newPath in Directory.GetFiles(srcPath, filePattern, SearchOption.AllDirectories))
                {
                    var realNewPath = newPath.Replace(srcPath, destPath);
                    realNewPath = lowerPath ? realNewPath.ToLower() : realNewPath;
                    File.Copy(newPath, realNewPath, true);
                }
            }
            catch (Exception ex)
            {
                Huge.Debug.LogError(ex);
                return false;
            }

            return true;
        }

        public static void DeleteFilesFromDir(string dir, Func<string, bool> checkFunc)
        {
            foreach (string newPath in Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories))
            {
                if (checkFunc(newPath))
                {
                    File.Delete(newPath);
                    Huge.Debug.LogFormat("delete file:{0}", newPath);
                }
            }
        }

        public static void DeleteTopFileOrDirectoryFromDir(string dir, Func<string, bool, bool> checkFunc)
        {
            foreach (string topDir in Directory.GetDirectories(dir, "*", SearchOption.TopDirectoryOnly))
            {
                if (checkFunc(topDir, false))
                {
                    DeleteExistDirectory(topDir);
                    Huge.Debug.LogFormat("delete dir:{0}", topDir);
                }
            }

            foreach (string newPath in Directory.GetFiles(dir, "*.*", SearchOption.TopDirectoryOnly))
            {
                if (checkFunc(newPath, true))
                {
                    File.Delete(newPath);
                    Huge.Debug.LogFormat("delete file:{0}", newPath);
                }
            }
        }

        public static void DeleteExistFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public static void DeleteExistDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        public static bool IsDirectory(string path)
        {
            FileAttributes attr = File.GetAttributes(path);

            return (attr & FileAttributes.Directory) == FileAttributes.Directory;
        }

        public static void CreateDirectoryIfNotExist(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static void CreateFileDirectoryIfNoExist(string filePath)
        {
            CreateDirectoryIfNotExist(Path.GetDirectoryName(filePath));
        }

        public static long GetFileSize(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            return fileInfo.Length;
        }

        public static string GetFileMD5(string filePath, bool isLowerCase = true)
        {
            // 使用string方式计算二进制文件的hash可能会有问题，改用bytes
            var hash = MD5.Create();
            byte[] bytes = File.ReadAllBytes(filePath);
            bytes = hash.ComputeHash(bytes);
            var hashStr = BitConverter.ToString(bytes).Replace("-", "");
            if (isLowerCase)
            {
                hashStr = hashStr.ToLower();
            }
            return hashStr;
        }

        public static void Zip(string srcPath, string destPath)
        {
            if (File.Exists(destPath))
            {
                File.Delete(destPath);
            }
            if (Directory.Exists(srcPath))
            {
                ZipConstants.DefaultCodePage = Encoding.UTF8.CodePage;
                FastZip fastZip = new FastZip();
                fastZip.CreateZip(destPath, srcPath, true, "");
            }
        }

        public static void Unzip(string zipFile, string toDir)
        {
            if (File.Exists(zipFile))
            {
                ZipConstants.DefaultCodePage = Encoding.UTF8.CodePage;
                FastZip fastZip = new FastZip();
                fastZip.ExtractZip(zipFile, toDir, "");
            }
        }
    }
}
