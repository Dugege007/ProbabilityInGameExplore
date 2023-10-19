/****************************************************************************
 * 2023.10 MSI
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ProbabilityTest
{
	public partial class OptionResultHolderTemplete
	{
		[SerializeField] public TMPro.TextMeshProUGUI OptionLabel;
		[SerializeField] public TMPro.TextMeshProUGUI OptionScoreText;
		[SerializeField] public TMPro.TextMeshProUGUI OptionPercentText;

		public void Clear()
		{
			OptionLabel = null;
			OptionScoreText = null;
			OptionPercentText = null;
		}

		public override string ComponentName
		{
			get { return "OptionResultHolderTemplete";}
		}
	}
}
