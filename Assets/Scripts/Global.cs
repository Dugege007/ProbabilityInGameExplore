using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProbabilityTest
{
    public class Global : Architecture<Global>
    {
        public static string SubjectName = "SubjectName";
        public static string SubjectDescription = "SubjectDescription";
        public static List<string> OptionList = new List<string>();
        public static List<string> FocusList = new List<string>();

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
