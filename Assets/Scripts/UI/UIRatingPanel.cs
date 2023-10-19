using UnityEngine;
using QFramework;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Drawing;

namespace ProbabilityTest
{
    public class UIRatingPanelData : UIPanelData
    {
    }
    public partial class UIRatingPanel : UIPanel
    {
        private int mOptionIndex = 0;
        private string mOptionName = "";

        private List<Option> mGlobalOptions = Global.Subject.Options;
        private List<Slider> mRatingSliders = new List<Slider>();

        private List<RatingHolderTemplete> mRatingHolderTempletes = new List<RatingHolderTemplete>();

        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIRatingPanelData ?? new UIRatingPanelData();
            // please add init code here

            RatingHolderTemplete.Hide();
            ComputeBtn.Hide();

            SubjectText.text = "<b><size=60>" + Global.Subject.Name + "</size></b>\r\n";
            RefreshOptionInfo();
            CreateRatingHolder();

            // 监听 上一个选项按钮
            LastBtn.onClick.AddListener(() =>
            {
                int nextIndex = mOptionIndex - 1;
                if (nextIndex < 0)
                    nextIndex = mGlobalOptions.Count - 1;

                // 计算当前 Score
                CalculateEveryFocusScore(mGlobalOptions[mOptionIndex]);
                CalculateOptionScore(mGlobalOptions[mOptionIndex]);
                // 当前序号 - 1
                mOptionIndex = nextIndex;

                // 更新选项
                RefreshOptionInfo();
                // 重置滑动条
                ResetSliderValue(mGlobalOptions[mOptionIndex]);

                bool isAllUnLocked = true;
                foreach (Option option in mGlobalOptions)
                {
                    if (option.IsUnLocked == false)
                    {
                        isAllUnLocked = false;
                        break;
                    }
                }

                if (isAllUnLocked)
                    ComputeBtn.Show();
            });

            // 监听 下一个选项按钮
            NextBtn.onClick.AddListener(() =>
            {
                int nextIndex = mOptionIndex + 1;
                if (nextIndex >= mGlobalOptions.Count)
                    nextIndex = 0;

                // 计算当前 Score
                CalculateEveryFocusScore(mGlobalOptions[mOptionIndex]);
                CalculateOptionScore(mGlobalOptions[mOptionIndex]);
                // 当前序号 + 1
                mOptionIndex = nextIndex;

                // 更新选项
                RefreshOptionInfo();
                // 重置滑动条
                ResetSliderValue(mGlobalOptions[mOptionIndex]);

                bool isAllUnLocked = true;
                foreach (Option option in mGlobalOptions)
                {
                    if (option.IsUnLocked == false)
                    {
                        isAllUnLocked = false;
                        break;
                    }
                }

                if (isAllUnLocked)
                    ComputeBtn.Show();
            });

            // 监听 开始计算按钮
            ComputeBtn.onClick.AddListener(() =>
            {
                // 计算当前 Score
                CalculateEveryFocusScore(mGlobalOptions[mOptionIndex]);
                CalculateOptionScore(mGlobalOptions[mOptionIndex]);

                CloseSelf();
                // 打开结果面板
                UIKit.OpenPanel<UIResultPanel>();
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
            mOptionName = Global.Subject.Options[mOptionIndex].Name;
            OptionRatingHolder.OptionText.text = "<b><size=84>" + (mOptionIndex + 1) + ". " +
                 mOptionName + "</b></size>\r\n" +
                "请拖动滑块 为其打分";
        }

        private void CreateRatingHolder()
        {
            Option option = Global.Subject.GetOptionByName(mOptionName);

            for (int i = 0; i < Global.SampleSpace.SamplePoints.Count; i++)
            {
                SamplePoint point = Global.SampleSpace.SamplePoints[i];

                RatingHolderTemplete.InstantiateWithParent(OptionRatingHolder)
                    .Self(self =>
                    {
                        mRatingSliders.Add(self.RatingSlider);
                        mRatingHolderTempletes.Add(self);

                        self.RatingLabel.text = point.Name;

                        self.RatingSlider.onValueChanged.AddListener(value =>
                        {
                            // 解锁选项
                            option.IsUnLocked = true;

                            self.SliderText.text = value.ToString("F1");
                        });
                    })
                    .Show();
            }
        }

        // 计算该选项的总分
        private void CalculateOptionScore(Option option)
        {
            // 解锁选项
            option.IsUnLocked = true;

            // 重置选项分数
            option.Score = 0;
            foreach (Focus focus in option.Focuses)
            {
                option.Score += focus.Score;
            }
            Debug.Log("选项 " + option.Name + " 得分：" + option.Score);
        }

        private void CalculateEveryFocusScore(Option option)
        {
            for (int i = 0; i < mRatingSliders.Count; i++)
            {
                option.Focuses[i].Score = Global.SampleSpace.SamplePoints[i].Value * mRatingSliders[i].value;
                Debug.Log(option.Focuses[i].Name + " 得分：" + option.Focuses[i].Score);
            }
        }

        private void ResetSliderValue(Option option)
        {
            // 如果该选项还未解锁
            if (mGlobalOptions[mOptionIndex].IsUnLocked == false)
            {
                // 清空滑动条和分数
                for (int i = 0; i < mRatingSliders.Count; i++)
                {
                    mRatingSliders[i].value = 0;
                    option.Focuses[i].Score = 0;
                }
            }
            else
            {
                // 为滑动条赋值
                for (int i = 0; i < mRatingSliders.Count; i++)
                {
                    mRatingSliders[i].value = option.Focuses[i].Score / Global.SampleSpace.SamplePoints[i].Value;
                }
            }

            foreach (var ratingHolder in mRatingHolderTempletes)
                ratingHolder.Bubble.Hide();
        }

        protected override void OnHide()
        {
        }

        protected override void OnClose()
        {
        }
    }
}
