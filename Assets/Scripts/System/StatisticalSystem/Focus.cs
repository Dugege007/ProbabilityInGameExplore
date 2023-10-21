
using UnityEngine.Rendering.Universal;

namespace ProbabilityTest
{
    public class Focus
    {
        public string Name { get; set; }
        public float Value { get; set; }

        private float mMaxScore = 0;
        public float MaxScore
        {
            get
            {
                SamplePoint samplePoint = Global.SampleSpace.GetSamplePointByName(Name);
                mMaxScore = samplePoint.Value * samplePoint.MaxValue;
                return mMaxScore;
            }
        }
        private float mScore = 0;
        public float Score
        {
            get
            {
                SamplePoint samplePoint = Global.SampleSpace.GetSamplePointByName(Name);
                mScore = samplePoint.Value * Value;
                return mScore;
            }
        }
        private float mPercent = 0;
        public float Percent
        {
            get
            {
                mPercent = Score / MaxScore;
                return mPercent;
            }
        }

        public Focus(string name, float value = 1f)
        {
            Name = name;
            Value = value;
        }
    }
}
