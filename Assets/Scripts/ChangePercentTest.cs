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
        public Toggle IsLockedToggle;
        public Toggle IsRelatedToggle;
        public Dropdown ModeDropdown;

        private bool isRefreshing = false;

        // ���� ��������
        public string[] DiceFaces = new string[]
        { "Face1", "Face2", "Face3", "Face4", "Face5", "Face6" };

        // �����ռ�
        private SampleSpace DiceSampleSpace = new SampleSpace("DiceEvents", CalMode.Weight);

        // ������ɵ�Slider����
        private Dictionary<string, GameObject> CaseObjects = new Dictionary<string, GameObject>();

        private void Start()
        {
            for (int i = 0; i < DiceFaces.Length; i++)
            {
                DiceSampleSpace.AddSamplePoint(DiceFaces[i]);
            }

            DiceSampleSpace.ResetAllSamplePoints();

            // ��ʼ��Dropdown
            ModeDropdown.ClearOptions();
            List<string> options = new List<string> { "������", "��Ȩ��" };
            ModeDropdown.AddOptions(options);

            // ���ó�ʼѡ��
            ModeDropdown.value = (DiceSampleSpace.Mode == CalMode.Percent) ? 0 : 1;

            // ��Ӽ�����
            ModeDropdown.onValueChanged.AddListener(OnCalModeChanged);
            Debug.Log("����ģʽ��" + DiceSampleSpace.Mode);

            GenerateSliderBar(DiceSampleSpace);
            RefreshSliders(DiceSampleSpace);
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

        // ��������е�ֵ����Ȩ��
        private void AdjustPercentByInputField(string outname, InputField inputField)
        {
            SamplePoint samplePoint = DiceSampleSpace.GetSamplePointByName(outname);

            if (float.TryParse(inputField.text, out float targetValue))
            {
                if (DiceSampleSpace.Mode == CalMode.Percent)
                {
                    targetValue = Mathf.Clamp(targetValue / 100f, 0.0001f, 0.9999f);  // ����ֵ��0.0001��0.9999֮��
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

        // ��������ֵ�ı�ʱ����
        private void OnSliderValueChanged(string outname, Slider slider)
        {
            if (isRefreshing) return;   // ����Ƿ�����ˢ��

            SamplePoint samplePoint = DiceSampleSpace.GetSamplePointByName(outname);
            float newValue;

            if (DiceSampleSpace.Mode == CalMode.Percent)
            {
                newValue = Mathf.Clamp(slider.value, 0.0001f, 0.9999f);  // ����ֵ��0.0001��0.9999֮��
                DiceSampleSpace.AdjustPercent(samplePoint, newValue);
            }
            else
            {
                newValue = Mathf.Clamp(samplePoint.AdjustPercentByWeight(DiceSampleSpace.GetTotalWeight(), slider.value), 0, 10000f);
                DiceSampleSpace.AdjustWeight(samplePoint, newValue);
            }

            RefreshSliders(DiceSampleSpace);
        }

        // ˢ�����л��������ı�
        private void RefreshSliders(SampleSpace sampleSpace)
        {
            isRefreshing = true;  // ���ñ��Ϊtrue
            float percent;

            if (DiceSampleSpace.Mode == CalMode.Percent)
            {
                foreach (SamplePoint point in sampleSpace.SamplePoints)
                {
                    percent = point.Value;

                    GameObject caseObj = CaseObjects[point.Name];
                    Slider casePercentSlider = caseObj.GetComponent<Slider>();  // ��ȡSlider���
                    Text casePercentText = caseObj.transform.Find("CasePercentText").GetComponent<Text>();
                    InputField customPercentInputField = casePercentText.transform.Find("CustomPercentInputField").GetComponent<InputField>();

                    casePercentSlider.value = percent;  // ����Slider��ֵ
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
                        Slider casePercentSlider = caseObj.GetComponent<Slider>();  // ��ȡSlider���
                        Text casePercentText = caseObj.transform.Find("CasePercentText").GetComponent<Text>();
                        InputField customPercentInputField = casePercentText.transform.Find("CustomPercentInputField").GetComponent<InputField>();

                        casePercentSlider.value = percent;  // ����Slider��ֵ
                        casePercentText.text = (percent * 100).ToString("F2") + "%";
                        customPercentInputField.text = point.Value.ToString();
                    }
                }
                else
                {
                    Debug.LogWarning("��Ȩ��ֵ�����޷����㣡");
                }
            }

            isRefreshing = false;  // ���ñ��Ϊfalse
        }

        // ���� Dropdown ���л�ģʽ
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

            // ˢ�»�����������UIԪ��
            RefreshSliders(DiceSampleSpace);
        }
    }
}
