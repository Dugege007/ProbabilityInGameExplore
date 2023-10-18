/****************************************************************************
 * 2023.10 MSI
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using UnityEngine.UIElements;
using System.Collections;

namespace ProbabilityTest
{
    public partial class FocusHolderTemplete : UIElement
    {
        private Coroutine mHideBubbleCoroutine;

        private void Awake()
        {
            FocusSlider.maxValue = 10;
            FocusSlider.minValue = 0;

            Bubble.Hide();

            FocusSlider.onValueChanged.AddListener(value =>
            {
                Bubble.Show();

                SamplePoint samplePoint = Global.SampleSpace.GetSamplePointByName(FocusInputField.text);
                float newValue;

                if (Global.SampleSpace.Mode == CalMode.Percent)
                {
                    newValue = Mathf.Clamp(value, 0.0001f, 0.9999f);  // 限制值在0.0001到0.9999之间
                    Global.SampleSpace.AdjustPercent(samplePoint, newValue);
                }
                else
                {
                    Global.SampleSpace.AdjustWeight(samplePoint, value);
                    SliderText.text = value.ToString("F1");
                    //Debug.Log("TotalWeight: " + Global.SampleSpace.TotalWeight);
                }

                // 如果之前有 Coroutine 在运行，停止它
                if (mHideBubbleCoroutine != null)
                    StopCoroutine(mHideBubbleCoroutine);

                // 启动新的 Coroutine 来在两秒后隐藏 Bubble
                mHideBubbleCoroutine = StartCoroutine(HideBubbleAfterDelay(2.0f));
            });
        }

        private IEnumerator HideBubbleAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            Bubble.Hide();
        }

        protected override void OnBeforeDestroy()
        {
        }
    }
}