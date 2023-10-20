using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProbabilityTest
{
    public class Subject
    {
        public string Name;
        public string Description;

        public List<Option> Options { get; set; }
        public float TotalWeight;

        public Subject(string name)
        {
            Name = name;
            Options = new List<Option>();  // 初始化列表
        }

        public void AddOption(string optionName)
        {
            Option option = new Option(optionName);
            Options.Add(option);
        }

        public void RemoveOption(string optionName)
        {
            var optionToRemove = Options.FirstOrDefault(o => o.Name == optionName); // 使用 FirstOrDefault 避免抛出异常
            if (optionToRemove != null)
            {
                Options.Remove(optionToRemove);
            }
        }

        public Option GetOptionByName(string optionName)
        {
            foreach (Option option in Options)
            {
                if (option.Name.Equals(optionName))
                    return option;
            }

            Debug.Log("GetSamplePointByName() 未找到名为：" + optionName + " 的 SamplePoint");
            return null;
        }

        public float GetTotalScore()
        {
            float totalWeight = 0f;
            foreach (SamplePoint point in Global.SampleSpace.SamplePoints)
            {
                totalWeight += point.Value * point.MaxValue;
            }
            return totalWeight;
        }
    }
}
