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
        // 存放生成的 Slider 对象
        private Dictionary<string, GameObject> mFocusHolderDict = new Dictionary<string, GameObject>();

        // 关注点的个数
        private int mFocusCount = 0;
        // 是否正在刷新
        private bool mIsRefreshing = false;

        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIFocusesPanelData ?? new UIFocusesPanelData();
            // please add init code here

            FocusHolderTemplete.Hide();

            // 给样本空间名称赋值
            Global.SampleSpace.Name = Global.SubjectName;
            // 展示之前输入的信息
            SetInfoText();
            // 创建一个关注点输入框
            CreateFocusHolder();

            AddFocusBtn.onClick.AddListener(() =>
            {
                CreateFocusHolder();
            });

            NextBtn.onClick.AddListener(() =>
            {
                CloseSelf();
                // 打开分析面板
            });
        }

        protected override void OnOpen(IUIData uiData = null)
        {
        }

        protected override void OnShow()
        {
        }

        private void SetInfoText()
        {
            string subjectNameStr = Global.SubjectName + "\r\n\r\n";
            string optionsListStr = "";

            for (int i = 0; i < Global.OptionList.Count; i++)
            {
                optionsListStr += (i + 1).ToString() + ". " + Global.OptionList[i] + "\r\n";
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
            mFocusCount++;
            int index = mFocusCount - 1;

            // 给样本空间添加一个关注点（样本点）
            Global.SampleSpace.AddSamplePoint("", 1f);

            FocusHolderTemplete.InstantiateWithParent(Content)
                .SiblingIndex(Content.childCount - 3)
                .Self(self =>
                {
                    // 获取输入框
                    TMP_InputField inputField = self.FocusInputField;
                    self.Label.text = "关注点 " + mFocusCount;
                    // 获取滑动条
                    Slider slider = self.FocusSlider;
                    slider.enabled = false;

                    inputField.onEndEdit.AddListener(focusName =>
                    {
                        Global.SampleSpace.SamplePoints[index].Name = focusName;

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
            mFocusCount = 0;
        }
    }
}
