using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangePercentTest : MonoBehaviour
{
    public GameObject CaseBoardGO;

    public Text CaseNameText;
    public Slider CasePercentSlider;
    public Text CasePercentText;
    public InputField CustomPercentInputField;

    // ���� ��������
    public string[] DiceFaces = new string[]
    { "Face1", "Face2", "Face3", "Face4", "Face5", "Face6" };

    // �����㼯�� Sample Point List
    public List<string> SamplePointList = new List<string>();
    // �����ռ� Sample Space
    private Dictionary<string, float> SampleSpaceDic = new Dictionary<string, float>();

    private void Start()
    {
        ResetWeight();



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
        float weightAdjustment = (targetPercent * totalWeight - currentWeight) / 1 - targetPercent;

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
    private void GenerateSliderBar()
    {

    }
}
