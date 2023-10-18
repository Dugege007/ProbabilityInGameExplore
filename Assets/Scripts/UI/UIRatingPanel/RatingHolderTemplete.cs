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

                // ���֮ǰ�� Coroutine �����У�ֹͣ��
                if (mHideBubbleCoroutine != null)
                    StopCoroutine(mHideBubbleCoroutine);

                // �����µ� Coroutine ������������� Bubble
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