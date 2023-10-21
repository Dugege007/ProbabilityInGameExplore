using UnityEngine;
using UnityEngine.UI;
using QFramework;
using QAssetBundle;

namespace ProbabilityTest
{
    public class UIHomePanelData : UIPanelData
    {
    }
    public partial class UIHomePanel : UIPanel
    {
        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIHomePanelData ?? new UIHomePanelData();
            // please add init code here

            NewSubjectBtn.onClick.AddListener(() =>
            {
                AudioKit.PlaySound(Sfx.CLICK);

                Global.Subject = null;
                Global.SampleSpace = null;

                CloseSelf();
                UIKit.OpenPanel<UIStartPanel>();
            });

            ViewHistoryBtn.onClick.AddListener(() =>
            {
                AudioKit.PlaySound(Sfx.CLICK);

                CloseSelf();
                UIKit.OpenPanel<UIHistoryPanel>();
            });

            ClearHistoryBtn.onClick.AddListener(() =>
            {
                AudioKit.PlaySound(Sfx.CLICK);

                PlayerPrefs.DeleteAll();

                CloseSelf();
                UIKit.OpenPanel<UIStartPanel>();
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
