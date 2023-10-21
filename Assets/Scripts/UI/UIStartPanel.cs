using UnityEngine;
using UnityEngine.UI;
using QFramework;
using TMPro;
using System.Collections.Generic;
using QAssetBundle;
using System.Linq;
using static UnityEngine.Rendering.DebugUI;

namespace ProbabilityTest
{
    public class UIStartPanelData : UIPanelData
    {
    }
    public partial class UIStartPanel : UIPanel
    {
        private int mOptionIndex = 0;
        private List<TMP_InputField> mOptionInputFields = new List<TMP_InputField>();
        private List<TMP_Text> mTMPOptionLabels = new List<TMP_Text>();

        private Color mOriginalOptionBGColor = new Color(25f / 255, 103f / 255, 116f / 255);
        private List<TMP_InputField> mSameTextInputFields = new List<TMP_InputField>();

        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIStartPanelData ?? new UIStartPanelData();
            // please add init code here

            // ���� ѡ�������ģ��
            OptionInputFieldHolderTemplete.Hide();
            NotificationSubjectNull.Hide();
            NotificationOptionCount.Hide();
            NotificationOptionNull.Hide();
            NotificationOptionSame.Hide();

            SubjectInputField.onEndEdit.AddListener(subjectName =>
            {

            });

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

                if (SubjectInputField.text.IsNullOrEmpty())
                {
                    NotificationSubjectNull.ShowNotification();
                    return;
                }

                if (mOptionInputFields.Count < 1)
                {
                    NotificationOptionCount.ShowNotification();
                    return;
                }

                bool isAllInput = true;
                foreach (var field in mOptionInputFields)
                {
                    if (field.text.IsNullOrEmpty())
                    {
                        isAllInput = false;
                        field.transform.Find("Background").GetComponent<Image>().color = Color.red;
                    }
                }

                if (isAllInput == false)
                {
                    NotificationOptionNull.ShowNotification();
                    return;
                }

                for (int i = 0; i < mOptionInputFields.Count - 1; i++)
                {
                    for (int j = i + 1; j < mOptionInputFields.Count; j++)
                    {
                        if (mOptionInputFields[i].text == mOptionInputFields[j].text)
                        {
                            NotificationOptionSame.ShowNotification();
                            mOptionInputFields[i].transform.Find("Background").GetComponent<Image>().color = Color.red;
                            mOptionInputFields[j].transform.Find("Background").GetComponent<Image>().color = Color.red;
                            mSameTextInputFields.Add(mOptionInputFields[i]);
                            mSameTextInputFields.Add(mOptionInputFields[j]);
                            return;
                        }
                    }
                }

                // ������������ռ�����ֵ
                Global.SampleSpace.Name = SubjectInputField.text;
                Global.Subject.Name = SubjectInputField.text;

                // ��������е�ѡ�������
                Global.Subject.Options.Clear();
                // �� Subject ����� Option
                foreach (var inputField in mOptionInputFields)
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

            OptionInputFieldHolderTemplete.InstantiateWithParent(Content)
                .SiblingIndex(Content.childCount - 3)
                .Self(self =>
                {
                    // ���������б�
                    mOptionInputFields.Add(self.OptionInputField);
                    self.OptionInputField.onEndEdit.AddListener(optionName =>
                    {
                        self.OptionInputField.transform.Find("Background").GetComponent<Image>().color = mOriginalOptionBGColor;

                        foreach (var inputField in mSameTextInputFields.Where(f => f != null))
                        {
                            inputField.transform.Find("Background").GetComponent<Image>().color = mOriginalOptionBGColor;
                        }
                    });

                    self.Label.text = "ѡ�� " + mOptionIndex;
                    mTMPOptionLabels.Add(self.Label);

                    self.RemoveBtn.onClick.AddListener(() =>
                    {
                        AudioKit.PlaySound(Sfx.CLICK);

                        mOptionInputFields.Remove(self.OptionInputField);
                        mTMPOptionLabels.Remove(self.Label);
                        mOptionIndex--;

                        // �������
                        for (int i = 0; i < mTMPOptionLabels.Count; i++)
                            mTMPOptionLabels[i].text = "ѡ�� " + (i + 1);

                        self.DestroyGameObjGracefully();
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
