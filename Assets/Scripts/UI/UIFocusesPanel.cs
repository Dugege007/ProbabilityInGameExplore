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
        // ������ɵ� Slider ����
        private Dictionary<string, GameObject> mFocusHolderDict = new Dictionary<string, GameObject>();

        // ��ע��ĸ���
        private int mFocusCount = 0;
        // �Ƿ�����ˢ��
        private bool mIsRefreshing = false;

        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIFocusesPanelData ?? new UIFocusesPanelData();
            // please add init code here

            FocusHolderTemplete.Hide();

            // �������ռ����Ƹ�ֵ
            Global.SampleSpace.Name = Global.SubjectName;
            // չʾ֮ǰ�������Ϣ
            SetInfoText();
            // ����һ����ע�������
            CreateFocusHolder();

            AddFocusBtn.onClick.AddListener(() =>
            {
                CreateFocusHolder();
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

        // ������ע����Ŀ
        private void CreateFocusHolder()
        {
            mFocusCount++;
            int index = mFocusCount - 1;

            // �������ռ����һ����ע�㣨�����㣩
            Global.SampleSpace.AddSamplePoint("", 1f);

            FocusHolderTemplete.InstantiateWithParent(Content)
                .SiblingIndex(Content.childCount - 3)
                .Self(self =>
                {
                    // ��ȡ�����
                    TMP_InputField inputField = self.FocusInputField;
                    self.Label.text = "��ע�� " + mFocusCount;
                    // ��ȡ������
                    Slider slider = self.FocusSlider;
                    slider.enabled = false;

                    inputField.onEndEdit.AddListener(focusName =>
                    {
                        Global.SampleSpace.SamplePoints[index].Name = focusName;

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
            mFocusCount = 0;
        }
    }
}
