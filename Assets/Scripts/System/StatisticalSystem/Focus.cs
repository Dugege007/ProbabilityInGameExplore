
namespace ProbabilityTest
{
    public class Focus
    {
        public string Name { get; set; }
        private float mValue = 0;
        public float Value
        {
            get { return mValue; }
            set { mValue = value; }
        }

        private float mMaxScore = 0;
        public float MaxScore
        {
            get
            {
                mMaxScore = Global.SampleSpace.GetSamplePointByName(Name).Value * Global.SampleSpace.GetSamplePointByName(Name).MaxValue;
                return mMaxScore;
            }
        }
        private float mScore = 0;
        public float Score
        {
            get
            {
                mScore = Global.SampleSpace.GetSamplePointByName(Name).Value * mValue;
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
