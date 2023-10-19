using UnityEngine;
using UnityEngine.UI;
using QFramework;
using TMPro;
using System.Collections.Generic;

namespace ProbabilityTest
{
    public class UIFocusesPanelData : UIPanelData
    {
    }
    public partial class UIFocusesPanel : UIPanel
    {
        private int mFocusIndex = 0;
        private List<TMP_InputField> mFocusInputFields = new List<TMP_InputField>();
        private List<Slider> mFocusSliders = new List<Slider>();

        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIFocusesPanelData ?? new UIFocusesPanelData();
            // please add init code here

            // ���� ��ע��ģ��
            FocusHolderTemplete.Hide();

            // չʾ֮ǰ�������Ϣ
            SetInfoText();
            // ����һ����ע�������
            CreateFocusHolder();

            // ���� ��ӹ�ע�㰴ť
            AddFocusBtn.onClick.AddListener(() =>
            {
                CreateFocusHolder();
            });

            // ���� ��һ����ť
            NextBtn.onClick.AddListener(() =>
            {
                // ��������ռ��е�������
                Global.SampleSpace.SamplePoints.Clear();

                // �����û��������������
                for (int i = 0; i < mFocusInputFields.Count; i++)
                {
                    Global.SampleSpace.AddSamplePoint(mFocusInputFields[i].text, mFocusSliders[i].value);
                    Debug.Log("����ӣ�" + Global.SampleSpace.SamplePoints[i].Name + ": " + Global.SampleSpace.SamplePoints[i].Value);
                }

                for (int i = 0;i < Global.Subject.Options.Count; i++)
                {
                    for (int j = 0; j < mFocusInputFields.Count; j++)
                    {
                        Global.Subject.Options[i].AddFocus(mFocusInputFields[j].text);
                    }
                }

                CloseSelf();
                // ���������
                UIKit.OpenPanel<UIRatingPanel>();
            });
        }

        protected override void OnOpen(IUIData uiData = null)
        {
        }

        protected override void OnShow()
        {
        }

        // ������Ϣ
        private void SetInfoText()
        {
            string subjectNameStr = Global.Subject.Name + "\r\n\r\n";
            string optionsListStr = "";

            for (int i = 0; i < Global.Subject.Options.Count; i++)
            {
                optionsListStr += (i + 1).ToString() + ". " + Global.Subject.Options[i].Name + "\r\n";
            }

            InfoText.text = "<b><size=42>���⣺</size></b>\r\n" +
                subjectNameStr +
                "<b><size=42>ѡ�</size></b>\r\n" +
                optionsListStr +
                " ";
        }

        // ������ע����Ŀ
        private void CreateFocusHolder()
        {
            mFocusIndex++;

            FocusHolderTemplete.InstantiateWithParent(Content)
                .SiblingIndex(Content.childCount - 3)
                .Self(self =>
                {
                    // ��ȡ�����
                    TMP_InputField inputField = self.FocusInputField;
                    // ���������б�
                    mFocusInputFields.Add(inputField);

                    self.Label.text = "��ע�� " + mFocusIndex;

                    // ��ȡ������
                    Slider slider = self.FocusSlider;
                    // ��ӻ��������б�
                    mFocusSliders.Add(slider);
                    // ���û�����
                    slider.enabled = false;

                    // �������Ϊ��ʱ������������
                    inputField.onEndEdit.AddListener(focusName =>
                    {
                        if (focusName == "")
                            slider.enabled = false;
                        else
                            slider.enabled = true;
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
