using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ProbabilityTest
{
	// Generate Id:b458f685-9309-46fa-8bd7-d32194bdfbca
	public partial class UIFocusesPanel
	{
		public const string Name = "UIFocusesPanel";
		
		[SerializeField]
		public FocusHolderTemplete FocusHolderTemplete;
		[SerializeField]
		public TMPro.TextMeshProUGUI OptionTextTemplete;
		[SerializeField]
		public RectTransform Content;
		[SerializeField]
		public TMPro.TextMeshProUGUI SubjectText;
		[SerializeField]
		public RectTransform OptiontTextHolder;
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
			OptionTextTemplete = null;
			Content = null;
			SubjectText = null;
			OptiontTextHolder = null;
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
