/****************************************************************************
 * 2023.10 MSI
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ProbabilityTest
{
	public partial class FocusHolderTemplete
	{
		[SerializeField] public TMPro.TMP_InputField FocusInputField;
		[SerializeField] public TMPro.TextMeshProUGUI Label;
		[SerializeField] public UnityEngine.UI.Slider FocusSlider;
		[SerializeField] public UnityEngine.UI.Image Bubble;
		[SerializeField] public TMPro.TextMeshProUGUI BubbleText;
		[SerializeField] public TMPro.TextMeshProUGUI SliderText;

		public void Clear()
		{
			FocusInputField = null;
			Label = null;
			FocusSlider = null;
			Bubble = null;
			BubbleText = null;
			SliderText = null;
		}

		public override string ComponentName
		{
			get { return "FocusHolderTemplete";}
		}
	}
}
