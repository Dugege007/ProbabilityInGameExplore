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

        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIFocusesPanelData ?? new UIFocusesPanelData();
            // please add init code here

            // ���� ��ע��ģ��
            FocusHolderTemplete.Hide();

            // �������ռ����Ƹ�ֵ
            Global.SampleSpace.Name = Global.Subject.Name;
            Global.SampleSpace.SamplePoints.Clear();

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
            // �������ռ����һ����ע�㣨�����㣩
            Global.SampleSpace.AddSamplePoint("", 1f);

            FocusHolderTemplete.InstantiateWithParent(Content)
                .SiblingIndex(Content.childCount - 3)
                .Self(self =>
                {
                    // ��ȡ�����
                    TMP_InputField inputField = self.FocusInputField;
                    self.Label.text = "��ע�� " + mFocusIndex;
                    // ��ȡ������
                    Slider slider = self.FocusSlider;
                    slider.enabled = false;

                    inputField.onEndEdit.AddListener(focusName =>
                    {
                        Global.SampleSpace.SamplePoints[mFocusIndex - 1].Name = focusName;

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
