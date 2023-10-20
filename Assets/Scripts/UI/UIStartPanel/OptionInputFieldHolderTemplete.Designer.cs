/****************************************************************************
 * 2023.10 MSI
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ProbabilityTest
{
	public partial class OptionInputFieldHolderTemplete
	{
		[SerializeField] public TMPro.TMP_InputField OptionInputField;
		[SerializeField] public TMPro.TextMeshProUGUI Label;
		[SerializeField] public UnityEngine.UI.Button RemoveBtn;

		public void Clear()
		{
			OptionInputField = null;
			Label = null;
			RemoveBtn = null;
		}

		public override string ComponentName
		{
			get { return "OptionInputFieldHolderTemplete";}
		}
	}
}
