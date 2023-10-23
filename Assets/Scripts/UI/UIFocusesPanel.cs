using UnityEngine;
using UnityEngine.UI;
using QFramework;
using TMPro;
using System.Collections.Generic;
using QAssetBundle;
using System.Linq;

namespace ProbabilityTest
{
    public class UIFocusesPanelData : UIPanelData
    {
    }
    public partial class UIFocusesPanel : UIPanel
    {
        private int mFocusIndex = 0;
        private List<FocusHolderTemplete> mFocusHolders = new List<FocusHolderTemplete>();

        private Color mOriginalFocusBGColor = new Color(77f / 255, 89f / 255, 89f / 255);
        private List<TMP_InputField> mSameTextInputFields = new List<TMP_InputField>();

        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIFocusesPanelData ?? new UIFocusesPanelData();
            // please add init code here

            // ���� ��ע��ģ��
            FocusHolderTemplete.Hide();
            NotificationFocusNull.Hide();
            NotificationFocusCount.Hide();
            NotificationFocusSame.Hide();

            // չʾ֮ǰ�������Ϣ
            SetInfoText();

            Subject subject = Global.Subject;
            Subject historySubject = Global.HistorySubject;

            if (historySubject.IsHistory)
            {
                foreach (SamplePoint point in historySubject.SampleSpace.SamplePoints)
                {
                    CreateFocusHolder(point.Name, point.Value);
                }
            }
            else
            {
                // ����һ����ע�������
                CreateFocusHolder();
            }

            // ���� ��ӹ�ע�㰴ť
            AddFocusBtn.onClick.AddListener(() =>
            {
                AudioKit.PlaySound(Sfx.CLICK);

                CreateFocusHolder();
            });

            // ���� ��һ����ť
            NextBtn.onClick.AddListener(() =>
            {
                AudioKit.PlaySound(Sfx.CLICK);

                // ����Ƿ�ȫ������
                if (ValidateInputs() == false)
                {
                    NotificationFocusNull.ShowNotification();
                    return;
                }

                // ����ע���Ƿ�С�� 1
                if (mFocusHolders.Count < 1)
                {
                    NotificationFocusCount.ShowNotification();
                    return;
                }

                // ����Ƿ�����ͬ������
                if (CheckSameInput() == false)
                {
                    return;
                }

                // �������ʷ��¼
                if (historySubject.IsHistory)
                {
                    // SampleSpace ����
                    subject.SampleSpace.SamplePoints.Clear();
                    // �滻�������б��еĹ�ע��
                    foreach (var focusHolder in mFocusHolders)
                    {
                        // �����ѡ���п����ҵ����еĹ�ע��
                        if (historySubject.SampleSpace.GetSamplePointByName(focusHolder.FocusInputField.text) != null)
                        {
                            // ��Ӹù�ע�㵽������
                            subject.SampleSpace.SamplePoints.Add(historySubject.SampleSpace.GetSamplePointByName(focusHolder.FocusInputField.text));
                        }
                        else
                        {
                            // �Ҳ����������һ���µĹ�ע��
                            subject.SampleSpace.AddSamplePoint(focusHolder.FocusInputField.text, focusHolder.FocusSlider.value);
                        }
                    }
                    Debug.Log("subject.SampleSpace.SamplePoints: " + subject.SampleSpace.SamplePoints.Count);

                    // �滻��ע���б��еĹ�ע��
                    foreach (var option in subject.Options)
                    {
                        List<Focus> focusList = new List<Focus>();

                        foreach (var focusHolder in mFocusHolders)
                        {
                            // �����ʷ�д� Option ����
                            if (historySubject.GetOptionByName(option.Name) != null)
                            {
                                // ��ȡ��ʷ�еĴ� Option ����Ϊ inputField.text �� Focus
                                Focus focus = historySubject.GetOptionByName(option.Name).GetFocusByName(focusHolder.FocusInputField.text);

                                // �����ѡ���п����ҵ����еĹ�ע��
                                if (focus != null)
                                {
                                    focusList.Add(focus);
                                    Debug.Log("�� Option ���ҵ����е� Focus");
                                }
                                // ����Ҳ���
                                else
                                {
                                    // �����һ���µĹ�ע��
                                    focusList.Add(new Focus(focusHolder.FocusInputField.text));
                                    Debug.Log("������µ� Focus");
                                }
                            }
                            // �����ʷ�д� Option ������
                            else
                            {
                                // ���ڴ� Option ���һ���µĹ�ע��
                                focusList.Add(new Focus(focusHolder.FocusInputField.text));
                                Debug.Log("������ Option ������µ� Focus");
                            }
                        }
                        option.Focuses = focusList;
                        Debug.Log("option.Focuses.Count: " + option.Focuses.Count);
                    }

                }
                // ����
                else
                {
                    // ��������ռ��е�������
                    subject.SampleSpace.SamplePoints.Clear();
                    // ��ո�ѡ���еĹ�ע��
                    foreach (Option option in subject.Options)
                        option.Focuses.Clear();

                    // �����û��������������
                    for (int i = 0; i < mFocusHolders.Count; i++)
                    {
                        subject.SampleSpace.AddSamplePoint(mFocusHolders[i].FocusInputField.text, mFocusHolders[i].FocusSlider.value);

                        Debug.Log("����ӣ�" + subject.SampleSpace.SamplePoints[i].Name + ": " + subject.SampleSpace.SamplePoints[i].Value);
                    }

                    // Ϊ����ѡ����ӹ�ע��
                    for (int i = 0; i < subject.Options.Count; i++)
                    {
                        for (int j = 0; j < mFocusHolders.Count; j++)
                        {
                            subject.Options[i].AddFocus(mFocusHolders[j].FocusInputField.text);
                        }
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
        private void CreateFocusHolder(string focusText = "", float focusValue = 1f)
        {
            mFocusIndex++;

            FocusHolderTemplete.InstantiateWithParent(Content)
                .SiblingIndex(Content.childCount - 3)
                .Self(self =>
                {
                    // ��ȡ�����
                    TMP_InputField inputField = self.FocusInputField;
                    inputField.text = focusText;

                    self.Label.text = "��ע�� " + mFocusIndex;

                    // ��ȡ������
                    Slider slider = self.FocusSlider;
                    slider.value = focusValue;
                    self.SliderText.text = focusValue.ToString("F1");

                    // ���������
                    inputField.onEndEdit.AddListener(focusName =>
                    {
                        SetInputFieldBGColor(inputField, mOriginalFocusBGColor);

                        foreach (var field in mSameTextInputFields.Where(f => f != null))
                        {
                            SetInputFieldBGColor(field, mOriginalFocusBGColor);
                        }
                    });

                    // ��Ӵ� Holder ���б�
                    mFocusHolders.Add(self);

                    self.RemoveBtn.onClick.AddListener(() =>
                    {
                        AudioKit.PlaySound(Sfx.CLICK);

                        mFocusHolders.Remove(self);

                        mFocusIndex--;

                        for (int i = 0; i < mFocusHolders.Count; i++)
                        {
                            mFocusHolders[i].Label.text = "��ע�� " + (i + 1);
                        }

                        self.DestroyGameObjGracefully();
                    });
                })
                .Show();
        }

        // �ж��Ƿ���������ͬ
        private bool CheckSameInput()
        {
            bool isAllDifferent = true;

            for (int i = 0; i < mFocusHolders.Count - 1; i++)
            {
                for (int j = i + 1; j < mFocusHolders.Count; j++)
                {
                    if (mFocusHolders[i].FocusInputField.text == mFocusHolders[j].FocusInputField.text)
                    {
                        NotificationFocusSame.ShowNotification();
                        SetInputFieldBGColor(mFocusHolders[i].FocusInputField, Color.red);
                        SetInputFieldBGColor(mFocusHolders[j].FocusInputField, Color.red);
                        mSameTextInputFields.Add(mFocusHolders[i].FocusInputField);
                        mSameTextInputFields.Add(mFocusHolders[j].FocusInputField);
                        isAllDifferent = false;
                    }
                }
            }

            return isAllDifferent;
        }

        // ��֤�Ƿ�ȫ������
        private bool ValidateInputs()
        {
            bool isAllInput = true;
            foreach (var focusHolder in mFocusHolders)
            {
                if (focusHolder.FocusInputField.text.IsNullOrEmpty())
                {
                    isAllInput = false;
                    SetInputFieldBGColor(focusHolder.FocusInputField, Color.red);
                }
            }
            return isAllInput;
        }

        // ���ñ�����ɫ
        private void SetInputFieldBGColor(TMP_InputField inputField, Color color)
        {
            inputField.transform.Find("Background").GetComponent<Image>().color = color;
        }

        protected override void OnHide()
        {
        }

        protected override void OnClose()
        {
        }
    }
}
