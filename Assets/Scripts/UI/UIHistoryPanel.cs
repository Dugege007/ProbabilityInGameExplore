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

        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIHistoryPanelData ?? new UIHistoryPanelData();
            // please add init code here

            HistoryBtnTemplete.Hide();

            Load();

            NewSubjectBtn.onClick.AddListener(() =>
            {
                AudioKit.PlaySound(Sfx.CLICK);

                Global.ResetData();

                CloseSelf();
                Global.IsTemporarilySave.Value = false;
                UIKit.OpenPanel<UIStartPanel>();
            });
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

                    StringBuilder descriptionStr = new StringBuilder("˵����");
                    descriptionStr.Append(historySubject.Description);
                    self.DescriptionText.text = descriptionStr.ToString();

                    //StringBuilder optionsStr = new StringBuilder("ѡ�");
                    //foreach (Option option in historySubject.Options)
                    //    optionsStr.Append(option.Name).Append("��");
                    //self.OptionsText.text = optionsStr.ToString();

                    //StringBuilder focusesStr = new StringBuilder("��ע�㣺");
                    //foreach (Focus focus in historySubject.Options[0].Focuses)
                    //    focusesStr.Append(focus.Name).Append("��");
                    //self.FocusesText.text = focusesStr.ToString();

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

        private void DeleteHistory(List<string> needDeleteFiles)
        {
            foreach (string filePath in needDeleteFiles)
            {

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
                // ��ȡ�����ļ���
                mSubjectFiles = Directory.GetFiles(subjectPath).ToList();

                // ��ȡÿ���ļ�
                foreach (string file in mSubjectFiles)
                {
                    Debug.Log("Reading file: " + file);

                    // ��ȡ�ļ�����
                    string json = File.ReadAllText(file);  // ע�������Ϊ file
                    Subject subject = JsonConvert.DeserializeObject<Subject>(json);

                    CreateSubjectHistoryBtn(subject);
                }
            }
            catch (Exception e)
            {
                // �쳣����
                Debug.Log("An error occurred: " + e.Message);
            }
        }
    }
}
