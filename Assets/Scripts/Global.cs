using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProbabilityTest
{
    public class Global : Architecture<Global>
    {
        public static Subject Subject = new Subject("SubjectName");
        // 主题的样本空间，暂时使用权重
        public static SampleSpace SampleSpace = new SampleSpace(Subject.Name, CalMode.Weight);

        protected override void Init()
        {
        }

        [RuntimeInitializeOnLoadMethod]
        public static void AutoInit()
        {
            // 初始化资源
            ResKit.Init();
            // 设置帧率
            Application.targetFrameRate = 60;
            // 设置 UI 适配分辨率
            UIKit.Root.SetResolution(1080, 1920, 0.5f);
            // 设置音频播放模式为：相同音频在 10 帧内不重复播放
            AudioKit.PlaySoundMode = AudioKit.PlaySoundModes.IgnoreSameSoundInGlobalFrames;

            // 主动初始化
            IArchitecture _ = Interface;
        }
    }
}
