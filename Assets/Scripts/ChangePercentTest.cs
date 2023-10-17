using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProbabilityTest
{
    public class ChangePercentTest : MonoBehaviour
    {
        public GameObject CaseRootGO;
        public GameObject CaseTemplate; // Slider模板

        public Text CaseNameText;
        public Slider CasePercentSlider;
        public Text CasePercentText;
        public InputField CustomPercentInputField;
        public Toggle IsLockedToggle;
        public Toggle IsRelatedToggle;
        public Dropdown ModeDropdown;

        private bool isRefreshing = false;

        // 例子 骰子六面
        public string[] DiceFaces = new string[]
        { "Face1", "Face2", "Face3", "Face4", "Face5", "Face6" };

        // 样本空间
        private SampleSpace DiceSampleSpace = new SampleSpace("DiceEvents", CalMode.Weight);

        // 存放生成的Slider对象
        private Dictionary<string, GameObject> CaseObjects = new Dictionary<string, GameObject>();

        private void Start()
        {
            for (int i = 0; i < DiceFaces.Length; i++)
            {
                DiceSampleSpace.AddSamplePoint(DiceFaces[i]);
            }

            DiceSampleSpace.ResetAllSamplePoints();

            // 初始化Dropdown
            ModeDropdown.ClearOptions();
            List<string> options = new List<string> { "按概率", "按权重" };
            ModeDropdown.AddOptions(options);

            // 设置初始选项
            ModeDropdown.value = (DiceSampleSpace.Mode == CalMode.Percent) ? 0 : 1;

            // 添加监听器
            ModeDropdown.onValueChanged.AddListener(OnCalModeChanged);
            Debug.Log("计算模式：" + DiceSampleSpace.Mode);

            GenerateSliderBar(DiceSampleSpace);
            RefreshSliders(DiceSampleSpace);
        }

        // 生成各情况的滑动条
        private void GenerateSliderBar(SampleSpace sampleSpace)
        {
            foreach (SamplePoint point in sampleSpace.SamplePoints)
            {
                string pointName = point.Name;
                float percent = point.Value;

                GameObject newCase = Instantiate(CaseTemplate, CaseRootGO.transform);
                newCase.name = pointName;
                newCase.SetActive(true);

                Slider casePercentSlider = newCase.transform.Find("CasePercentSlider").GetComponent<Slider>();
                casePercentSlider.value = percent;
                casePercentSlider.onValueChanged.AddListener(delegate { OnSliderValueChanged(pointName, casePercentSlider); });

                Text caseNameText = casePercentSlider.transform.Find("CaseNameText").GetComponent<Text>();
                caseNameText.text = pointName;

                Text casePercentText = casePercentSlider.transform.Find("CasePercentText").GetComponent<Text>();
                casePercentText.text = (percent * 100).ToString("F2") + "%";

                InputField customPercentInputField = casePercentText.transform.Find("CustomPercentInputField").GetComponent<InputField>();
                customPercentInputField.onEndEdit.AddListener(delegate { AdjustPercentByInputField(pointName, customPercentInputField); });

                Toggle isLockedToggle = customPercentInputField.transform.Find("IsLockedToggle").GetComponent<Toggle>();
                isLockedToggle.isOn = point.IsLocked;
                isLockedToggle.onValueChanged.AddListener(isOn => point.IsLocked = isOn);

                Toggle isRelatedToggle = customPercentInputField.transform.Find("IsRelatedToggle").GetComponent<Toggle>();
                isRelatedToggle.isOn = point.IsRelated;
                isLockedToggle.onValueChanged.AddListener(isOn => point.IsRelated = isOn);

                CaseObjects.Add(pointName, casePercentSlider.gameObject);
            }
        }

        // 按输入框中的值调整权重
        private void AdjustPercentByInputField(string outname, InputField inputField)
        {
            SamplePoint samplePoint = DiceSampleSpace.GetSamplePointByName(outname);

            if (float.TryParse(inputField.text, out float targetValue))
            {
                if (DiceSampleSpace.Mode == CalMode.Percent)
                {
                    targetValue = Mathf.Clamp(targetValue / 100f, 0.0001f, 0.9999f);  // 限制值在0.0001到0.9999之间
                    DiceSampleSpace.AdjustPercent(samplePoint, targetValue);
                }
                else
                {
                    targetValue = Mathf.Clamp(targetValue, 0, 10000f);
                    DiceSampleSpace.AdjustWeight(samplePoint, targetValue);
                }

                RefreshSliders(DiceSampleSpace);
            }
        }

        // 当滑动条值改变时调用
        private void OnSliderValueChanged(string outname, Slider slider)
        {
            if (isRefreshing) return;   // 检查是否正在刷新

            SamplePoint samplePoint = DiceSampleSpace.GetSamplePointByName(outname);
            float newValue;

            if (DiceSampleSpace.Mode == CalMode.Percent)
            {
                newValue = Mathf.Clamp(slider.value, 0.0001f, 0.9999f);  // 限制值在0.0001到0.9999之间
                DiceSampleSpace.AdjustPercent(samplePoint, newValue);
            }
            else
            {
                newValue = Mathf.Clamp(samplePoint.AdjustPercentByWeight(DiceSampleSpace.GetTotalWeight(), slider.value), 0, 10000f);
                DiceSampleSpace.AdjustWeight(samplePoint, newValue);
            }

            RefreshSliders(DiceSampleSpace);
        }

        // 刷新所有滑动条和文本
        private void RefreshSliders(SampleSpace sampleSpace)
        {
            isRefreshing = true;  // 设置标记为true
            float percent;

            if (DiceSampleSpace.Mode == CalMode.Percent)
            {
                foreach (SamplePoint point in sampleSpace.SamplePoints)
                {
                    percent = point.Value;

                    GameObject caseObj = CaseObjects[point.Name];
                    Slider casePercentSlider = caseObj.GetComponent<Slider>();  // 获取Slider组件
                    Text casePercentText = caseObj.transform.Find("CasePercentText").GetComponent<Text>();
                    InputField customPercentInputField = casePercentText.transform.Find("CustomPercentInputField").GetComponent<InputField>();

                    casePercentSlider.value = percent;  // 更新Slider的值
                    casePercentText.text = (percent * 100).ToString("F2") + "%";
                    customPercentInputField.text = (percent * 100).ToString("F2");
                }
            }
            else
            {
                float totalWeight = sampleSpace.GetTotalWeight();
                if (totalWeight > 0)
                {
                    foreach (SamplePoint point in sampleSpace.SamplePoints)
                    {
                        percent = point.Value / totalWeight;

                        GameObject caseObj = CaseObjects[point.Name];
                        Slider casePercentSlider = caseObj.GetComponent<Slider>();  // 获取Slider组件
                        Text casePercentText = caseObj.transform.Find("CasePercentText").GetComponent<Text>();
                        InputField customPercentInputField = casePercentText.transform.Find("CustomPercentInputField").GetComponent<InputField>();

                        casePercentSlider.value = percent;  // 更新Slider的值
                        casePercentText.text = (percent * 100).ToString("F2") + "%";
                        customPercentInputField.text = point.Value.ToString();
                    }
                }
                else
                {
                    Debug.LogWarning("总权重值错误，无法计算！");
                }
            }

            isRefreshing = false;  // 设置标记为false
        }

        // 监听 Dropdown 以切换模式
        private void OnCalModeChanged(int selectedIndex)
        {
            if (selectedIndex == 0)
            {
                DiceSampleSpace.WeightToPercent();
            }
            else if (selectedIndex == 1)
            {
                DiceSampleSpace.PercentToWeight();
            }

            // 刷新滑动条和其他UI元素
            RefreshSliders(DiceSampleSpace);
        }
    }
}
