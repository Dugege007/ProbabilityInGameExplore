using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProbabilityTest
{
    public class Global : Architecture<Global>
    {
        public static Subject Subject = new Subject();
        // ����������ռ䣬��ʱʹ��Ȩ��
        public static SampleSpace SampleSpace = new SampleSpace(Subject.Name, CalMode.Weight);

        public static Dictionary<string, Subject> SubjectHistory = new Dictionary<string, Subject>();
        public static string SubjectHistoryKey = "subject_history";

        public static Dictionary<string, SampleSpace> SampleSpaceHistory = new Dictionary<string, SampleSpace>();
        public static string SampleSpaceHistoryKey = "sample_space_history";

        protected override void Init()
        {
        }

        [RuntimeInitializeOnLoadMethod]
        public static void AutoInit()
        {
            // ��ʼ����Դ
            ResKit.Init();
            // ����֡��
            Application.targetFrameRate = 60;
            // ���� UI ����ֱ���
            UIKit.Root.SetResolution(1080, 1920, 0.5f);
            // ������Ƶ����ģʽΪ����ͬ��Ƶ�� 10 ֡�ڲ��ظ�����
            AudioKit.PlaySoundMode = AudioKit.PlaySoundModes.IgnoreSameSoundInGlobalFrames;

            // ������ʼ��
            IArchitecture _ = Interface;
        }

        public static void ResetData()
        {
            Subject = new Subject();
            SampleSpace = new SampleSpace(Subject.Name, CalMode.Weight);
        }

    }
}
