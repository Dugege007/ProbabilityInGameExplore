using UnityEngine;
using UnityEngine.UI;
using QFramework;
using TMPro;
using System.Collections.Generic;

namespace ProbabilityTest
{
    public class UIFocusesPanelData : UIPanelData
    {
    }
    public partial class UIFocusesPanel : UIPanel
    {
        private int mFocusIndex = 0;

        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIFocusesPanelData ?? new UIFocusesPanelData();
            // please add init code here

            // 隐藏 关注点模板
            FocusHolderTemplete.Hide();

            // 给样本空间名称赋值
            Global.SampleSpace.Name = Global.Subject.Name;
            Global.SampleSpace.SamplePoints.Clear();

            // 展示之前输入的信息
            SetInfoText();
            // 创建一个关注点输入框
            CreateFocusHolder();

            // 监听 添加关注点按钮
            AddFocusBtn.onClick.AddListener(() =>
            {
                CreateFocusHolder();
            });

            // 监听 下一步按钮
            NextBtn.onClick.AddListener(() =>
            {
                CloseSelf();
                // 打开评分面板
                UIKit.OpenPanel<UIRatingPanel>();
            });
        }

        protected override void OnOpen(IUIData uiData = null)
        {
        }

        protected override void OnShow()
        {
        }

        // 设置信息
        private void SetInfoText()
        {
            string subjectNameStr = Global.Subject.Name + "\r\n\r\n";
            string optionsListStr = "";

            for (int i = 0; i < Global.Subject.Options.Count; i++)
            {
                optionsListStr += (i + 1).ToString() + ". " + Global.Subject.Options[i].Name + "\r\n";
            }

            InfoText.text = "<b><size=42>主题：</size></b>\r\n" +
                subjectNameStr +
                "<b><size=42>选项：</size></b>\r\n" +
                optionsListStr +
                " ";
        }

        // 创建关注点条目
        private void CreateFocusHolder()
        {
            mFocusIndex++;
            // 给样本空间添加一个关注点（样本点）
            Global.SampleSpace.AddSamplePoint("", 1f);

            FocusHolderTemplete.InstantiateWithParent(Content)
                .SiblingIndex(Content.childCount - 3)
                .Self(self =>
                {
                    // 获取输入框
                    TMP_InputField inputField = self.FocusInputField;
                    self.Label.text = "关注点 " + mFocusIndex;
                    // 获取滑动条
                    Slider slider = self.FocusSlider;
                    slider.enabled = false;

                    inputField.onEndEdit.AddListener(focusName =>
                    {
                        Global.SampleSpace.SamplePoints[mFocusIndex - 1].Name = focusName;

                        if (focusName == "")
                            slider.enabled = false;
                        else
                            slider.enabled = true;
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
    }
}
