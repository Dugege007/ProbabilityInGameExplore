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

            // չʾ֮ǰ�������Ϣ
            SetInfoText();
            // ����һ����ע�������
            CreateFocusInputField();

            AddFocusBtn.onClick.AddListener(() =>
            {
                CreateFocusInputField();
            });

            NextBtn.onClick.AddListener(() =>
            {
                CloseSelf();
                // �򿪷������
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

            InfoText.text = "<b><size=42>���⣺</size></b>\r\n" +
                subjectNameStr +
                "<b><size=42>ѡ�</size></b>\r\n" +
                optionsListStr +
                " ";
        }

        private void CreateFocusInputField()
        {
            mFocusCount++;
            Global.FocusList.Add("");
            int index = mFocusCount - 1;

            FocusInputFieldTemplete.InstantiateWithParent(Content)
                .SiblingIndex(Content.childCount - 3)
                .Self(self =>
                {
                    TMP_Text label = self.transform.Find("TextArea").Find("Label").GetComponent<TMP_Text>();
                    label.text = "��ע�� " + mFocusCount;

                    self.onEndEdit.AddListener(focus =>
                    {
                        Global.FocusList[index] = focus;
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
