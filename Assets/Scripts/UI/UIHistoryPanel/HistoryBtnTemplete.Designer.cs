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
		[SerializeField] public TMPro.TextMeshProUGUI DescriptionText;
		[SerializeField] public UnityEngine.UI.Toggle RemoveToggle;

		public void Clear()
		{
			Background = null;
			SubjectTitle = null;
			DateText = null;
			DescriptionText = null;
			RemoveToggle = null;
		}

		public override string ComponentName
		{
			get { return "HistoryBtnTemplete";}
		}
	}
}
