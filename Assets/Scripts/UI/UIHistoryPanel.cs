using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System.Collections.Generic;
using QAssetBundle;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System;

namespace ProbabilityTest
{
    public class UIHistoryPanelData : UIPanelData
    {
    }
    public partial class UIHistoryPanel : UIPanel, IController, ICanSave
    {
        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIHistoryPanelData ?? new UIHistoryPanelData();
            // please add init code here

            HistoryBtnTemplete.Hide();

            Global.ResetData();
            TraverseFolders();

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

        private void CreateSubjectHistoryBtn(Subject subject)
        {
            HistoryBtnTemplete.InstantiateWithParent(Content)
                .Self(self =>
                {
                    self.SubjectTitle.text = subject.Name;

                    StringBuilder optionsStr = new StringBuilder("ѡ�");
                    foreach (Option option in subject.Options)
                        optionsStr.Append(option.Name).Append("��");
                    self.OptionsText.text = optionsStr.ToString();

                    StringBuilder focusesStr = new StringBuilder("��ע�㣺");
                    foreach (Focus focus in subject.Options[0].Focuses)
                        focusesStr.Append(focus.Name).Append("��");
                    self.FocusesText.text = focusesStr.ToString();

                    self.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        AudioKit.PlaySound(Sfx.CLICK);

                        Global.Subject = subject;

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

        private void TraverseFolders()
        {
            string subjectPath = Path.Combine(Application.persistentDataPath, "Subject/");

            try
            {
                // ��ȡ�����ļ���
                string[] subjectFiles = Directory.GetFiles(subjectPath);

                // ��ȡÿ���ļ�
                foreach (string file in subjectFiles)
                {
                    Debug.Log("Reading file: " + file);

                    // ��ȡ�ļ�����
                    string json = File.ReadAllText(subjectPath);
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

        public IArchitecture GetArchitecture()
        {
            return Global.Interface;
        }

        public void Save()
        {

        }

        public void Load()
        {

        }
    }
}
