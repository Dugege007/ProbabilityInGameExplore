using UnityEngine;
using QFramework;

namespace ProbabilityTest
{
    public partial class AppController : ViewController
    {
        private void Start()
        {
            UIKit.OpenPanel<UIStartPanel>();
        }
    }
}
