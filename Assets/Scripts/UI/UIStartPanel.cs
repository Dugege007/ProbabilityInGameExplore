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
        private int OptionCount = 0;

        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIStartPanelData ?? new UIStartPanelData();
            // please add init code here

            OptionInputFieldTemplete.Hide();

            AddOptionBtn.onClick.AddListener(() =>
            {
                CreateOptionInputField();
            });

            NextBtn.onClick.AddListener(() =>
            {
                CloseSelf();
                UIKit.OpenPanel<UIFocusesPanel>();
            });
        }

        protected override void OnOpen(IUIData uiData = null)
        {
        }

        protected override void OnShow()
        {
        }

        private void CreateOptionInputField()
        {
            OptionCount++;

            OptionInputFieldTemplete.InstantiateWithParent(Content)
                .SiblingIndex(Content.childCount - 3)
                .Self(self =>
                {
                    TMP_Text label = self.transform.Find("TextArea").Find("Label").GetComponent<TMP_Text>();
                    label.text = "СЎПо " + OptionCount;
                })
                .Show();
        }

        protected override void OnHide()
        {
        }

        protected override void OnClose()
        {
            OptionCount = 0;
        }
    }
}
