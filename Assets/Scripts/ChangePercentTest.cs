using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // 样本点集合 Sample Point List
    public List<string> SamplePointList = new List<string>();
    // 样本空间 Sample Space
    private Dictionary<string, float> SampleSpaceDic = new Dictionary<string, float>();
    // 存放生成的Slider对象
    private Dictionary<string, GameObject> CaseObjects = new Dictionary<string, GameObject>();

    private void Start()
    {
        ResetWeight();

        GenerateSliderBar(SampleSpaceDic);

        //for (int i = 0; i < SampleSpaceDic.Count; i++)
        //{
        //    Debug.Log("事件：" + DiceFaces[i] + "；" + "概率：" + SampleSpaceDic[DiceFaces[i]]);
        //}
    }

    // 按目标概率调整权重
    private void AdjustWeightByTargetPercent(string outcome, float targetPercent)
    {
        float totalWeight = GetTotalWeight(SampleSpaceDic);
        float currentWeight = SampleSpaceDic[outcome];
        float weightAdjustment = (targetPercent * totalWeight - currentWeight) / (1 - targetPercent);

        SampleSpaceDic[outcome] = currentWeight + weightAdjustment;
    }
    // 等此算法确定后，可以将样本空间封装成一个类，方便进行各种操作

    // 按倍率调整权重
    private void AdjustWeightByRate(string outcome, float rate)
    {
        float currentWeight = SampleSpaceDic[outcome];

        SampleSpaceDic[outcome] = currentWeight * rate;
    }

    // 重置权重
    private void ResetWeight()
    {
        SampleSpaceDic.Clear();

        for (int i = 0; i < DiceFaces.Length; i++)
        {
            SampleSpaceDic.Add(DiceFaces[i], 1f);
        }
    }

    // 获取总权重
    private float GetTotalWeight(Dictionary<string, float> sampleSpaceDic)
    {
        float totalWeight = 0f;
        foreach (float weight in sampleSpaceDic.Values)
        {
            totalWeight += weight;
        }

        return totalWeight;
    }

    // 生成各情况的滑动条
    private void GenerateSliderBar(Dictionary<string, float> sampleSpaceDic)
    {
        foreach (var key in sampleSpaceDic.Keys)
        {
            GameObject newCase = Instantiate(CaseTemplate, CaseRootGO.transform);
            newCase.name = key;
            newCase.SetActive(true);

            Text caseNameText = newCase.transform.Find("CaseNameText").GetComponent<Text>();
            Slider casePercentSlider = newCase.transform.GetComponent<Slider>();
            Text casePercentText = newCase.transform.Find("CasePercentText").GetComponent<Text>();
            InputField customPercentInputField = casePercentText.transform.Find("CustomPercentInputField").GetComponent<InputField>();

            caseNameText.text = key;

            float totalWeight = GetTotalWeight(sampleSpaceDic);
            float percent = sampleSpaceDic[key] / totalWeight;
            casePercentSlider.value = percent;
            casePercentSlider.onValueChanged.AddListener(delegate { OnSliderValueChanged(key, casePercentSlider); });

            casePercentText.text = (percent * 100).ToString("F2") + "%";

            customPercentInputField.onEndEdit.AddListener(delegate { AdjustWeightByInputField(key, customPercentInputField); });

            CaseObjects.Add(key, newCase);
        }
    }

    private void AdjustWeightByInputField(string key, InputField inputField)
    {
        float targetPercent;
        if (float.TryParse(inputField.text, out targetPercent))
        {
            targetPercent = Mathf.Clamp(targetPercent / 100f, 0.01f, 0.999f);  // 限制值在0.01到0.999之间
            AdjustWeightByTargetPercent(key, targetPercent);
            RefreshSliders();
        }
    }

    // 当滑动条值改变时调用
    private void OnSliderValueChanged(string key, Slider slider)
    {
        if (isRefreshing)  // 检查是否正在刷新
        {
            return;
        }
        float newPercent = Mathf.Clamp(slider.value, 0.01f, 0.999f);  // 限制值在0.01到0.999之间
        AdjustWeightByTargetPercent(key, newPercent);
        RefreshSliders();
    }

    // 刷新所有滑动条和文本
    private void RefreshSliders()
    {
        isRefreshing = true;  // 设置标记为true

        float totalWeight = GetTotalWeight(SampleSpaceDic);
        List<string> keys = new List<string>(SampleSpaceDic.Keys);  // 创建一个临时的键列表

        foreach (var key in keys)  // 遍历临时的键列表
        {
            GameObject caseObj = CaseObjects[key];
            Slider casePercentSlider = caseObj.transform.GetComponent<Slider>();
            Text casePercentText = caseObj.transform.Find("CasePercentText").GetComponent<Text>();

            float percent = SampleSpaceDic[key] / totalWeight;
            casePercentSlider.value = Mathf.Clamp(percent, 0.01f, 0.99f);
            casePercentText.text = (percent * 100).ToString("F2") + "%";
        }

        isRefreshing = false;  // 设置标记为false
    }
}
