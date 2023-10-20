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
        private List<TMP_InputField> mOptionInputFields = new List<TMP_InputField>();
        private List<TMP_Text> mTMPOptionLabels = new List<TMP_Text>();

        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIStartPanelData ?? new UIStartPanelData();
            // please add init code here

            // ���� ѡ�������ģ��
            OptionInputFieldHolderTemplete.Hide();
            NotificationSubjectNull.Hide();
            NotificationOptionCount.Hide();
            NotificationOptionNull.Hide();

            SubjectInputField.onEndEdit.AddListener(subjectName =>
            {
                if (subjectName != null)
                    NotificationSubjectNull.Hide();
            });

            // ���� ���ѡ�ť
            AddOptionBtn.onClick.AddListener(() =>
            {
                AudioKit.PlaySound(Sfx.CLICK);

                CreateOptionInputField();

                if (mOptionInputFields.Count >= 1)
                    NotificationOptionCount.Hide();
            });

            // ���� ��һ����ť
            NextBtn.onClick.AddListener(() =>
            {
                AudioKit.PlaySound(Sfx.CLICK);

                if (SubjectInputField.text.IsNullOrEmpty())
                {
                    NotificationSubjectNull.Show();
                    return;
                }
                else
                {
                    NotificationSubjectNull.Hide();
                }

                if (mOptionInputFields.Count < 1)
                {
                    NotificationOptionCount.Show();
                    return;
                }

                bool isAllInput = true;
                foreach (var field in mOptionInputFields)
                {
                    if (field.text.IsNullOrEmpty())
                    {
                        isAllInput = false;
                        break;
                    }
                }

                if (isAllInput == false)
                {
                    NotificationOptionNull.Show();
                    return;
                }
                else
                {
                    NotificationOptionNull.Hide();
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
                    self.OptionInputField.onEndEdit.AddListener(optionName =>
                    {
                        if (optionName!=null)
                            NotificationOptionNull.Hide();
                    });
                    // ���������б�
                    mOptionInputFields.Add(self.OptionInputField);

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
