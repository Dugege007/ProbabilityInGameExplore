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

            NotificationNewSubject.Hide();

            Global.IsTemporarilySave.RegisterWithInitValue(isTempSave =>
            {
                if (isTempSave)
                    ContinueEditBtn.Show();
                else
                    ContinueEditBtn.Hide();

            }).UnRegisterWhenGameObjectDestroyed(this);

            // 新建 主题
            NewSubjectBtn.onClick.AddListener(() =>
            {
                AudioKit.PlaySound(Sfx.CLICK);

                if (Global.IsTemporarilySave.Value == true)
                {
                    NotificationNewSubject.Show();
                }
                else
                {
                    Global.ResetData();

                    CloseSelf();
                    Global.IsTemporarilySave.Value = false;
                    UIKit.OpenPanel<UIStartPanel>();
                }
            });

            ViewHistoryBtn.onClick.AddListener(() =>
            {
                AudioKit.PlaySound(Sfx.CLICK);

                CloseSelf();
                UIKit.OpenPanel<UIHistoryPanel>();
            });

            ContinueEditBtn.onClick.AddListener(() =>
            {
                AudioKit.PlaySound(Sfx.CLICK);

                CloseSelf();
                Global.IsTemporarilySave.Value = false;
                UIKit.OpenPanel<UIStartPanel>();
            });

            NotificationNewSubject.CancelBtn.onClick.AddListener(() =>
            {
                NotificationNewSubject.Hide();
            });

            NotificationNewSubject.YesBtn.onClick.AddListener(() =>
            {
                Global.ResetData();

                CloseSelf();
                Global.IsTemporarilySave.Value = false;
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
