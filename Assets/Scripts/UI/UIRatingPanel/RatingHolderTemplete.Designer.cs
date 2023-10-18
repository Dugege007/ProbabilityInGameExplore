/****************************************************************************
 * 2023.10 MSI
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ProbabilityTest
{
	public partial class RatingHolderTemplete
	{
		[SerializeField] public TMPro.TextMeshProUGUI RatingLabel;
		[SerializeField] public UnityEngine.UI.Slider RatingSlider;
		[SerializeField] public UnityEngine.UI.Image Bubble;
		[SerializeField] public TMPro.TextMeshProUGUI BubbleText;
		[SerializeField] public TMPro.TextMeshProUGUI SliderText;

		public void Clear()
		{
			RatingLabel = null;
			RatingSlider = null;
			Bubble = null;
			BubbleText = null;
			SliderText = null;
		}

		public override string ComponentName
		{
			get { return "RatingHolderTemplete";}
		}
	}
}
