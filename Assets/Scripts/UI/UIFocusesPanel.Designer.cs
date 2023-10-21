using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ProbabilityTest
{
	// Generate Id:91895209-5029-40ed-b747-0f34d2907260
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
		[SerializeField]
		public RainbowArt.CleanFlatUI.Notification NotificationFocusNull;
		[SerializeField]
		public RainbowArt.CleanFlatUI.Notification NotificationFocusCount;
		[SerializeField]
		public RainbowArt.CleanFlatUI.Notification NotificationFocusSame;
		
		private UIFocusesPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			FocusHolderTemplete = null;
			Content = null;
			InfoText = null;
			AddFocusBtn = null;
			NextBtn = null;
			NotificationFocusNull = null;
			NotificationFocusCount = null;
			NotificationFocusSame = null;
			
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
