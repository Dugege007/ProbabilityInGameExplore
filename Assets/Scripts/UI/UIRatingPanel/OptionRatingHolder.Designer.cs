/****************************************************************************
 * 2023.10 MSI
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ProbabilityTest
{
	public partial class OptionRatingHolder
	{
		[SerializeField] public TMPro.TextMeshProUGUI OptionText;

		public void Clear()
		{
			OptionText = null;
		}

		public override string ComponentName
		{
			get { return "OptionRatingHolder";}
		}
	}
}
