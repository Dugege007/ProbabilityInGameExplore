using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProbabilityTest
{
    public class Global : Architecture<Global>
    {
        public static Subject Subject = new Subject();
        public static Subject HistorySubject = new Subject();

        public static BindableProperty<bool> IsTemporarilySave = new(false);

        protected override void Init()
        {
            this.RegisterSystem(new SaveSystem());
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
            HistorySubject = new Subject();
        }
    }
}
