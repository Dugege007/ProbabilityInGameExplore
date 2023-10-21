using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System.Linq;
using QAssetBundle;
using System.Collections.Generic;

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

            OptionResultHolderTemplete.Hide();
            FocusScoreHolderTemplete.Hide();

            CreateOptionResultHolder();

            BackToHomeBtn.onClick.AddListener(() =>
            {
                AudioKit.PlaySound(Sfx.CLICK);
            });

            ShareBtn.onClick.AddListener(() =>
            {
                AudioKit.PlaySound(Sfx.CLICK);
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
                        optionHolder.OptionLabel.text = "No." + (i + 1) +"\r\n" +
                            "<b><size=54>" + sortedOptions[i].Name + "</size></b>";
                        // ��ʾѡ��÷�
                        optionHolder.OptionScoreText.text = "<b>�ܷ֣�"  + sortedOptions[i].Score.ToString("F1") + "</b>" + " / " + sortedOptions[i].MaxScore.ToString("F1");
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
                                    focusHolder.FocusScoreText.text = "<b>" + sortedFocuses[j].Name + 
                                        sortedFocuses[j].Score.ToString("F1") + "��</b>" + " / " + sortedFocuses[j].MaxScore.ToString("F1");
                                    // ��ʾ��ע��ٷֱ�
                                    focusHolder.FocusPercentText.text = (sortedFocuses[j].Percent * 100f).ToString("F1") + "%";
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
    }
}
