using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System.Linq;

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
        }

        protected override void OnOpen(IUIData uiData = null)
        {
        }

        protected override void OnShow()
        {
        }

        private void CreateOptionResultHolder()
        {
            // 使用LINQ对 Global.Subject.Options 列表按 Score 从大到小排序
            var sortedOptions = Global.Subject.Options.OrderByDescending(o => o.Score).ToList();

            for (int i = 0; i < sortedOptions.Count; i++)
            {
                OptionResultHolderTemplete.InstantiateWithParent(Content)
                    .SiblingIndex(Content.childCount - 3)
                    .Self(optionHolder =>
                    {
                        optionHolder.OptionLabel.text = "No." + (i + 1) +"\r\n" +
                            "<b><size=54>" + sortedOptions[i].Name + "</size></b>";
                        optionHolder.OptionScoreText.text = "<b>总分：</b>" + sortedOptions[i].Score.ToString("F1");
                        optionHolder.OptionPercentText.text = (sortedOptions[i].Score / Global.Subject.GetTotalScore() * 100f).ToString("F1") + "%";

                        var sortedFocuses = sortedOptions[i].Focuses.OrderByDescending(f => f.Score).ToList();

                        for (int j = 0; j < sortedOptions[i].Focuses.Count; j++)
                        {
                            FocusScoreHolderTemplete.InstantiateWithParent(optionHolder)
                                .Self(focusHolder =>
                                {
                                    focusHolder.FocusScoreText.text = "<b>" + sortedFocuses[j].Name + "：</b>" + 
                                        sortedFocuses[j].Score.ToString("F1");
                                    focusHolder.FocusPercentText.text = (sortedFocuses[j].Score / (Global.SampleSpace.GetSamplePointByName(sortedFocuses[j].Name).Value * 10f) * 100f).ToString("F1") + "%";
                                })
                                .Show();
                        }
                        CreateFocusResultHolder(optionHolder);
                    })
                    .Show();
            }
        }

        private void CreateFocusResultHolder(OptionResultHolderTemplete optionHolder)
        {

        }

        protected override void OnHide()
        {
        }

        protected override void OnClose()
        {
        }
    }
}
