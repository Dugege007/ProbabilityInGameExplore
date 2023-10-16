using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProbabilityTest
{
    public class SampleSpace
    {
        public string Name;
        public List<SamplePoint> SamplePoints = new List<SamplePoint>();
        // 是否使用权重
        public bool UseWeight = false;

        public SampleSpace(string name = "DefaultSampleSpace", bool useWeight = false)
        {
            Name = name;
            UseWeight = useWeight;
        }

        /// <summary>
        /// 添加样本点
        /// </summary>
        /// <param name="samplePointName">样本点名称</param>
        public void AddSamplePoint(string samplePointName)
        {
            if (CheckSamplePointExist(samplePointName))
            {
                Debug.LogWarning("SamplePoint：" + samplePointName + " 已存在!");
                return;
            }

            SamplePoint samplePoint;
            if (UseWeight)
            {
                samplePoint = new SamplePoint(samplePointName, 1f);
                SamplePoints.Add(samplePoint);
            }
            else
            {
                AddSamplePoint(samplePointName, 0f);
            }
        }

        /// <summary>
        /// 添加样本点
        /// </summary>
        /// <param name="samplePointName">样本点名称</param>
        /// <param name="value">值（概率或权重）</param>
        public void AddSamplePoint(string samplePointName, float value)
        {
            if (CheckSamplePointExist(samplePointName))
            {
                Debug.LogWarning("添加失败！SamplePoint：" + samplePointName + " 已存在!");
                return;
            }

            SamplePoint samplePoint;
            if (UseWeight)
                samplePoint = new SamplePoint(samplePointName, Mathf.Clamp(value, 1f, 10000f));
            else
                samplePoint = new SamplePoint(samplePointName, Mathf.Clamp01(value));

            SamplePoints.Add(samplePoint);
        }

        /// <summary>
        /// 删除样本点
        /// </summary>
        /// <param name="samplePoint">样本点</param>
        public void RemoveSamplePoint(SamplePoint samplePoint)
        {
            if (CheckSamplePointValueEqualsZero(samplePoint))
            {
                SamplePoints.Remove(samplePoint);
            }
            else
            {
                SetSamplePointToZero(samplePoint);
                SamplePoints.Remove(samplePoint);
            }
        }

        /// <summary>
        /// 按目标概率调整百分比
        /// </summary>
        /// <param name="samplePoint">样本点</param>
        /// <param name="targetPercent">目标百分比</param>
        public void AdjustPercent(SamplePoint samplePoint, float targetPercent)
        {
            // 获取除了 samplePoint 之外的 且没有锁定的，所有样本点的列表
            List<SamplePoint> needChangePoints = SamplePoints.Where(p => p != samplePoint && !p.IsLocked).ToList();
            // 无变化的列表，包括当前 samplePoint 和锁定的样本点
            List<SamplePoint> noChangePoints = SamplePoints.Where(p => p == samplePoint || p.IsLocked).ToList();

            // 计算需要变化的样本点的总百分比
            float totalOtherPercent = 0;
            foreach (SamplePoint point in needChangePoints)
            {
                totalOtherPercent += point.Value;
                // 注意：totalOtherPercent 不能等于 0
            }

            samplePoint.Value = targetPercent;

            // 计算调整后的不包括 当前和其他锁定的样本点的 概率总和
            float newPotalOtherPercent = 1f;
            foreach (SamplePoint point in noChangePoints)
            {
                newPotalOtherPercent -= point.Value;
            }

            // 遍历需要变化的样本点
            foreach (SamplePoint point in needChangePoints)
            {
                if (totalOtherPercent <= 0.000001f)
                {
                    point.Value = 0;
                    continue;
                }
                // 按比例调整其他的百分比
                float newPercent = (point.Value / totalOtherPercent) * newPotalOtherPercent;
                point.Value = newPercent;
            }
        }

        /// <summary>
        /// 按倍率调整百分比
        /// </summary>
        /// <param name="samplePoint">样本点</param>
        /// <param name="rate">倍率</param>
        public void AdjustPercentByRate(SamplePoint samplePoint, float rate)
        {
            List<SamplePoint> needChangePoints = SamplePoints.Where(p => p != samplePoint && !p.IsLocked).ToList();
            List<SamplePoint> noChangePoints = SamplePoints.Where(p => p == samplePoint || p.IsLocked).ToList();

            float totalOtherPercent = 0;
            foreach (SamplePoint point in needChangePoints)
            {
                totalOtherPercent += point.Value;
            }

            samplePoint.Value *= rate;

            float newPotalOtherPercent = 1f;
            foreach (SamplePoint point in noChangePoints)
            {
                newPotalOtherPercent -= point.Value;
            }

            foreach (SamplePoint point in SamplePoints.Where(p => p != samplePoint && !p.IsLocked))
            {
                if (totalOtherPercent <= 0.000001f)
                {
                    point.Value = 0;
                    continue;
                }
                float newPercent = (point.Value / totalOtherPercent) * newPotalOtherPercent;
                point.Value = newPercent;
            }
        }

        /// <summary>
        /// 根据名称获取样本点
        /// </summary>
        /// <param name="samplePointName">样本点名称</param>
        /// <returns>样本点</returns>
        public SamplePoint GetSamplePointByName(string samplePointName)
        {
            foreach (SamplePoint samplePoint in SamplePoints)
            {
                if (samplePoint.Name.Equals(samplePointName))
                    return samplePoint;
            }

            Debug.Log("GetSamplePointByName() 未找到名为：" + samplePointName + " 的 SamplePoint");
            return null;
        }

        /// <summary>
        /// 根据名称检查样本点是否存在
        /// </summary>
        /// <param name="samplePointName">样本点名称</param>
        /// <returns>是否存在</returns>
        public bool CheckSamplePointExist(string samplePointName)
        {
            // 使用 LINQ 检查样本点是否存在
            return SamplePoints.Any(p => p.Name == samplePointName);
        }

        /// <summary>
        /// 检查样本点的值是否为 0
        /// </summary>
        /// <param name="samplePoint">样本点</param>
        /// <returns>是否为 0</returns>
        public bool CheckSamplePointValueEqualsZero(SamplePoint samplePoint)
        {
            return samplePoint.Value == 0;
        }

        /// <summary>
        /// 获取总权重
        /// </summary>
        /// <returns>总权重</returns>
        public float GetTotalWeight()
        {
            float totalWeight = 0f;
            foreach (SamplePoint point in SamplePoints)
            {
                totalWeight += point.Value;
            }

            return totalWeight;
        }

        /// <summary>
        /// 权重值 转 概率
        /// </summary>
        public void WeightToPercent()
        {
            UseWeight = false;

            float totalWeight = GetTotalWeight();
            if (totalWeight > 0)
                foreach (SamplePoint point in SamplePoints)
                    point.Value /= totalWeight;
        }

        /// <summary>
        /// 将样本点置零
        /// </summary>
        public void SetSamplePointToZero(SamplePoint samplePoint)
        {
            if (UseWeight)
                WeightToPercent();

            AdjustPercent(samplePoint, 0);
        }

        /// <summary>
        /// 重置所有样本点
        /// </summary>
        public void ResetAllSamplePoints()
        {
            int amount = SamplePoints.Count;
            float targetPercent = 1f / amount;
            Debug.Log("所有 SamplePoint 的值重置为：" + targetPercent);

            if (UseWeight)
                foreach (SamplePoint point in SamplePoints.Where(p => !p.IsLocked && !p.IsLocked))
                    point.Value = 1f;
            else
                foreach (SamplePoint point in SamplePoints.Where(p => !p.IsLocked && !p.IsLocked))
                    point.Value = targetPercent;
        }
    }
}
