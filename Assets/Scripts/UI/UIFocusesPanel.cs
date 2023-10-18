using UnityEngine;
using UnityEngine.UI;
using QFramework;
using TMPro;

namespace ProbabilityTest
{
    public class UIFocusesPanelData : UIPanelData
    {
    }
    public partial class UIFocusesPanel : UIPanel
    {
        private int mFocusCount = 0;

        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIFocusesPanelData ?? new UIFocusesPanelData();
            // please add init code here

            FocusInputFieldTemplete.Hide();
            // 先创建一个关注点输入框
            CreateFocusInputField();

            AddFocusBtn.onClick.AddListener(() =>
            {
                CreateFocusInputField();
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

        private void CreateFocusInputField()
        {
            mFocusCount++;

            FocusInputFieldTemplete.InstantiateWithParent(Content)
                .SiblingIndex(Content.childCount - 3)
                .Self(self =>
                {
                    TMP_Text label = self.transform.Find("TextArea").Find("Label").GetComponent<TMP_Text>();
                    label.text = "关注点 " + mFocusCount;
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
