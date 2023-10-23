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

            // 显示主题标题
            SubjectText.text = Global.Subject.Name;

            CreateOptionResultHolder();

            SaveBtn.onClick.AddListener(() =>
            {
                AudioKit.PlaySound(Sfx.CLICK);

                Global.Subject.IsHistory = true;

                foreach (var option in Global.Subject.Options)
                {
                    Debug.Log("Global.Subject.Options：" + option.Name);
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
            // 使用LINQ对 Global.Subject.Options 列表按 Score 从大到小排序
            List<Option> sortedOptions = Global.Subject.Options.OrderByDescending(o => o.Score).ToList();

            for (int i = 0; i < sortedOptions.Count; i++)
            {
                OptionResultHolderTemplete.InstantiateWithParent(Content)
                    .SiblingIndex(Content.childCount - 3)
                    .Self(optionHolder =>
                    {
                        // 显示选项信息
                        optionHolder.OptionLabel.text = "No." + (i + 1) + "\r\n" +
                            "<b><size=54>" + sortedOptions[i].Name + "</size></b>";
                        // 显示选项得分
                        optionHolder.OptionScoreText.text = "<b>总分：" + sortedOptions[i].Score.ToString("F1") + "</b>" + " / " + sortedOptions[i].MaxScore.ToString("F1");
                        // 显示选项百分比
                        optionHolder.OptionPercentText.text = (sortedOptions[i].Percent * 100f).ToString("F1") + "%";

                        // 对选项的 Focuses 按分数大小排序
                        var sortedFocuses = sortedOptions[i].Focuses.OrderByDescending(f => f.Score).ToList();

                        foreach(Focus focus in sortedOptions[i].Focuses)
                        {
                            FocusScoreHolderTemplete.InstantiateWithParent(optionHolder)
                                .Self(focusHolder =>
                                {
                                    // 显示关注点得分
                                    focusHolder.FocusScoreText.text = "<b>" + focus.Name + "：" +
                                        focus.Score.ToString("F1") + "</b>" + " / " + focus.MaxScore.ToString("F1");
                                    // 显示关注点百分比
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
