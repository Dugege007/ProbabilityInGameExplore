using QFramework;
using UnityEngine;

namespace ProbabilityTest
{
    public class SamplePoint
    {
        public string Name;
        private float mValue;

        public float Value
        {
            get { return mValue; }
            set { mValue = Mathf.Clamp(value, 0, 10000f); }
        }

        // 锁定
        public bool IsLocked;
        // 关联
        public bool IsRelated;

        public SamplePoint(string name, float value = 0, bool isLocked = false)
        {
            if (name.IsNullOrEmpty())
                Name = "NullEvent";
            else
                Name = name;

            mValue = value;
            IsLocked = isLocked;
        }

        /// <summary>
        /// 按目标概率调整权重
        /// </summary>
        /// <param name="targetPercent">目标概率</param>
        /// <remark>此方法无需关注样本空间中其他样本点，但需要注意数据溢出</remark>
        public float AdjustPercentByWeight(float totalWeight, float targetPercent)
        {
            targetPercent = Mathf.Clamp(targetPercent, 0, 10000f);

            //if (1 - targetPercent <= 0.0001f)
            //{
            //    Debug.LogError("targetPercent 不能是 1");
            //    return 0;
            //}

            float currentWeight = Value;
            float weightAdjustment = (targetPercent * totalWeight - currentWeight) / (1 - targetPercent);
            //Debug.Log("权重需调整的值：" + weightAdjustment);

            Value = currentWeight + weightAdjustment;
            return Value;
        }

        /// <summary>
        /// 按倍率调整权重
        /// </summary>
        /// <param name="rate">倍率</param>
        /// <remark>此方法无需关注样本空间中其他样本点，但需要注意数据溢出</remark>
        private void AdjustWeightByRate(float rate)
        {
            Value *= rate;
        }
    }
}
