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

        private bool isRefreshing = false;

        // 例子 骰子六面
        public string[] DiceFaces = new string[]
        { "Face1", "Face2", "Face3", "Face4", "Face5", "Face6" };

        // 样本空间
        private SampleSpace DiceSampleSpace = new SampleSpace("DiceEvents");

        // 存放生成的Slider对象
        private Dictionary<string, GameObject> CaseObjects = new Dictionary<string, GameObject>();

        private void Start()
        {
            for (int i = 0; i < DiceFaces.Length; i++)
            {
                DiceSampleSpace.AddSamplePoint(DiceFaces[i]);
            }

            //for (int i = 0; i < DiceSampleSpace.SamplePoints.Count; i++)
            //{
            //    Debug.Log("样本点：" + DiceFaces[i] + "；" + "值：" + DiceSampleSpace.SamplePoints[i].Value);
            //}

            DiceSampleSpace.ResetAllSamplePoints();

            //for (int i = 0; i < DiceSampleSpace.SamplePoints.Count; i++)
            //{
            //    Debug.Log("样本点：" + DiceFaces[i] + "；" + "值：" + DiceSampleSpace.SamplePoints[i].Value);
            //}

            Debug.Log("是否使用权重：" + DiceSampleSpace.UseWeight);

            GenerateSliderBar(DiceSampleSpace);
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

                Text caseNameText = newCase.transform.Find("CaseNameText").GetComponent<Text>();
                Slider casePercentSlider = newCase.transform.GetComponent<Slider>();
                Text casePercentText = newCase.transform.Find("CasePercentText").GetComponent<Text>();
                InputField customPercentInputField = casePercentText.transform.Find("CustomPercentInputField").GetComponent<InputField>();

                caseNameText.text = pointName;

                casePercentSlider.value = percent;
                casePercentSlider.onValueChanged.AddListener(delegate { OnSliderValueChanged(pointName, casePercentSlider); });

                casePercentText.text = (percent * 100).ToString("F2") + "%";

                customPercentInputField.onEndEdit.AddListener(delegate { AdjustPercentInputField(pointName, customPercentInputField); });

                CaseObjects.Add(pointName, newCase);
            }
        }

        // 按输入框中的值调整权重
        private void AdjustPercentInputField(string outname, InputField inputField)
        {
            SamplePoint samplePoint = DiceSampleSpace.GetSamplePointByName(outname);

            if (float.TryParse(inputField.text, out float targetPercent))
            {
                targetPercent = Mathf.Clamp(targetPercent / 100f, 0.001f, 0.999f);  // 限制值在0.001到0.999之间

                if (DiceSampleSpace.UseWeight)
                    samplePoint.AdjustWeight(DiceSampleSpace.GetTotalWeight(), targetPercent);
                else
                    DiceSampleSpace.AdjustPercent(samplePoint, targetPercent);

                RefreshSliders(DiceSampleSpace);
            }
        }

        // 当滑动条值改变时调用
        private void OnSliderValueChanged(string outname, Slider slider)
        {
            if (isRefreshing) return;   // 检查是否正在刷新

            SamplePoint samplePoint = DiceSampleSpace.GetSamplePointByName(outname);
            float newPercent = Mathf.Clamp(slider.value, 0.001f, 0.999f);  // 限制值在0.001到0.999之间

            if (DiceSampleSpace.UseWeight)
                samplePoint.AdjustWeight(DiceSampleSpace.GetTotalWeight(), newPercent);
            else
                DiceSampleSpace.AdjustPercent(samplePoint, newPercent);

            RefreshSliders(DiceSampleSpace);
        }

        // 刷新所有滑动条和文本
        private void RefreshSliders(SampleSpace sampleSpace)
        {
            isRefreshing = true;  // 设置标记为true

            foreach (SamplePoint point in sampleSpace.SamplePoints)
            {
                float percent = point.Value;

                GameObject caseObj = CaseObjects[point.Name];
                Text casePercentText = caseObj.transform.Find("CasePercentText").GetComponent<Text>();
                Slider casePercentSlider = caseObj.GetComponent<Slider>();  // 获取Slider组件

                casePercentText.text = (percent * 100).ToString("F2") + "%";
                casePercentSlider.value = percent;  // 更新Slider的值
            }

            isRefreshing = false;  // 设置标记为false
        }
    }
}
