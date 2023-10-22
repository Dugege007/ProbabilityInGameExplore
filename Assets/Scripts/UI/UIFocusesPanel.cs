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

            // 隐藏 关注点模板
            FocusHolderTemplete.Hide();
            NotificationFocusNull.Hide();
            NotificationFocusCount.Hide();
            NotificationFocusSame.Hide();

            // 展示之前输入的信息
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
                // 创建一个关注点输入框
                CreateFocusHolder();
            }

            // 监听 添加关注点按钮
            AddFocusBtn.onClick.AddListener(() =>
            {
                AudioKit.PlaySound(Sfx.CLICK);

                CreateFocusHolder();
            });

            // 监听 下一步按钮
            NextBtn.onClick.AddListener(() =>
            {
                AudioKit.PlaySound(Sfx.CLICK);

                // 检查是否全部输入
                if (ValidateInputs() == false)
                {
                    NotificationFocusNull.ShowNotification();
                    return;
                }

                // 检查关注点是否小于 1
                if (mFocusInputFields.Count < 1)
                {
                    NotificationFocusCount.ShowNotification();
                    return;
                }

                // 检查是否有相同的输入
                if (CheckSameInput() == false)
                {
                    return;
                }

                // 如果是历史记录
                if (historySubject.IsHistory)
                {
                    // SampleSpace 缓存
                    subject.SampleSpace.SamplePoints.Clear();

                    // 在样本空间的样本点列表中添加关注点
                    foreach (var inputField in mFocusInputFields)
                    {
                        // 如果在选项中可以找到已有的关注点
                        if (historySubject.SampleSpace.GetSamplePointByName(inputField.text) != null)
                        {
                            // 添加该关注点到缓存中
                            subject.SampleSpace.SamplePoints.Add(historySubject.SampleSpace.GetSamplePointByName(inputField.text));
                        }
                        else
                        {
                            // 找不到，则添加一个新的关注点
                            subject.SampleSpace.AddSamplePoint(inputField.text);
                        }
                    }
                    Debug.Log("从历史记录中提取 SampleSpace");
                }
                // 否则
                else
                {
                    // 清空样本空间中的样本点
                    subject.SampleSpace.SamplePoints.Clear();
                    // 清空各选项中的关注点
                    foreach(Option option in subject.Options)
                        option.Focuses.Clear();

                    // 根据用户操作添加样本点
                    for (int i = 0; i < mFocusInputFields.Count; i++)
                    {
                        subject.SampleSpace.AddSamplePoint(mFocusInputFields[i].text, mFocusSliders[i].value);
                        Debug.Log("已添加：" + subject.SampleSpace.SamplePoints[i].Name + ": " + subject.SampleSpace.SamplePoints[i].Value);
                    }
                }

                // 为所有选项添加关注点
                for (int i = 0; i < subject.Options.Count; i++)
                {
                    for (int j = 0; j < mFocusInputFields.Count; j++)
                    {
                        subject.Options[i].AddFocus(mFocusInputFields[j].text);
                    }
                }

                CloseSelf();
                // 打开评分面板
                UIKit.OpenPanel<UIRatingPanel>();
            });
        }

        protected override void OnOpen(IUIData uiData = null)
        {
        }

        protected override void OnShow()
        {
        }

        // 设置信息
        private void SetInfoText()
        {
            string subjectNameStr = Global.Subject.Name + "\r\n\r\n";
            string optionsListStr = "";

            for (int i = 0; i < Global.Subject.Options.Count; i++)
            {
                optionsListStr += (i + 1).ToString() + ". " + Global.Subject.Options[i].Name + "\r\n";
            }

            InfoText.text = "<b><size=42>主题：</size></b>\r\n" +
                subjectNameStr +
                "<b><size=42>选项：</size></b>\r\n" +
                optionsListStr +
                " ";
        }

        // 创建关注点条目
        private void CreateFocusHolder(string focusText = "", float focusValue = 1f)
        {
            mFocusIndex++;

            FocusHolderTemplete.InstantiateWithParent(Content)
                .SiblingIndex(Content.childCount - 3)
                .Self(self =>
                {
                    // 获取输入框
                    TMP_InputField inputField = self.FocusInputField;
                    inputField.text = focusText;
                    // 添加输入框到列表
                    mFocusInputFields.Add(inputField);

                    self.Label.text = "关注点 " + mFocusIndex;
                    mTMPFocusLabels.Add(self.Label);

                    // 获取滑动条
                    Slider slider = self.FocusSlider;
                    slider.value = focusValue;
                    self.SliderText.text = focusValue.ToString("F1");
                    // 添加滑动条到列表
                    mFocusSliders.Add(slider);

                    // 监听输入框
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
                            mTMPFocusLabels[i].text = "关注点 " + (i + 1);
                        }

                        self.DestroyGameObjGracefully();
                    });
                })
                .Show();
        }

        // 判断是否多个输入相同
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

        // 验证是否全部输入
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

        // 设置背景颜色
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
