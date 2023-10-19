using UnityEngine;
using QFramework;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Drawing;
using QAssetBundle;

namespace ProbabilityTest
{
    public class UIRatingPanelData : UIPanelData
    {
    }
    public partial class UIRatingPanel : UIPanel
    {
        private int mOptionIndex = 0;
        private string mOptionName = "";

        private List<Option> mGlobalOptions = Global.Subject.Options;
        private List<Slider> mRatingSliders = new List<Slider>();

        private List<RatingHolderTemplete> mRatingHolderTempletes = new List<RatingHolderTemplete>();

        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIRatingPanelData ?? new UIRatingPanelData();
            // please add init code here

            RatingHolderTemplete.Hide();
            ComputeBtn.Hide();

            SubjectText.text = "<b><size=60>" + Global.Subject.Name + "</size></b>\r\n";
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
                CalculateOptionScore(mGlobalOptions[mOptionIndex]);
                // ��ǰ��� - 1
                mOptionIndex = nextIndex;

                // ����ѡ��
                RefreshOptionInfo();
                // ���û�����
                ResetSliderValue(mGlobalOptions[mOptionIndex]);

                bool isAllUnLocked = true;
                foreach (Option option in mGlobalOptions)
                {
                    if (option.IsUnLocked == false)
                    {
                        isAllUnLocked = false;
                        break;
                    }
                }

                if (isAllUnLocked)
                    ComputeBtn.Show();
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
                CalculateOptionScore(mGlobalOptions[mOptionIndex]);
                // ��ǰ��� + 1
                mOptionIndex = nextIndex;

                // ����ѡ��
                RefreshOptionInfo();
                // ���û�����
                ResetSliderValue(mGlobalOptions[mOptionIndex]);

                bool isAllUnLocked = true;
                foreach (Option option in mGlobalOptions)
                {
                    if (option.IsUnLocked == false)
                    {
                        isAllUnLocked = false;
                        break;
                    }
                }

                if (isAllUnLocked)
                    ComputeBtn.Show();
            });

            // ���� ��ʼ���㰴ť
            ComputeBtn.onClick.AddListener(() =>
            {
                AudioKit.PlaySound(Sfx.CLICK);

                // ���㵱ǰ Score
                CalculateEveryFocusScore(mGlobalOptions[mOptionIndex]);
                CalculateOptionScore(mGlobalOptions[mOptionIndex]);

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
            Option option = Global.Subject.GetOptionByName(mOptionName);

            for (int i = 0; i < Global.SampleSpace.SamplePoints.Count; i++)
            {
                SamplePoint point = Global.SampleSpace.SamplePoints[i];

                RatingHolderTemplete.InstantiateWithParent(OptionRatingHolder)
                    .Self(self =>
                    {
                        mRatingSliders.Add(self.RatingSlider);
                        mRatingHolderTempletes.Add(self);

                        self.RatingLabel.text = point.Name;

                        self.RatingSlider.onValueChanged.AddListener(value =>
                        {
                            // ����ѡ��
                            option.IsUnLocked = true;

                            self.SliderText.text = value.ToString("F1");
                        });
                    })
                    .Show();
            }
        }

        // �����ѡ����ܷ�
        private void CalculateOptionScore(Option option)
        {
            // ����ѡ��
            option.IsUnLocked = true;

            // ����ѡ�����
            option.Score = 0;
            foreach (Focus focus in option.Focuses)
            {
                option.Score += focus.Score;
            }
            Debug.Log("ѡ�� " + option.Name + " �÷֣�" + option.Score);
        }

        private void CalculateEveryFocusScore(Option option)
        {
            for (int i = 0; i < mRatingSliders.Count; i++)
            {
                option.Focuses[i].Score = Global.SampleSpace.SamplePoints[i].Value * mRatingSliders[i].value;
                Debug.Log(option.Focuses[i].Name + " �÷֣�" + option.Focuses[i].Score);
            }
        }

        private void ResetSliderValue(Option option)
        {
            // �����ѡ�δ����
            if (mGlobalOptions[mOptionIndex].IsUnLocked == false)
            {
                // ��ջ������ͷ���
                for (int i = 0; i < mRatingSliders.Count; i++)
                {
                    mRatingSliders[i].value = 0;
                    option.Focuses[i].Score = 0;
                }
            }
            else
            {
                // Ϊ��������ֵ
                for (int i = 0; i < mRatingSliders.Count; i++)
                {
                    mRatingSliders[i].value = option.Focuses[i].Score / Global.SampleSpace.SamplePoints[i].Value;
                }
            }

            foreach (var ratingHolder in mRatingHolderTempletes)
                ratingHolder.Bubble.Hide();
        }

        protected override void OnHide()
        {
        }

        protected override void OnClose()
        {
        }
    }
}
