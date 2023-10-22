using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProbabilityTest
{
    public class Subject
    {
        public string Name;
        public string Description;

        public bool IsHistory { get; set; } = false;

        public SampleSpace SampleSpace { get; set; }
        public List<Option> Options { get; set; }

        public Subject(string name = "SubjectName")
        {
            Name = name;
            Options = new List<Option>();  // 初始化列表
            SampleSpace = new SampleSpace("SampleSpace", CalMode.Weight);
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

            Debug.Log("GetOptionByName() 未找到名为：" + optionName + " 的 Option");
            return null;
        }
    }
}
