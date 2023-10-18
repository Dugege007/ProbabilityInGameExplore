using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ProbabilityTest
{
	// Generate Id:3a52ad0d-48f4-49fd-943e-7ee49126582b
	public partial class UIFocusesPanel
	{
		public const string Name = "UIFocusesPanel";
		
		[SerializeField]
		public TMPro.TMP_InputField FocusInputFieldTemplete;
		[SerializeField]
		public TMPro.TextMeshProUGUI Label;
		[SerializeField]
		public RectTransform Content;
		[SerializeField]
		public UnityEngine.UI.Button AddFocusBtn;
		[SerializeField]
		public UnityEngine.UI.Button NextBtn;
		
		private UIFocusesPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			FocusInputFieldTemplete = null;
			Label = null;
			Content = null;
			AddFocusBtn = null;
			NextBtn = null;
			
			mData = null;
		}
		
		public UIFocusesPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIFocusesPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIFocusesPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
