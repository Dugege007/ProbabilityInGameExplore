using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProbabilityTest
{
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

        // �����ռ�
        private SampleSpace DiceSampleSpace = new SampleSpace("DiceEvents");

        // ������ɵ�Slider����
        private Dictionary<string, GameObject> CaseObjects = new Dictionary<string, GameObject>();

        private void Start()
        {
            for (int i = 0; i < DiceFaces.Length; i++)
            {
                DiceSampleSpace.AddSamplePoint(DiceFaces[i]);
            }

            //for (int i = 0; i < DiceSampleSpace.SamplePoints.Count; i++)
            //{
            //    Debug.Log("�����㣺" + DiceFaces[i] + "��" + "ֵ��" + DiceSampleSpace.SamplePoints[i].Value);
            //}

            DiceSampleSpace.ResetAllSamplePoints();

            //for (int i = 0; i < DiceSampleSpace.SamplePoints.Count; i++)
            //{
            //    Debug.Log("�����㣺" + DiceFaces[i] + "��" + "ֵ��" + DiceSampleSpace.SamplePoints[i].Value);
            //}

            Debug.Log("�Ƿ�ʹ��Ȩ�أ�" + DiceSampleSpace.UseWeight);

            GenerateSliderBar(DiceSampleSpace);
        }

        // ���ɸ�����Ļ�����
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

        // ��������е�ֵ����Ȩ��
        private void AdjustPercentInputField(string outname, InputField inputField)
        {
            SamplePoint samplePoint = DiceSampleSpace.GetSamplePointByName(outname);

            if (float.TryParse(inputField.text, out float targetPercent))
            {
                targetPercent = Mathf.Clamp(targetPercent / 100f, 0.001f, 0.999f);  // ����ֵ��0.001��0.999֮��

                if (DiceSampleSpace.UseWeight)
                    samplePoint.AdjustWeight(DiceSampleSpace.GetTotalWeight(), targetPercent);
                else
                    DiceSampleSpace.AdjustPercent(samplePoint, targetPercent);

                RefreshSliders(DiceSampleSpace);
            }
        }

        // ��������ֵ�ı�ʱ����
        private void OnSliderValueChanged(string outname, Slider slider)
        {
            if (isRefreshing) return;   // ����Ƿ�����ˢ��

            SamplePoint samplePoint = DiceSampleSpace.GetSamplePointByName(outname);
            float newPercent = Mathf.Clamp(slider.value, 0.001f, 0.999f);  // ����ֵ��0.001��0.999֮��

            if (DiceSampleSpace.UseWeight)
                samplePoint.AdjustWeight(DiceSampleSpace.GetTotalWeight(), newPercent);
            else
                DiceSampleSpace.AdjustPercent(samplePoint, newPercent);

            RefreshSliders(DiceSampleSpace);
        }

        // ˢ�����л��������ı�
        private void RefreshSliders(SampleSpace sampleSpace)
        {
            isRefreshing = true;  // ���ñ��Ϊtrue

            foreach (SamplePoint point in sampleSpace.SamplePoints)
            {
                float percent = point.Value;

                GameObject caseObj = CaseObjects[point.Name];
                Text casePercentText = caseObj.transform.Find("CasePercentText").GetComponent<Text>();
                Slider casePercentSlider = caseObj.GetComponent<Slider>();  // ��ȡSlider���

                casePercentText.text = (percent * 100).ToString("F2") + "%";
                casePercentSlider.value = percent;  // ����Slider��ֵ
            }

            isRefreshing = false;  // ���ñ��Ϊfalse
        }
    }
}
