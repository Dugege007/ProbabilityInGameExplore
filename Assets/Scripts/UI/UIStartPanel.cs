using UnityEngine;
using UnityEngine.UI;
using QFramework;
using TMPro;

namespace ProbabilityTest
{
    public class UIStartPanelData : UIPanelData
    {
    }
    public partial class UIStartPanel : UIPanel
    {
        private int mOptionIndex = 0;

        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIStartPanelData ?? new UIStartPanelData();
            // please add init code here

            // 隐藏 选项输入框模板
            OptionInputFieldTemplete.Hide();

            // 监听 主题输入框
            SubjectInputField.onEndEdit.AddListener(subject =>
            {
                Global.Subject.Name = subject;
            });

            // 监听 添加选项按钮
            AddOptionBtn.onClick.AddListener(() =>
            {
                CreateOptionInputField();
            });

            // 监听 下一步按钮
            NextBtn.onClick.AddListener(() =>
            {
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
            Global.Subject.AddOption("");

            OptionInputFieldTemplete.InstantiateWithParent(Content)
                .SiblingIndex(Content.childCount - 3)
                .Self(self =>
                {
                    TMP_Text label = self.transform.Find("TextArea").Find("Label").GetComponent<TMP_Text>();

                    label.text = "选项 " + mOptionIndex;

                    // 监听 选项输入框
                    self.onEndEdit.AddListener(optionName =>
                    {
                        Global.Subject.Options[mOptionIndex - 1].Name = optionName;
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
