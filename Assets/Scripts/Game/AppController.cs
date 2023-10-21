using UnityEngine;
using QFramework;

namespace ProbabilityTest
{
    public partial class AppController : ViewController
    {
        public static AppController mDefault;

        private void Awake()
        {
            mDefault = this;
        }

        private void Start()
        {
            UIKit.OpenPanel<UIHomePanel>();
        }

        private void OnDestroy()
        {
            mDefault = null;
        }
    }
}
