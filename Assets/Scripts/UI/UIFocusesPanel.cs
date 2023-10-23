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
                if (mFocusHolders.Count < 1)
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
                    // 替换样本点列表中的关注点
                    foreach (var focusHolder in mFocusHolders)
                    {
                        // 如果在选项中可以找到已有的关注点
                        if (historySubject.SampleSpace.GetSamplePointByName(focusHolder.FocusInputField.text) != null)
                        {
                            // 添加该关注点到缓存中
                            subject.SampleSpace.SamplePoints.Add(historySubject.SampleSpace.GetSamplePointByName(focusHolder.FocusInputField.text));
                        }
                        else
                        {
                            // 找不到，则添加一个新的关注点
                            subject.SampleSpace.AddSamplePoint(focusHolder.FocusInputField.text, focusHolder.FocusSlider.value);
                        }
                    }
                    Debug.Log("subject.SampleSpace.SamplePoints: " + subject.SampleSpace.SamplePoints.Count);

                    // 替换关注点列表中的关注点
                    foreach (var option in subject.Options)
                    {
                        List<Focus> focusList = new List<Focus>();

                        foreach (var focusHolder in mFocusHolders)
                        {
                            // 如果历史中此 Option 存在
                            if (historySubject.GetOptionByName(option.Name) != null)
                            {
                                // 获取历史中的此 Option 的名为 inputField.text 的 Focus
                                Focus focus = historySubject.GetOptionByName(option.Name).GetFocusByName(focusHolder.FocusInputField.text);

                                // 如果在选项中可以找到已有的关注点
                                if (focus != null)
                                {
                                    focusList.Add(focus);
                                    Debug.Log("在 Option 中找到已有的 Focus");
                                }
                                // 如果找不到
                                else
                                {
                                    // 则添加一个新的关注点
                                    focusList.Add(new Focus(focusHolder.FocusInputField.text));
                                    Debug.Log("已添加新的 Focus");
                                }
                            }
                            // 如果历史中此 Option 不存在
                            else
                            {
                                // 则在此 Option 添加一个新的关注点
                                focusList.Add(new Focus(focusHolder.FocusInputField.text));
                                Debug.Log("已在新 Option 中添加新的 Focus");
                            }
                        }
                        option.Focuses = focusList;
                        Debug.Log("option.Focuses.Count: " + option.Focuses.Count);
                    }

                }
                // 否则
                else
                {
                    // 清空样本空间中的样本点
                    subject.SampleSpace.SamplePoints.Clear();
                    // 清空各选项中的关注点
                    foreach (Option option in subject.Options)
                        option.Focuses.Clear();

                    // 根据用户操作添加样本点
                    for (int i = 0; i < mFocusHolders.Count; i++)
                    {
                        subject.SampleSpace.AddSamplePoint(mFocusHolders[i].FocusInputField.text, mFocusHolders[i].FocusSlider.value);

                        Debug.Log("已添加：" + subject.SampleSpace.SamplePoints[i].Name + ": " + subject.SampleSpace.SamplePoints[i].Value);
                    }

                    // 为所有选项添加关注点
                    for (int i = 0; i < subject.Options.Count; i++)
                    {
                        for (int j = 0; j < mFocusHolders.Count; j++)
                        {
                            subject.Options[i].AddFocus(mFocusHolders[j].FocusInputField.text);
                        }
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

                    self.Label.text = "关注点 " + mFocusIndex;

                    // 获取滑动条
                    Slider slider = self.FocusSlider;
                    slider.value = focusValue;
                    self.SliderText.text = focusValue.ToString("F1");

                    // 监听输入框
                    inputField.onEndEdit.AddListener(focusName =>
                    {
                        SetInputFieldBGColor(inputField, mOriginalFocusBGColor);

                        foreach (var field in mSameTextInputFields.Where(f => f != null))
                        {
                            SetInputFieldBGColor(field, mOriginalFocusBGColor);
                        }
                    });

                    // 添加此 Holder 到列表
                    mFocusHolders.Add(self);

                    self.RemoveBtn.onClick.AddListener(() =>
                    {
                        AudioKit.PlaySound(Sfx.CLICK);

                        mFocusHolders.Remove(self);

                        mFocusIndex--;

                        for (int i = 0; i < mFocusHolders.Count; i++)
                        {
                            mFocusHolders[i].Label.text = "关注点 " + (i + 1);
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

        // 验证是否全部输入
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
