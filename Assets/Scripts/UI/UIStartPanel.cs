using UnityEngine;
using UnityEngine.UI;
using QFramework;
using TMPro;
using System.Collections.Generic;
using QAssetBundle;

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

            // ���� ѡ�������ģ��
            OptionInputFieldTemplete.Hide();

            // ���� ���ѡ�ť
            AddOptionBtn.onClick.AddListener(() =>
            {
                AudioKit.PlaySound(Sfx.CLICK);
                CreateOptionInputField();
            });

            // ���� ��һ����ť
            NextBtn.onClick.AddListener(() =>
            {
                AudioKit.PlaySound(Sfx.CLICK);

                // ������������ռ�����ֵ
                Global.SampleSpace.Name = SubjectInputField.text;
                Global.Subject.Name = SubjectInputField.text;

                // ��������е�ѡ�������
                Global.Subject.Options.Clear();

                foreach (var inputField in mTMPInputFields)
                {
                    Global.Subject.AddOption(inputField.text);
                }

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

            OptionInputFieldTemplete.InstantiateWithParent(Content)
                .SiblingIndex(Content.childCount - 3)
                .Self(self =>
                {
                    // ���������б�
                    mTMPInputFields.Add(self);

                    TMP_Text label = self.transform.Find("TextArea").Find("Label").GetComponent<TMP_Text>();
                    label.text = "ѡ�� " + mOptionIndex;
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
