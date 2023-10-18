using UnityEngine;
using QFramework;
using System.Collections.Generic;

namespace ProbabilityTest
{
    public class UIRatingPanelData : UIPanelData
    {
    }
    public partial class UIRatingPanel : UIPanel
    {
        private int mOptionIndex = 0;

        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIRatingPanelData ?? new UIRatingPanelData();
            // please add init code here

            RatingHolderTemplete.Hide();
            ComputeBtn.enabled = false;

            SubjectText.text = "<b><size=60>" + Global.Subject.Name + "</size></b>\r\n";
            RefreshOptionInfo();

            CreateRatingHolder();

            // 监听 上一个选项按钮
            LastBtn.onClick.AddListener(() =>
            {
                CalculateScore(Global.Subject.Options[mOptionIndex]);

                if (mOptionIndex == 0) return;

                mOptionIndex--;
            });

            // 监听 下一个选项按钮
            NextBtn.onClick.AddListener(() =>
            {
                List<Option> options = Global.Subject.Options;

                CalculateScore(options[mOptionIndex]);

                if (mOptionIndex >= options.Count - 1)
                    mOptionIndex = 0;
                else
                    mOptionIndex++;

                RefreshOptionInfo();

                bool isAllLocked = true;
                foreach (Option option in options)
                {
                    if (option.IsLocked == false)
                    {
                        isAllLocked = false;
                        break;
                    }
                }

                if (isAllLocked)
                {
                    ComputeBtn.enabled = true;
                }
            });

            // 监听 开始计算按钮
            ComputeBtn.onClick.AddListener(() =>
            {
                CloseSelf();
                // 打开结果面板
                Debug.Log("开始计算");

            });
        }

        protected override void OnOpen(IUIData uiData = null)
        {
        }

        protected override void OnShow()
        {
        }

        private void RefreshOptionInfo()
        {
            OptionText.text = "<b><size=54>" + (mOptionIndex + 1) + ". " + 
                Global.Subject.Options[mOptionIndex].Name + "</b></size>\r\n" +
                "请拖动滑块 为其打分";
        }

        private void CreateRatingHolder()
        {
            for (int i = 0; i < Global.SampleSpace.SamplePoints.Count; i++)
            {
                SamplePoint point = Global.SampleSpace.SamplePoints[i];
                Option option = Global.Subject.Options[i];
                option.AddFocus(point.Name);

                RatingHolderTemplete.InstantiateWithParent(Content)
                    .SiblingIndex(Content.childCount - 3)
                    .Self(self =>
                    {
                        self.RatingLabel.text = point.Name;

                        self.RatingSlider.onValueChanged.AddListener(value =>
                        {
                            self.SliderText.text = value.ToString("F1");

                            option.GetFocusByName(self.RatingLabel.text).Score = value * point.Value;

                            // 解锁选项
                            option.IsLocked = false;
                            // 重置选项分数
                            option.Score = 0;
                        });

                    })
                    .Show();
            }
        }

        // 计算该选项的总分
        private void CalculateScore(Option option)
        {
            foreach (Focus focus in option.Focuses)
            {
                option.Score += focus.Score;
            }

            // 计算一次之后锁定，也是该选项计算完成的标志
            option.IsLocked = true;
        }

        protected override void OnHide()
        {
        }

        protected override void OnClose()
        {
        }
    }
}
