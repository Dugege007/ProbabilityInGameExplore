using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System.Linq;
using QAssetBundle;
using System.Collections.Generic;
using TMPro;
using Newtonsoft.Json;
using UnityEditor;
using System.IO;

namespace ProbabilityTest
{
    public class UIResultPanelData : UIPanelData
    {
    }
    public partial class UIResultPanel : UIPanel
    {
        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIResultPanelData ?? new UIResultPanelData();
            // please add init code here

            foreach (var option in Global.Subject.Options)
            {
                Debug.Log("Global.Subject.Options��" + option.Name);
            }

            foreach (var point in Global.SampleSpace.SamplePoints)
            {
                Debug.Log("Global.SampleSpace.SamplePoints��" + point.Name);
            }

            OptionResultHolderTemplete.Hide();
            FocusScoreHolderTemplete.Hide();

            // ��ʾ�������
            SubjectText.text = Global.Subject.Name;

            CreateOptionResultHolder();

            SaveBtn.onClick.AddListener(() =>
            {
                AudioKit.PlaySound(Sfx.CLICK);

                Global.Subject.IsHistory = true;
                Global.Subject.Date = System.DateTime.Now.ToString("G");
                Global.SampleSpace.Date = Global.Subject.Date;

                SaveHistory(Global.SubjectHistory, "SubjectHistory.json", Global.Subject, Global.Subject.Key);
                SaveHistory(Global.SampleSpaceHistory, "SampleSpaceHistory.json", Global.SampleSpace, Global.SampleSpace.Key);

                CloseSelf();
                UIKit.OpenPanel<UIHistoryPanel>();
            });

            BackToHomeBtn.onClick.AddListener(() =>
            {
                AudioKit.PlaySound(Sfx.CLICK);

                CloseSelf();
                UIKit.OpenPanel<UIHomePanel>();
            });
        }

        protected override void OnOpen(IUIData uiData = null)
        {
        }

        protected override void OnShow()
        {
        }

        private void CreateOptionResultHolder()
        {
            // ʹ��LINQ�� Global.Subject.Options �б� Score �Ӵ�С����
            List<Option> sortedOptions = Global.Subject.Options.OrderByDescending(o => o.Score).ToList();

            for (int i = 0; i < sortedOptions.Count; i++)
            {
                OptionResultHolderTemplete.InstantiateWithParent(Content)
                    .SiblingIndex(Content.childCount - 3)
                    .Self(optionHolder =>
                    {
                        // ��ʾѡ����Ϣ
                        optionHolder.OptionLabel.text = "No." + (i + 1) + "\r\n" +
                            "<b><size=54>" + sortedOptions[i].Name + "</size></b>";
                        // ��ʾѡ��÷�
                        optionHolder.OptionScoreText.text = "<b>�ܷ֣�" + sortedOptions[i].Score.ToString("F1") + "</b>" + " / " + sortedOptions[i].MaxScore.ToString("F1");
                        // ��ʾѡ��ٷֱ�
                        optionHolder.OptionPercentText.text = (sortedOptions[i].Percent * 100f).ToString("F1") + "%";

                        // ��ѡ��� Focuses ��������С����
                        var sortedFocuses = sortedOptions[i].Focuses.OrderByDescending(f => f.Score).ToList();

                        for (int j = 0; j < sortedOptions[i].Focuses.Count; j++)
                        {
                            FocusScoreHolderTemplete.InstantiateWithParent(optionHolder)
                                .Self(focusHolder =>
                                {
                                    // ��ʾ��ע��÷�
                                    focusHolder.FocusScoreText.text = "<b>" + sortedFocuses[j].Name + "��" +
                                        sortedFocuses[j].Score.ToString("F1") + "</b>" + " / " + sortedFocuses[j].MaxScore.ToString("F1");
                                    // ��ʾ��ע��ٷֱ�
                                    focusHolder.FocusPercentText.text = (sortedFocuses[j].Percent * 100f).ToString("F1") + "%";
                                })
                                .Show();
                        }
                    })
                    .Show();
            }
        }

        // ���ݱ���
        private void SaveHistory<T>(Dictionary<string, T> history, string historyFileName, T obj, string objKey)
        {
            if (history.ContainsKey(objKey) == false)
                history.Add(objKey, obj);
            else
                history[objKey] = obj;

            //string jsonData = JsonConvert.SerializeObject(history);
            string path = Path.Combine(Application.persistentDataPath, historyFileName);
            File.WriteAllText(path, JsonConvert.SerializeObject(history));
            //string path = Application.dataPath + "/Data/your_file.json";
            if (File.Exists(path) == false)
                File.Create(path);
            File.WriteAllText(path, JsonConvert.SerializeObject(history));
            AssetDatabase.Refresh();

            //// PlayerPrefs.SetString() ���Խ���Ϸ���ݺ�Json�����Լ�ֵ�Ե���ʽƥ�䣬�������ڴ�����
            //PlayerPrefs.SetString(historyKey, jsonData);
            //// ��������
            //PlayerPrefs.Save();
        }

        //private void SaveHistory<T>(Dictionary<string, T> history, T obj, string key)
        //{
        //    history.Add(key, obj);

        //    string jsonData = JsonConvert.SerializeObject(obj);
        //    // PlayerPrefs.SetString() ���Խ���Ϸ���ݺ�Json�����Լ�ֵ�Ե���ʽƥ�䣬�������ڴ�����
        //    PlayerPrefs.SetString(key, jsonData);
        //    // ��������
        //    PlayerPrefs.Save();
        //}

        protected override void OnHide()
        {
        }

        protected override void OnClose()
        {
        }
    }
}
