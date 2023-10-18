using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProbabilityTest
{
    public class Global : Architecture<Global>
    {
        public static Subject Subject = new Subject("SubjectName");
        // ����������ռ䣬��ʱʹ��Ȩ��
        public static SampleSpace SampleSpace = new SampleSpace(Subject.Name, CalMode.Weight);

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
    }
}
