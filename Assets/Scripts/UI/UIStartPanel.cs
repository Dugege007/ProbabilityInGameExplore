using UnityEngine;
using UnityEngine.UI;
using QFramework;
using TMPro;
using System.Collections.Generic;
using QAssetBundle;
using System.Linq;

namespace ProbabilityTest
{
    public class UIStartPanelData : UIPanelData
    {
    }
    public partial class UIStartPanel : UIPanel
    {
        private int mOptionIndex = 0;
        private List<TMP_InputField> mOptionInputFields = new List<TMP_InputField>();
        private List<TMP_Text> mOptionLabels = new List<TMP_Text>();

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

            if (Global.Subject != null)
            {
                SubjectInputField.text = Global.Subject.Name;
                foreach (Option option in Global.Subject.Options)
                {
                    CreateOptionInputField(option.Name);
                }
            }

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

                if (ValidateInputs() == false)
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
                            SetInputFieldBGColor(mOptionInputFields[i], Color.red);
                            SetInputFieldBGColor(mOptionInputFields[j], Color.red);
                            mSameTextInputFields.Add(mOptionInputFields[i]);
                            mSameTextInputFields.Add(mOptionInputFields[j]);
                            return;
                        }
                    }
                }

                if (Global.Subject == null)
                    Global.Subject = new Subject();

                // ������������ռ�����ֵ
                Global.Subject.Name = SubjectInputField.text;

                if (Global.Subject.IsHistory)
                {
                    Subject subjectCache = new Subject(SubjectInputField.text);

                    foreach (var inputField in mOptionInputFields)
                    {
                        if (Global.Subject.GetOptionByName(inputField.text) != null)
                        {
                            subjectCache.Options.Add(Global.Subject.GetOptionByName(inputField.text));
                        }
                        else
                        {
                            subjectCache.AddOption(inputField.text);
                        }
                    }
                    Global.Subject = subjectCache;
                }
                else
                {
                    // ��������е�ѡ�������
                    Global.Subject.Options.Clear();
                    // �� Subject ����� Option
                    foreach (var inputField in mOptionInputFields)
                    {
                        Global.Subject.AddOption(inputField.text);
                    }
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
        private void CreateOptionInputField(string optionText = "")
        {
            mOptionIndex++;

            OptionInputFieldHolderTemplete.InstantiateWithParent(Content)
                .SiblingIndex(Content.childCount - 3)
                .Self(self =>
                {
                    self.OptionInputField.text = optionText;
                    // ���������б�
                    mOptionInputFields.Add(self.OptionInputField);
                    self.OptionInputField.onEndEdit.AddListener(optionName =>
                    {
                        SetInputFieldBGColor(self.OptionInputField, mOriginalOptionBGColor);

                        foreach (var inputField in mSameTextInputFields.Where(f => f != null))
                            SetInputFieldBGColor(inputField, mOriginalOptionBGColor);
                    });

                    self.Label.text = "ѡ�� " + mOptionIndex;
                    mOptionLabels.Add(self.Label);

                    self.RemoveBtn.onClick.AddListener(() =>
                    {
                        AudioKit.PlaySound(Sfx.CLICK);

                        mOptionInputFields.Remove(self.OptionInputField);
                        mOptionLabels.Remove(self.Label);
                        mOptionIndex--;

                        // �������
                        for (int i = 0; i < mOptionLabels.Count; i++)
                            mOptionLabels[i].text = "ѡ�� " + (i + 1);

                        self.DestroyGameObjGracefully();
                    });
                })
                .Show();
        }

        // ���ñ�����ɫ
        private void SetInputFieldBGColor(TMP_InputField inputField, Color color)
        {
            inputField.transform.Find("Background").GetComponent<Image>().color = color;
        }

        // ��֤�Ƿ�ȫ������
        private bool ValidateInputs()
        {
            bool isAllInput = true;
            foreach (var field in mOptionInputFields)
            {
                if (field.text.IsNullOrEmpty())
                {
                    isAllInput = false;
                    SetInputFieldBGColor(field, Color.red);
                }
            }
            return isAllInput;
        }

        protected override void OnHide()
        {
        }

        protected override void OnClose()
        {
        }
    }
}
