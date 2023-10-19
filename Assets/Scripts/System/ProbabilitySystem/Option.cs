using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProbabilityTest
{
    public class Option
    {
        public string Name;
        private float mScore = 0f;
        public float Score
        {
            get { return mScore; }
            set
            {
                mScore = value;
            }
        }
        public float Percent;

        public bool IsUnLocked = false;

        public List<Focus> Focuses { get; set; }

        public Option(string name)
        {
            Name = name;
            Focuses = new List<Focus>();
        }

        public void AddFocus(string focusName)
        {
            Focus focus = new Focus(focusName);
            Focuses.Add(focus);
        }

        public void RemoveOption(string focusName)
        {
            var focusToRemove = Focuses.FirstOrDefault(o => o.Name == focusName); // 使用 FirstOrDefault 避免抛出异常
            if (focusToRemove != null)
            {
                Focuses.Remove(focusToRemove);
            }
        }

        public Focus GetFocusByName(string focusName)
        {
            foreach (Focus focus in Focuses)
            {
                if (focus.Name.Equals(focusName))
                    return focus;
            }

            Debug.Log("GetFocusByName() 未找到名为：" + focusName + " 的 Focus");
            return null;
        }
    }
}
