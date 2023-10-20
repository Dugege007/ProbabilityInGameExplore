using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ProbabilityTest
{
	// Generate Id:5d2f7807-1b73-47c2-adf0-0893ebd53677
	public partial class UIFocusesPanel
	{
		public const string Name = "UIFocusesPanel";
		
		[SerializeField]
		public FocusHolderTemplete FocusHolderTemplete;
		[SerializeField]
		public RectTransform Content;
		[SerializeField]
		public TMPro.TextMeshProUGUI InfoText;
		[SerializeField]
		public UnityEngine.UI.Button AddFocusBtn;
		[SerializeField]
		public UnityEngine.UI.Button NextBtn;
		
		private UIFocusesPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			FocusHolderTemplete = null;
			Content = null;
			InfoText = null;
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
