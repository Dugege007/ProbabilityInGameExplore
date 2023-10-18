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

            // ���� ѡ�������ģ��
            OptionInputFieldTemplete.Hide();

            // ���� ���������
            SubjectInputField.onEndEdit.AddListener(subject =>
            {
                Global.Subject.Name = subject;
            });

            // ���� ���ѡ�ť
            AddOptionBtn.onClick.AddListener(() =>
            {
                CreateOptionInputField();
            });

            // ���� ��һ����ť
            NextBtn.onClick.AddListener(() =>
            {
                CloseSelf();
                // �򿪹�ע�����
                UIKit.OpenPanel<UIFocusesPanel>();
            });
        }

        protected override void OnOpen(IUIData uiData = null)
        {
        }

        protected override void OnShow()
        {
        }

        // ���� ѡ�������
        private void CreateOptionInputField()
        {
            mOptionIndex++;
            Global.Subject.AddOption("");

            OptionInputFieldTemplete.InstantiateWithParent(Content)
                .SiblingIndex(Content.childCount - 3)
                .Self(self =>
                {
                    TMP_Text label = self.transform.Find("TextArea").Find("Label").GetComponent<TMP_Text>();

                    label.text = "ѡ�� " + mOptionIndex;

                    // ���� ѡ�������
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
