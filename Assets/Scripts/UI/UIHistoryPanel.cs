using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System.Collections.Generic;
using QAssetBundle;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace ProbabilityTest
{
    public class UIHistoryPanelData : UIPanelData
    {
    }
    public partial class UIHistoryPanel : UIPanel
    {
        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIHistoryPanelData ?? new UIHistoryPanelData();
            // please add init code here

            HistoryBtnTemplete.Hide();

            LoadSubjectHistory();
            LoadSampleSpaceHistory();
            Global.ResetData();

            CreateHistoryBtn(Global.SubjectHistory, Global.SampleSpaceHistory);

            NewSubjectBtn.onClick.AddListener(() =>
            {
                AudioKit.PlaySound(Sfx.CLICK);

                Global.ResetData();

                CloseSelf();
                UIKit.OpenPanel<UIStartPanel>();
            });
        }

        protected override void OnOpen(IUIData uiData = null)
        {
        }

        protected override void OnShow()
        {
        }

        private void LoadSampleSpaceHistory()
        {
            //string path = Application.dataPath + "/Data/your_file.json";
            //if (File.Exists(path) == false)
            //    return;

            string path = Path.Combine(Application.persistentDataPath, "SampleSpaceHistory.json");
            if (File.Exists(path) == false)
                return;

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                Global.SampleSpaceHistory = JsonConvert.DeserializeObject<Dictionary<string, SampleSpace>>(json);
            }
        }

        private void LoadSubjectHistory()
        {
            //string path = Application.dataPath + "/Data/your_file.json";
            //if (File.Exists(path) == false)
            //    return;

            string path = Path.Combine(Application.persistentDataPath, "SubjectHistory.json");
            if (File.Exists(path) == false)
                return;

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                Global.SubjectHistory = JsonConvert.DeserializeObject<Dictionary<string, Subject>>(json);
            }
        }

        // 使用 Newtonsoft.Json 反序列化
        //private void LoadSampleSpace(string key)
        //{
        //    if (PlayerPrefs.HasKey(key))
        //    {
        //        string json = PlayerPrefs.GetString(key);
        //        Global.SampleSpace = JsonConvert.DeserializeObject<SampleSpace>(json);
        //    }
        //}

        //private void LoadSubject(string key)
        //{
        //    if (PlayerPrefs.HasKey(key))
        //    {
        //        string json = PlayerPrefs.GetString(key);
        //        Global.Subject = JsonConvert.DeserializeObject<Subject>(json);
        //    }
        //}

        //private void LoadSampleSpaceHistory()
        //{
        //    if (PlayerPrefs.HasKey(Global.SampleSpaceHistoryKey))
        //    {
        //        string json = PlayerPrefs.GetString(Global.SampleSpaceHistoryKey);
        //        Global.SampleSpaceHistory = JsonConvert.DeserializeObject<Dictionary<string, SampleSpace>>(json);
        //    }
        //}

        //private void LoadSubjectHistory()
        //{
        //    if (PlayerPrefs.HasKey(Global.SubjectHistoryKey))
        //    {
        //        string json = PlayerPrefs.GetString(Global.SubjectHistoryKey);
        //        Global.SubjectHistory = JsonConvert.DeserializeObject<Dictionary<string, Subject>>(json);
        //    }
        //}

        //private void LoadSampleSpaceHistory()
        //{
        //    if (PlayerPrefs.HasKey(Global.SampleSpaceHistoryKey))
        //    {
        //        JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(Global.SampleSpaceHistoryKey), Global.SampleSpaceHistory);
        //    }
        //}

        //private void LoadSubjectHistory()
        //{
        //    if (PlayerPrefs.HasKey(Global.SubjectHistoryKey))
        //    {
        //        JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(Global.SubjectHistoryKey), Global.SubjectHistory);
        //    }
        //}

        private void CreateHistoryBtn(Dictionary<string, Subject> subjectHistory, Dictionary<string, SampleSpace> sampleSpaceHistory)
        {
            foreach (Subject subject in subjectHistory.Values)
            {
                HistoryBtnTemplete.InstantiateWithParent(Content)
                    .Self(self =>
                    {
                        self.SubjectTitle.text = subject.Name;
                        self.DateText.text = subject.Date;

                        StringBuilder optionsStr = new StringBuilder("选项：");
                        foreach (Option option in subject.Options)
                            optionsStr.Append(option.Name).Append("、");
                        self.OptionsText.text = optionsStr.ToString();

                        StringBuilder focusesStr = new StringBuilder("关注点：");
                        foreach (Focus focus in subject.Options[0].Focuses)
                            focusesStr.Append(focus.Name).Append("、");
                        self.FocusesText.text = focusesStr.ToString();

                        self.GetComponent<Button>().onClick.AddListener(() =>
                        {
                            AudioKit.PlaySound(Sfx.CLICK);

                            Global.Subject = subject;
                            sampleSpaceHistory.TryGetValue(subject.Key, out Global.SampleSpace);

                            CloseSelf();
                            UIKit.OpenPanel<UIStartPanel>();
                        });
                    })
                    .Show();
            }
        }

        protected override void OnHide()
        {
        }

        protected override void OnClose()
        {
        }
    }
}
