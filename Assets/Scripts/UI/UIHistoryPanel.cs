using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System.Collections.Generic;
using QAssetBundle;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Linq;

namespace ProbabilityTest
{
    public class UIHistoryPanelData : UIPanelData
    {
    }
    public partial class UIHistoryPanel : UIPanel, IController, ICanSave
    {
        private List<string> mSubjectFiles = new List<string>();
        private List<HistoryBtnTemplete> mHistoryBtns = new List<HistoryBtnTemplete>(); 

        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIHistoryPanelData ?? new UIHistoryPanelData();
            // please add init code here

            HistoryBtnTemplete.Hide();
            NotificationOpenHistory.Hide();
            NotificationClearAllHistory.Hide();

            Load();

            NewSubjectBtn.onClick.AddListener(() =>
            {
                AudioKit.PlaySound(Sfx.CLICK);

                Global.ResetData();

                CloseSelf();
                Global.IsTemporarilySave.Value = false;
                UIKit.OpenPanel<UIStartPanel>();
            });

            ClearAllHistoryBtn.onClick.AddListener(() =>
            {
                NotificationClearAllHistory.Show();
            });

            NotificationClearAllHistory.CloseBtn.onClick.AddListener(() =>
            {
                NotificationClearAllHistory.Hide();
            });

            NotificationClearAllHistory.CancelBtn.onClick.AddListener(() =>
            {
                NotificationClearAllHistory.Hide();
            });

            NotificationClearAllHistory.YesBtn.onClick.AddListener(() =>
            {
                DeleteAllHistory();
                CloseSelf();
                UIKit.OpenPanel<UIHomePanel>();
            });

            //NotificationOpenHistory.CloseBtn.onClick.AddListener(() =>
            //{
            //    NotificationOpenHistory.Hide();
            //});

            //NotificationOpenHistory.CancelBtn.onClick.AddListener(() =>
            //{
            //    NotificationOpenHistory.Hide();
            //});

            //NotificationOpenHistory.YesBtn.onClick.AddListener(() =>
            //{
            //    Global.ResetData();

            //    CloseSelf();
            //    Global.IsTemporarilySave.Value = false;
            //    UIKit.OpenPanel<UIStartPanel>();
            //});
        }

        protected override void OnOpen(IUIData uiData = null)
        {
        }

        protected override void OnShow()
        {
        }

        private void CreateSubjectHistoryBtn(Subject historySubject)
        {
            HistoryBtnTemplete.InstantiateWithParent(Content)
                .Self(self =>
                {
                    self.SubjectTitle.text = historySubject.Name;

                    if (historySubject.SampleSpace != null)
                        self.DateText.text = historySubject.SampleSpace.Date;
                    else
                        self.DateText.text = "";

                    StringBuilder descriptionStr = new StringBuilder("说明：");
                    descriptionStr.Append(historySubject.Description);
                    self.DescriptionText.text = descriptionStr.ToString();

                    mHistoryBtns.Add(self);

                    self.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        AudioKit.PlaySound(Sfx.CLICK);

                        Global.HistorySubject = historySubject;

                        CloseSelf();
                        UIKit.OpenPanel<UIStartPanel>();
                    });
                })
                .Show();
        }

        protected override void OnHide()
        {
        }

        protected override void OnClose()
        {
        }

        private void DeleteAllHistory()
        {
            string subjectPath = Path.Combine(Application.persistentDataPath, "Subject/");

            try
            {
                // 获取所有文件名
                string[] subjectFiles = Directory.GetFiles(subjectPath);

                if (subjectFiles.Length > 0)
                {
                    // 删除每个文件
                    foreach (string file in subjectFiles)
                    {
                        Debug.Log("Deleting file: " + file);
                        File.Delete(file);
                    }
                }
            }
            catch (Exception e)
            {
                // 异常处理
                Debug.Log("An error occurred: " + e.Message);
            }
        }

        public IArchitecture GetArchitecture()
        {
            return Global.Interface;
        }

        public void Save()
        {

        }

        public void Load()
        {
            string subjectPath = Path.Combine(Application.persistentDataPath, "Subject/");

            try
            {
                // 获取所有文件名
                mSubjectFiles = Directory.GetFiles(subjectPath).ToList();

                // 读取每个文件
                foreach (string file in mSubjectFiles)
                {
                    Debug.Log("Reading file: " + file);

                    // 读取文件内容
                    string json = File.ReadAllText(file);  // 注意这里改为 file
                    Subject subject = JsonConvert.DeserializeObject<Subject>(json);

                    CreateSubjectHistoryBtn(subject);
                }
            }
            catch (Exception e)
            {
                // 异常处理
                Debug.Log("An error occurred: " + e.Message);
            }
        }
    }
}
