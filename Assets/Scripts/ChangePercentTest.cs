using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangePercentTest : MonoBehaviour
{
    public GameObject CaseRootGO;
    public GameObject CaseTemplate; // Sliderģ��

    public Text CaseNameText;
    public Slider CasePercentSlider;
    public Text CasePercentText;
    public InputField CustomPercentInputField;

    private bool isRefreshing = false;

    // ���� ��������
    public string[] DiceFaces = new string[]
    { "Face1", "Face2", "Face3", "Face4", "Face5", "Face6" };

    // �����㼯�� Sample Point List
    public List<string> SamplePointList = new List<string>();
    // �����ռ� Sample Space
    private Dictionary<string, float> SampleSpaceDic = new Dictionary<string, float>();
    // ������ɵ�Slider����
    private Dictionary<string, GameObject> CaseObjects = new Dictionary<string, GameObject>();

    private void Start()
    {
        ResetWeight();

        GenerateSliderBar(SampleSpaceDic);

        //for (int i = 0; i < SampleSpaceDic.Count; i++)
        //{
        //    Debug.Log("�¼���" + DiceFaces[i] + "��" + "���ʣ�" + SampleSpaceDic[DiceFaces[i]]);
        //}
    }

    // ��Ŀ����ʵ���Ȩ��
    private void AdjustWeightByTargetPercent(string outcome, float targetPercent)
    {
        float totalWeight = GetTotalWeight(SampleSpaceDic);
        float currentWeight = SampleSpaceDic[outcome];
        float weightAdjustment = (targetPercent * totalWeight - currentWeight) / (1 - targetPercent);

        SampleSpaceDic[outcome] = currentWeight + weightAdjustment;
    }
    // �ȴ��㷨ȷ���󣬿��Խ������ռ��װ��һ���࣬������и��ֲ���

    // �����ʵ���Ȩ��
    private void AdjustWeightByRate(string outcome, float rate)
    {
        float currentWeight = SampleSpaceDic[outcome];

        SampleSpaceDic[outcome] = currentWeight * rate;
    }

    // ����Ȩ��
    private void ResetWeight()
    {
        SampleSpaceDic.Clear();

        for (int i = 0; i < DiceFaces.Length; i++)
        {
            SampleSpaceDic.Add(DiceFaces[i], 1f);
        }
    }

    // ��ȡ��Ȩ��
    private float GetTotalWeight(Dictionary<string, float> sampleSpaceDic)
    {
        float totalWeight = 0f;
        foreach (float weight in sampleSpaceDic.Values)
        {
            totalWeight += weight;
        }

        return totalWeight;
    }

    // ���ɸ�����Ļ�����
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
            targetPercent = Mathf.Clamp(targetPercent / 100f, 0.01f, 0.999f);  // ����ֵ��0.01��0.999֮��
            AdjustWeightByTargetPercent(key, targetPercent);
            RefreshSliders();
        }
    }

    // ��������ֵ�ı�ʱ����
    private void OnSliderValueChanged(string key, Slider slider)
    {
        if (isRefreshing)  // ����Ƿ�����ˢ��
        {
            return;
        }
        float newPercent = Mathf.Clamp(slider.value, 0.01f, 0.999f);  // ����ֵ��0.01��0.999֮��
        AdjustWeightByTargetPercent(key, newPercent);
        RefreshSliders();
    }

    // ˢ�����л��������ı�
    private void RefreshSliders()
    {
        isRefreshing = true;  // ���ñ��Ϊtrue

        float totalWeight = GetTotalWeight(SampleSpaceDic);
        List<string> keys = new List<string>(SampleSpaceDic.Keys);  // ����һ����ʱ�ļ��б�

        foreach (var key in keys)  // ������ʱ�ļ��б�
        {
            GameObject caseObj = CaseObjects[key];
            Slider casePercentSlider = caseObj.transform.GetComponent<Slider>();
            Text casePercentText = caseObj.transform.Find("CasePercentText").GetComponent<Text>();

            float percent = SampleSpaceDic[key] / totalWeight;
            casePercentSlider.value = Mathf.Clamp(percent, 0.01f, 0.99f);
            casePercentText.text = (percent * 100).ToString("F2") + "%";
        }

        isRefreshing = false;  // ���ñ��Ϊfalse
    }
}
