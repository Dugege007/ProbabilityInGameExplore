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
        private List<TMP_InputField> mFocusInputFields = new List<TMP_InputField>();
        private List<Slider> mFocusSliders = new List<Slider>();
        private List<TMP_Text> mTMPFocusLabels = new List<TMP_Text>();

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
                if (mFocusInputFields.Count < 1)
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

                    // �������ռ���������б�����ӹ�ע��
                    foreach (var inputField in mFocusInputFields)
                    {
                        // �����ѡ���п����ҵ����еĹ�ע��
                        if (historySubject.SampleSpace.GetSamplePointByName(inputField.text) != null)
                        {
                            // ��Ӹù�ע�㵽������
                            subject.SampleSpace.SamplePoints.Add(historySubject.SampleSpace.GetSamplePointByName(inputField.text));
                        }
                        else
                        {
                            // �Ҳ����������һ���µĹ�ע��
                            subject.SampleSpace.AddSamplePoint(inputField.text);
                        }
                    }
                    Debug.Log("����ʷ��¼����ȡ SampleSpace");
                }
                // ����
                else
                {
                    // ��������ռ��е�������
                    subject.SampleSpace.SamplePoints.Clear();
                    // ��ո�ѡ���еĹ�ע��
                    foreach(Option option in subject.Options)
                        option.Focuses.Clear();

                    // �����û��������������
                    for (int i = 0; i < mFocusInputFields.Count; i++)
                    {
                        subject.SampleSpace.AddSamplePoint(mFocusInputFields[i].text, mFocusSliders[i].value);
                        Debug.Log("����ӣ�" + subject.SampleSpace.SamplePoints[i].Name + ": " + subject.SampleSpace.SamplePoints[i].Value);
                    }
                }

                // Ϊ����ѡ����ӹ�ע��
                for (int i = 0; i < subject.Options.Count; i++)
                {
                    for (int j = 0; j < mFocusInputFields.Count; j++)
                    {
                        subject.Options[i].AddFocus(mFocusInputFields[j].text);
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
                    // ���������б�
                    mFocusInputFields.Add(inputField);

                    self.Label.text = "��ע�� " + mFocusIndex;
                    mTMPFocusLabels.Add(self.Label);

                    // ��ȡ������
                    Slider slider = self.FocusSlider;
                    slider.value = focusValue;
                    self.SliderText.text = focusValue.ToString("F1");
                    // ��ӻ��������б�
                    mFocusSliders.Add(slider);

                    // ���������
                    inputField.onEndEdit.AddListener(focusName =>
                    {
                        SetInputFieldBGColor(inputField, mOriginalFocusBGColor);

                        foreach (var field in mSameTextInputFields.Where(f => f != null))
                        {
                            SetInputFieldBGColor(field, mOriginalFocusBGColor);
                        }
                    });

                    self.RemoveBtn.onClick.AddListener(() =>
                    {
                        AudioKit.PlaySound(Sfx.CLICK);

                        mFocusInputFields.Remove(inputField);
                        mFocusSliders.Remove(slider);
                        mTMPFocusLabels.Remove(self.Label);
                        mFocusIndex--;

                        for (int i = 0; i < mTMPFocusLabels.Count; i++)
                        {
                            mTMPFocusLabels[i].text = "��ע�� " + (i + 1);
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

            for (int i = 0; i < mFocusInputFields.Count - 1; i++)
            {
                for (int j = i + 1; j < mFocusInputFields.Count; j++)
                {
                    if (mFocusInputFields[i].text == mFocusInputFields[j].text)
                    {
                        NotificationFocusSame.ShowNotification();
                        SetInputFieldBGColor(mFocusInputFields[i], Color.red);
                        SetInputFieldBGColor(mFocusInputFields[j], Color.red);
                        mSameTextInputFields.Add(mFocusInputFields[i]);
                        mSameTextInputFields.Add(mFocusInputFields[j]);
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
            foreach (var field in mFocusInputFields)
            {
                if (field.text.IsNullOrEmpty())
                {
                    isAllInput = false;
                    SetInputFieldBGColor(field, Color.red);
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
