using UnityEngine;
using QFramework;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Drawing;
using QAssetBundle;
using Unity.VisualScripting;
using System.Collections.ObjectModel;

namespace ProbabilityTest
{
    public class UIRatingPanelData : UIPanelData
    {
    }
    public partial class UIRatingPanel : UIPanel
    {
        private int mOptionIndex = 0;
        private string mOptionName = "";

        private List<Option> mGlobalOptions = new List<Option>();

        private List<RatingHolderTemplete> mRatingHolders = new List<RatingHolderTemplete>();

        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIRatingPanelData ?? new UIRatingPanelData();
            // please add init code here

            Global.IsTemporarilySave.RegisterWithInitValue(isTempSave =>
            {
                if (isTempSave)
                {
                    // ����
                    CalculateEveryFocusScore(mGlobalOptions[mOptionIndex]);

                    Global.Subject.IsHistory = true;
                    Global.HistorySubject = Global.Subject;
                    CloseSelf();
                    UIKit.OpenPanel<UIHomePanel>();
                }

            }).UnRegisterWhenGameObjectDestroyed(this);

            mGlobalOptions = Global.Subject.Options;

            foreach (var option in mGlobalOptions)
            {
                Debug.Log("mGlobalOptions��" + option.Name);
                foreach (var focus in option.Focuses)
                {
                    Debug.Log("focus��" + focus.Name);
                }
            }

            RatingHolderTemplete.Hide();
            ComputeBtn.Hide();

            SubjectText.text = Global.Subject.Name;
            RefreshOptionInfo();
            CreateRatingHolder();

            // ���� ��һ��ѡ�ť
            LastBtn.onClick.AddListener(() =>
            {
                AudioKit.PlaySound(Sfx.CLICK);

                int nextIndex = mOptionIndex - 1;
                if (nextIndex < 0)
                    nextIndex = mGlobalOptions.Count - 1;

                // ���㵱ǰ Score
                CalculateEveryFocusScore(mGlobalOptions[mOptionIndex]);
                // ��ǰ��� - 1
                mOptionIndex = nextIndex;

                // ����ѡ��
                RefreshOptionInfo();
                // ���û�����
                ResetSliderValue(mGlobalOptions[mOptionIndex]);
                // ���ѡ���Ƿ�ȫ������
                CheckAllOptionsUnLocked();
            });

            // ���� ��һ��ѡ�ť
            NextBtn.onClick.AddListener(() =>
            {
                AudioKit.PlaySound(Sfx.CLICK);

                int nextIndex = mOptionIndex + 1;
                if (nextIndex >= mGlobalOptions.Count)
                    nextIndex = 0;

                // ���㵱ǰ Score
                CalculateEveryFocusScore(mGlobalOptions[mOptionIndex]);
                // ��ǰ��� + 1
                mOptionIndex = nextIndex;

                // ����ѡ��
                RefreshOptionInfo();
                // ���û�����
                ResetSliderValue(mGlobalOptions[mOptionIndex]);
                // ���ѡ���Ƿ�ȫ������
                CheckAllOptionsUnLocked();
            });

            // ���� ��ʼ���㰴ť
            ComputeBtn.onClick.AddListener(() =>
            {
                AudioKit.PlaySound(Sfx.CLICK);

                // ���� Score
                CalculateEveryFocusScore(mGlobalOptions[mOptionIndex]);

                CloseSelf();
                // �򿪽�����
                UIKit.OpenPanel<UIResultPanel>();
            });
        }

        protected override void OnOpen(IUIData uiData = null)
        {
        }

        protected override void OnShow()
        {
        }

        private void RefreshOptionInfo()
        {
            mOptionName = Global.Subject.Options[mOptionIndex].Name;
            OptionRatingHolder.OptionText.text = "<b><size=84>" + (mOptionIndex + 1) + ". " +
                 mOptionName + "</b></size>\r\n" +
                "���϶����� Ϊ����";
        }

        private void CreateRatingHolder()
        {
            Option option = Global.Subject.Options[0];
            List<SamplePoint> samplePoints = Global.Subject.SampleSpace.SamplePoints;

            for (int i = 0; i < samplePoints.Count; i++)
            {
                RatingHolderTemplete.InstantiateWithParent(OptionRatingHolder)
                    .Self(self =>
                    {
                        mRatingHolders.Add(self);

                        self.RatingLabel.text = samplePoints[i].Name;
                        //Debug.Log(option.GetFocusByName(samplePoints[i].Name).Value);
                        self.RatingSlider.value = option.GetFocusByName(samplePoints[i].Name).Value;
                        self.SliderText.text = option.GetFocusByName(samplePoints[i].Name).Value.ToString("F1");

                        self.RatingSlider.onValueChanged.AddListener(value =>
                        {
                            // ����ѡ��
                            option.IsUnLocked = true;
                            self.SliderText.text = value.ToString("F1");
                            CheckAllOptionsUnLocked();
                        });
                    })
                    .Show();
            }
        }

        private void CalculateEveryFocusScore(Option option)
        {
            // ����ѡ��
            option.IsUnLocked = true;

            for (int i = 0; i < mRatingHolders.Count; i++)
            {
                option.Focuses[i].Value = mRatingHolders[i].RatingSlider.value;
                Debug.Log(option.Focuses[i].Name + " �÷֣�" + option.Focuses[i].Score);
            }
        }

        private void ResetSliderValue(Option option)
        {
            // �����ѡ�δ����
            if (mGlobalOptions[mOptionIndex].IsUnLocked == false)
            {
                // ���û������ͷ���
                for (int i = 0; i < mRatingHolders.Count; i++)
                {
                    mRatingHolders[i].RatingSlider.value = 1f;
                    option.Focuses[i].Value = 1f;
                }
            }
            else
            {
                // Ϊ��������ֵ
                for (int i = 0; i < mRatingHolders.Count; i++)
                {
                    mRatingHolders[i].RatingSlider.value = option.Focuses[i].Value;
                }
            }

            foreach (var ratingHolder in mRatingHolders)
                ratingHolder.Bubble.Hide();
        }

        private void CheckAllOptionsUnLocked()
        {
            bool isAllUnLocked = true;
            foreach (Option option in mGlobalOptions)
            {
                if (!option.IsUnLocked)
                {
                    isAllUnLocked = false;
                    break;
                }
            }

            if (isAllUnLocked)
                ComputeBtn.Show();
        }

        protected override void OnHide()
        {
        }

        protected override void OnClose()
        {
        }
    }
}
