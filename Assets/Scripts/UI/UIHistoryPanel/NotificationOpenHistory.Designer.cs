/****************************************************************************
 * 2023.10 MSI
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ProbabilityTest
{
	public partial class NotificationOpenHistory
	{
		[SerializeField] public UnityEngine.UI.Button CloseBtn;
		[SerializeField] public UnityEngine.UI.Button YesBtn;
		[SerializeField] public UnityEngine.UI.Button CancelBtn;

		public void Clear()
		{
			CloseBtn = null;
			YesBtn = null;
			CancelBtn = null;
		}

		public override string ComponentName
		{
			get { return "NotificationOpenHistory";}
		}
	}
}
