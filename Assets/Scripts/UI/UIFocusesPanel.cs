using UnityEngine;
using UnityEngine.UI;
using QFramework;
using TMPro;
using System.Collections.Generic;
using QAssetBundle;

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

        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIFocusesPanelData ?? new UIFocusesPanelData();
            // please add init code here

            // 隐藏 关注点模板
            FocusHolderTemplete.Hide();

            // 展示之前输入的信息
            SetInfoText();
            // 创建一个关注点输入框
            CreateFocusHolder();

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

                // 清空样本空间中的样本点
                Global.SampleSpace.SamplePoints.Clear();

                // 根据用户操作添加样本点
                for (int i = 0; i < mFocusInputFields.Count; i++)
                {
                    Global.SampleSpace.AddSamplePoint(mFocusInputFields[i].text, mFocusSliders[i].value);
                    Debug.Log("已添加：" + Global.SampleSpace.SamplePoints[i].Name + ": " + Global.SampleSpace.SamplePoints[i].Value);
                }

                for (int i = 0;i < Global.Subject.Options.Count; i++)
                {
                    for (int j = 0; j < mFocusInputFields.Count; j++)
                    {
                        Global.Subject.Options[i].AddFocus(mFocusInputFields[j].text);
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
        private void CreateFocusHolder()
        {
            mFocusIndex++;

            FocusHolderTemplete.InstantiateWithParent(Content)
                .SiblingIndex(Content.childCount - 3)
                .Self(self =>
                {
                    // 获取输入框
                    TMP_InputField inputField = self.FocusInputField;
                    // 添加输入框到列表
                    mFocusInputFields.Add(inputField);

                    self.Label.text = "关注点 " + mFocusIndex;
                    mTMPFocusLabels.Add(self.Label);

                    // 获取滑动条
                    Slider slider = self.FocusSlider;
                    // 添加滑动条到列表
                    mFocusSliders.Add(slider);
                    // 禁用滑动条
                    slider.enabled = false;

                    // 当输入框不为空时，解锁滑动条
                    inputField.onEndEdit.AddListener(focusName =>
                    {
                        if (focusName == "")
                            slider.enabled = false;
                        else
                            slider.enabled = true;
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

        protected override void OnHide()
        {
        }

        protected override void OnClose()
        {
        }
    }
}
