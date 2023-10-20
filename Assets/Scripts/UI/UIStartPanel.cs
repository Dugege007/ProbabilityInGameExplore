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

            // 隐藏 选项输入框模板
            OptionInputFieldHolderTemplete.Hide();
            NotificationSubjectNull.Hide();
            NotificationOptionCount.Hide();
            NotificationOptionNull.Hide();

            SubjectInputField.onEndEdit.AddListener(subjectName =>
            {
                if (subjectName != null)
                    NotificationSubjectNull.Hide();
            });

            // 监听 添加选项按钮
            AddOptionBtn.onClick.AddListener(() =>
            {
                AudioKit.PlaySound(Sfx.CLICK);

                CreateOptionInputField();

                if (mOptionInputFields.Count >= 1)
                    NotificationOptionCount.Hide();
            });

            // 监听 下一步按钮
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

                // 给主题和样本空间名赋值
                Global.SampleSpace.Name = SubjectInputField.text;
                Global.Subject.Name = SubjectInputField.text;

                // 清空主题中的选项的数据
                Global.Subject.Options.Clear();
                // 在 Subject 中添加 Option
                foreach (var inputField in mOptionInputFields)
                {
                    Global.Subject.AddOption(inputField.text);
                }

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
                    // 添加输入框到列表
                    mOptionInputFields.Add(self.OptionInputField);

                    self.Label.text = "选项 " + mOptionIndex;
                    mTMPOptionLabels.Add(self.Label);

                    self.RemoveBtn.onClick.AddListener(() =>
                    {
                        AudioKit.PlaySound(Sfx.CLICK);

                        mOptionInputFields.Remove(self.OptionInputField);
                        mTMPOptionLabels.Remove(self.Label);
                        mOptionIndex--;

                        // 重设序号
                        for (int i = 0; i < mTMPOptionLabels.Count; i++)
                            mTMPOptionLabels[i].text = "选项 " + (i + 1);

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
