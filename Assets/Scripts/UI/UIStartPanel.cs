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

            // 隐藏 选项输入框模板
            OptionInputFieldHolderTemplete.Hide();
            NotificationSubjectNull.Hide();
            NotificationOptionCount.Hide();
            NotificationOptionNull.Hide();
            NotificationOptionSame.Hide();

            Global.IsTemporarilySave.RegisterWithInitValue(isTempSave =>
            {
                if (isTempSave)
                {
                    // 给 Global.Subject 赋值（暂存 Subject）
                    TemporarilySaveSubject();
                    Global.Subject.IsHistory = true;
                    Global.HistorySubject = Global.Subject;
                    CloseSelf();
                    UIKit.OpenPanel<UIHomePanel>();
                }

            }).UnRegisterWhenGameObjectDestroyed(this);

            Subject historySubject = Global.HistorySubject;

            if (historySubject.IsHistory)
            {
                DescriptionInputField.Select();
                SubjectInputField.Select();
                DescriptionInputField.text = historySubject.Description;
                SubjectInputField.text = historySubject.Name;

                foreach (Option option in historySubject.Options)
                {
                    CreateOptionInputField(option.Name);
                }
            }

            SubjectInputField.onEndEdit.AddListener(subjectName =>
            {

            });

            // 监听 添加选项按钮
            AddOptionBtn.onClick.AddListener(() =>
            {
                AudioKit.PlaySound(Sfx.CLICK);

                CreateOptionInputField();
            });

            // 监听 下一步 按钮
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

                // 给 Global.Subject 赋值（暂存 Subject）
                TemporarilySaveSubject();

                CloseSelf();
                // 打开关注点面板
                UIKit.OpenPanel<UIFocusesPanel>();
            });
        }

        protected override void OnOpen(IUIData uiData = null)
        {
        }

        protected override void OnShow()
        {
        }

        // 创建 选项输入框
        private void CreateOptionInputField(string optionText = "")
        {
            mOptionIndex++;

            OptionInputFieldHolderTemplete.InstantiateWithParent(Content)
                .SiblingIndex(Content.childCount - 3)
                .Self(self =>
                {
                    self.OptionInputField.text = optionText;
                    // 添加输入框到列表
                    mOptionInputFields.Add(self.OptionInputField);
                    self.OptionInputField.onEndEdit.AddListener(optionName =>
                    {
                        SetInputFieldBGColor(self.OptionInputField, mOriginalOptionBGColor);

                        foreach (var inputField in mSameTextInputFields.Where(f => f != null))
                            SetInputFieldBGColor(inputField, mOriginalOptionBGColor);
                    });

                    self.Label.text = "选项 " + mOptionIndex;
                    mOptionLabels.Add(self.Label);

                    self.RemoveBtn.onClick.AddListener(() =>
                    {
                        AudioKit.PlaySound(Sfx.CLICK);

                        mOptionInputFields.Remove(self.OptionInputField);
                        mOptionLabels.Remove(self.Label);
                        mOptionIndex--;

                        // 重设序号
                        for (int i = 0; i < mOptionLabels.Count; i++)
                            mOptionLabels[i].text = "选项 " + (i + 1);

                        self.DestroyGameObjGracefully();
                    });
                })
                .Show();
        }

        private void TemporarilySaveSubject()
        {
            Subject subject = Global.Subject;
            Subject historySubject = Global.HistorySubject;

            // 给主题和样本空间名赋值
            subject.Name = SubjectInputField.text;
            subject.Description = DescriptionInputField.text;

            if (historySubject.IsHistory)
            {
                subject.Options.Clear();

                foreach (var inputField in mOptionInputFields)
                {
                    if (historySubject.GetOptionByName(inputField.text) != null)
                    {
                        subject.Options.Add(historySubject.GetOptionByName(inputField.text));
                    }
                    else
                    {
                        subject.AddOption(inputField.text);
                        Debug.Log("添加新 Option：" + inputField.text);
                    }
                }
                Debug.Log("从历史记录中提取 Options");
            }
            else
            {
                // 清空主题中的选项的数据
                Global.Subject.Options.Clear();
                // 在 Subject 中添加 Option
                foreach (var inputField in mOptionInputFields)
                {
                    Global.Subject.AddOption(inputField.text);
                    Debug.Log("在 Subject 中添加 Option");
                }
            }
        }

        // 设置背景颜色
        private void SetInputFieldBGColor(TMP_InputField inputField, Color color)
        {
            inputField.transform.Find("Background").GetComponent<Image>().color = color;
        }

        // 验证是否全部输入
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
