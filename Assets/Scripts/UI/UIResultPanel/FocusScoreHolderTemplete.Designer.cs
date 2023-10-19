/****************************************************************************
 * 2023.10 MSI
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ProbabilityTest
{
	public partial class FocusScoreHolderTemplete
	{
		[SerializeField] public TMPro.TextMeshProUGUI FocusScoreText;
		[SerializeField] public TMPro.TextMeshProUGUI FocusPercentText;
		[SerializeField] public TMPro.TextMeshProUGUI Line;

		public void Clear()
		{
			FocusScoreText = null;
			FocusPercentText = null;
			Line = null;
		}

		public override string ComponentName
		{
			get { return "FocusScoreHolderTemplete";}
		}
	}
}
