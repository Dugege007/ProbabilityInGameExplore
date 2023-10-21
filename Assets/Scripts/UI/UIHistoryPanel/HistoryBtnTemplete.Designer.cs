/****************************************************************************
 * 2023.10 MSI
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ProbabilityTest
{
	public partial class HistoryBtnTemplete
	{
		[SerializeField] public UnityEngine.UI.Image Background;
		[SerializeField] public TMPro.TextMeshProUGUI SubjectTitle;
		[SerializeField] public TMPro.TextMeshProUGUI DateText;
		[SerializeField] public TMPro.TextMeshProUGUI OptionsText;
		[SerializeField] public TMPro.TextMeshProUGUI FocusesText;

		public void Clear()
		{
			Background = null;
			SubjectTitle = null;
			DateText = null;
			OptionsText = null;
			FocusesText = null;
		}

		public override string ComponentName
		{
			get { return "HistoryBtnTemplete";}
		}
	}
}
