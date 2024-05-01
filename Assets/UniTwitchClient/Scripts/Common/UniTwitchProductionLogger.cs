using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UniTwitchClient.Common
{
    public class UniTwitchProductionLogger : IUniTwitchLogger
    {
        public void Log(string message)
        {

        }

        public void LogError(string message)
        {
            WriteToFile(message);
        }

        private void WriteToFile(string messageRaw) 
        {
            var time = DateTime.Now;
            var fileName = string.Format("UniTwitchClient_errorlog{0}{1}{2}.txt", time.ToString("yyyy"), time.ToString("MM"), time.ToString("dd"));
            var root = Application.persistentDataPath;
            var directoryPath = $"{root}/UniTwitchClient_ErrorLog";
            var path = $"{directoryPath}/{fileName}";
            Debug.Log("write to file. path:" + path);

            var timeStamp = string.Format("[{0}:{1}:{2}] ", time.ToString("HH"), time.ToString("mm"), time.ToString("ss"));
            var message = timeStamp + messageRaw;

            if (File.Exists(path))
            {
                var data = File.ReadAllText(path);
                if (string.IsNullOrEmpty(data))
                {
                    File.WriteAllText(path, message);
                }
                else
                {
                    File.WriteAllText(path, data + Environment.NewLine + message);
                }
            }
            else 
            {
                Directory.CreateDirectory(directoryPath);
                File.WriteAllText(path, message);
            }
        }
    }
}