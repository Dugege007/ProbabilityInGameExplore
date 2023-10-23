using UnityEngine;
using QFramework;
using System.Linq;
using QAssetBundle;
using System.Collections.Generic;

namespace ProbabilityTest
{
    public class UIResultPanelData : UIPanelData
    {
    }
    public partial class UIResultPanel : UIPanel, IController, ICanSave
    {
        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIResultPanelData ?? new UIResultPanelData();
            // please add init code here

            OptionResultHolderTemplete.Hide();
            FocusScoreHolderTemplete.Hide();

            // ��ʾ�������
            SubjectText.text = Global.Subject.Name;

            CreateOptionResultHolder();

            SaveBtn.onClick.AddListener(() =>
            {
                AudioKit.PlaySound(Sfx.CLICK);

                Global.Subject.IsHistory = true;

                foreach (var option in Global.Subject.Options)
                {
                    Debug.Log("Global.Subject.Options��" + option.Name);
                }

                this.GetSystem<SaveSystem>().SaveObject(Global.Subject,"Subject/", Global.Subject.Name, () =>
                {
                    CloseSelf();
                    UIKit.OpenPanel<UIHistoryPanel>();
                });
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

                        foreach(Focus focus in sortedOptions[i].Focuses)
                        {
                            FocusScoreHolderTemplete.InstantiateWithParent(optionHolder)
                                .Self(focusHolder =>
                                {
                                    // ��ʾ��ע��÷�
                                    focusHolder.FocusScoreText.text = "<b>" + focus.Name + "��" +
                                        focus.Score.ToString("F1") + "</b>" + " / " + focus.MaxScore.ToString("F1");
                                    // ��ʾ��ע��ٷֱ�
                                    focusHolder.FocusPercentText.text = (focus.Percent * 100f).ToString("F1") + "%";
                                })
                                .Show();
                        }
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

        public void Save()
        {
            
        }

        public void Load()
        {
            
        }

        public IArchitecture GetArchitecture()
        {
            return Global.Interface;
        }
    }
}
