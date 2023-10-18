/****************************************************************************
 * 2023.10 MSI
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System.Collections;

namespace ProbabilityTest
{
    public partial class RatingHolderTemplete : UIElement
    {
        private Coroutine mHideBubbleCoroutine;

        private void Awake()
        {
            RatingSlider.maxValue = 10;
            RatingSlider.minValue = 0;

            Bubble.Hide();

            RatingSlider.onValueChanged.AddListener(value =>
            {
                Bubble.Show();

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