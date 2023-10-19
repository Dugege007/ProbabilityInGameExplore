using UnityEngine;
using UnityEngine.UI;
using QFramework;
using TMPro;
using System.Collections.Generic;

namespace ProbabilityTest
{
    public class UIStartPanelData : UIPanelData
    {
    }
    public partial class UIStartPanel : UIPanel
    {
        private int mOptionIndex = 0;
        private List<TMP_InputField> mTMPInputFields = new List<TMP_InputField>();

        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIStartPanelData ?? new UIStartPanelData();
            // please add init code here

            // 隐藏 选项输入框模板
            OptionInputFieldTemplete.Hide();

            // 监听 添加选项按钮
            AddOptionBtn.onClick.AddListener(() =>
            {
                CreateOptionInputField();
            });

            // 监听 下一步按钮
            NextBtn.onClick.AddListener(() =>
            {
                // 给主题和样本空间名赋值
                Global.SampleSpace.Name = SubjectInputField.text;
                Global.Subject.Name = SubjectInputField.text;

                // 清空主题中的选项的数据
                Global.Subject.Options.Clear();

                foreach (var inputField in mTMPInputFields)
                {
                    Global.Subject.AddOption(inputField.text);
                }

                CloseSelf();
                // 打开关注点面板
                UIKit.OpenPanel<UIFocusesPanel>();
            });
        }

        protected override void OnOpen(IUIData uiData = null)
        {
        }

        protected override void OnShow()
        {
        }

        // 创建 选项输入框
        private void CreateOptionInputField()
        {
            mOptionIndex++;

            OptionInputFieldTemplete.InstantiateWithParent(Content)
                .SiblingIndex(Content.childCount - 3)
                .Self(self =>
                {
                    // 添加输入框到列表
                    mTMPInputFields.Add(self);

                    TMP_Text label = self.transform.Find("TextArea").Find("Label").GetComponent<TMP_Text>();
                    label.text = "选项 " + mOptionIndex;
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
