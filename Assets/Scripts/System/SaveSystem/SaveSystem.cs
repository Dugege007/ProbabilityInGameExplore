using Newtonsoft.Json;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace ProbabilityTest
{
    public class SaveSystem : AbstractSystem
    {
        public HashSet<string> Keys = new HashSet<string>();

        protected override void OnInit()
        {
            ActionKit.OnGUI.Register(() =>
            {
                if (Input.GetKey(KeyCode.L))
                {
                    // 展示数据
                    foreach (var key in Keys)
                    {
                        GUILayout.Label(key + ": " + PlayerPrefs.GetInt(key));
                        GUILayout.Label(key + ": " + PlayerPrefs.GetFloat(key));
                        GUILayout.Label(key + ": " + PlayerPrefs.GetString(key));
                    }
                }
            });
        }

        public void SaveObject<T>(T obj, string folder, string objName, Action onComplete)
        {
            string fileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + "_" + objName + ".json"; // 替换冒号和空格
            string folderPath = Path.Combine(Application.persistentDataPath, folder);
            string filePath = Path.Combine(folderPath, fileName);

            // 确保目录存在
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // 创建并写入文件
            File.WriteAllText(filePath, JsonConvert.SerializeObject(obj, Formatting.Indented));
            Debug.Log(obj + "已保存" + JsonConvert.SerializeObject(obj, Formatting.Indented));

#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif

            onComplete?.Invoke(); // 调用回调
        }

        public void LoadObject<T>(T obj, string fileName)
        {
            string path = Path.Combine(Application.persistentDataPath, fileName);
            if (File.Exists(path) == false)
                return;
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                obj = JsonConvert.DeserializeObject<T>(json);
            }
        }

        public void SaveBool(string key, bool value)
        {
            Keys.Add(key);
            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }

        public bool LoadBool(string key, bool defaultValue = false)
        {
            Keys.Add(key);
            return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
        }

        public void SaveInt(string key, int value)
        {
            Keys.Add(key);
            PlayerPrefs.SetInt(key, value);
        }

        public int LoadInt(string key, int defaultValue = 0)
        {
            Keys.Add(key);
            return PlayerPrefs.GetInt(key, defaultValue);
        }

        public void SaveFloat(string key, float value)
        {
            Keys.Add(key);
            PlayerPrefs.SetFloat(key, value);
        }

        public float LoadFloat(string key, float defaultValue = 0f)
        {
            Keys.Add(key);
            return PlayerPrefs.GetFloat(key, defaultValue);
        }

        public void SaveString(string key, string value)
        {
            Keys.Add(key);
            PlayerPrefs.SetString(key, value);
        }

        public string LoadString(string key, string defaultValue = default)
        {
            Keys.Add(key);
            return PlayerPrefs.GetString(key, defaultValue);
        }

    }
}
