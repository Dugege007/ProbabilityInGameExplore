using UnityEngine;
using QFramework;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Drawing;
using QAssetBundle;

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
                AudioKit.PlaySound(Sfx.CLICK);

                int nextIndex = mOptionIndex - 1;
                if (nextIndex < 0)
                    nextIndex = mGlobalOptions.Count - 1;

                // 计算当前 Score
                CalculateEveryFocusScore(mGlobalOptions[mOptionIndex]);
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
                AudioKit.PlaySound(Sfx.CLICK);

                int nextIndex = mOptionIndex + 1;
                if (nextIndex >= mGlobalOptions.Count)
                    nextIndex = 0;

                // 计算当前 Score
                CalculateEveryFocusScore(mGlobalOptions[mOptionIndex]);
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
                AudioKit.PlaySound(Sfx.CLICK);

                // 计算当前 Score
                CalculateEveryFocusScore(mGlobalOptions[mOptionIndex]);

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

        private void CalculateEveryFocusScore(Option option)
        {
            // 解锁选项
            option.IsUnLocked = true;

            for (int i = 0; i < mRatingSliders.Count; i++)
            {
                option.Focuses[i].Value = mRatingSliders[i].value;
                Debug.Log(option.Focuses[i].Name + " 得分：" + option.Focuses[i].Score);
            }
        }

        private void ResetSliderValue(Option option)
        {
            // 如果该选项还未解锁
            if (mGlobalOptions[mOptionIndex].IsUnLocked == false)
            {
                // 重置滑动条和分数
                for (int i = 0; i < mRatingSliders.Count; i++)
                {
                    mRatingSliders[i].value = 1f;
                    option.Focuses[i].Value = 1f;
                }
            }
            else
            {
                // 为滑动条赋值
                for (int i = 0; i < mRatingSliders.Count; i++)
                {
                    mRatingSliders[i].value = option.Focuses[i].Value;
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
