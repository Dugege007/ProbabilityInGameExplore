using UnityEngine;
using UnityEngine.UI;
using QFramework;
using QAssetBundle;

namespace ProbabilityTest
{
    public class UIDefaultPanelData : UIPanelData
    {
    }
    public partial class UIDefaultPanel : UIPanel
    {
        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIDefaultPanelData ?? new UIDefaultPanelData();
            // please add init code here

            BackBtn.onClick.AddListener(() =>
            {
                AudioKit.PlaySound(Sfx.CLICK);

                if (UIKit.GetPanel<UIHistoryPanel>())
                {
                    UIKit.ClosePanel<UIHistoryPanel>();
                    UIKit.OpenPanel<UIHomePanel>();
                }
                else
                    Global.IsTemporarilySave.Value = true;
            });

            SettingsBtn.onClick.AddListener(() =>
            {
                AudioKit.PlaySound(Sfx.CLICK);

            });
        }

        protected override void OnOpen(IUIData uiData = null)
        {
        }

        protected override void OnShow()
        {
        }

        protected override void OnHide()
        {
        }

        protected override void OnClose()
        {
        }
    }
}
