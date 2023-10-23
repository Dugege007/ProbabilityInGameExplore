using UnityEngine;
using QFramework;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Drawing;
using QAssetBundle;
using Unity.VisualScripting;
using System.Collections.ObjectModel;

namespace ProbabilityTest
{
    public class UIRatingPanelData : UIPanelData
    {
    }
    public partial class UIRatingPanel : UIPanel
    {
        private int mOptionIndex = 0;
        private string mOptionName = "";

        private List<Option> mGlobalOptions = new List<Option>();

        private List<RatingHolderTemplete> mRatingHolders = new List<RatingHolderTemplete>();

        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIRatingPanelData ?? new UIRatingPanelData();
            // please add init code here

            mGlobalOptions = Global.Subject.Options;

            foreach (var option in mGlobalOptions)
            {
                Debug.Log("mGlobalOptions：" + option.Name);
                foreach (var focus in option.Focuses)
                {
                    Debug.Log("focus：" + focus.Name);
                }
            }

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
                // 检查选项是否全部解锁
                CheckAllOptionsUnLocked();
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
                // 检查选项是否全部解锁
                CheckAllOptionsUnLocked();
            });

            // 监听 开始计算按钮
            ComputeBtn.onClick.AddListener(() =>
            {
                AudioKit.PlaySound(Sfx.CLICK);

                // 计算 Score
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
            Option option = Global.Subject.Options[0];
            List<SamplePoint> samplePoints = Global.Subject.SampleSpace.SamplePoints;

            for (int i = 0; i < samplePoints.Count; i++)
            {
                RatingHolderTemplete.InstantiateWithParent(OptionRatingHolder)
                    .Self(self =>
                    {
                        mRatingHolders.Add(self);

                        self.RatingLabel.text = samplePoints[i].Name;
                        //Debug.Log(option.GetFocusByName(samplePoints[i].Name).Value);
                        self.RatingSlider.value = option.GetFocusByName(samplePoints[i].Name).Value;
                        self.SliderText.text = option.GetFocusByName(samplePoints[i].Name).Value.ToString("F1");

                        self.RatingSlider.onValueChanged.AddListener(value =>
                        {
                            // 解锁选项
                            option.IsUnLocked = true;
                            self.SliderText.text = value.ToString("F1");
                            CheckAllOptionsUnLocked();
                        });
                    })
                    .Show();
            }
        }

        private void CalculateEveryFocusScore(Option option)
        {
            // 解锁选项
            option.IsUnLocked = true;

            for (int i = 0; i < mRatingHolders.Count; i++)
            {
                option.Focuses[i].Value = mRatingHolders[i].RatingSlider.value;
                Debug.Log(option.Focuses[i].Name + " 得分：" + option.Focuses[i].Score);
            }
        }

        private void ResetSliderValue(Option option)
        {
            // 如果该选项还未解锁
            if (mGlobalOptions[mOptionIndex].IsUnLocked == false)
            {
                // 重置滑动条和分数
                for (int i = 0; i < mRatingHolders.Count; i++)
                {
                    mRatingHolders[i].RatingSlider.value = 1f;
                    option.Focuses[i].Value = 1f;
                }
            }
            else
            {
                // 为滑动条赋值
                for (int i = 0; i < mRatingHolders.Count; i++)
                {
                    mRatingHolders[i].RatingSlider.value = option.Focuses[i].Value;
                }
            }

            foreach (var ratingHolder in mRatingHolders)
                ratingHolder.Bubble.Hide();
        }

        private void CheckAllOptionsUnLocked()
        {
            bool isAllUnLocked = true;
            foreach (Option option in mGlobalOptions)
            {
                if (!option.IsUnLocked)
                {
                    isAllUnLocked = false;
                    break;
                }
            }

            if (isAllUnLocked)
                ComputeBtn.Show();
        }

        protected override void OnHide()
        {
        }

        protected override void OnClose()
        {
        }
    }
}
