using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ProbabilityTest
{
	// Generate Id:504017f6-d867-4bac-8db2-5d7189bf32ae
	public partial class UIStartPanel
	{
		public const string Name = "UIStartPanel";
		
		[SerializeField]
		public OptionInputFieldHolderTemplete OptionInputFieldHolderTemplete;
		[SerializeField]
		public RectTransform Content;
		[SerializeField]
		public TMPro.TMP_InputField SubjectInputField;
		[SerializeField]
		public UnityEngine.UI.Button AddOptionBtn;
		[SerializeField]
		public UnityEngine.UI.Button NextBtn;
		[SerializeField]
		public RainbowArt.CleanFlatUI.Notification NotificationSubjectNull;
		[SerializeField]
		public RainbowArt.CleanFlatUI.Notification NotificationOptionCount;
		[SerializeField]
		public RainbowArt.CleanFlatUI.Notification NotificationOptionNull;
		[SerializeField]
		public RainbowArt.CleanFlatUI.Notification NotificationOptionSame;
		
		private UIStartPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			OptionInputFieldHolderTemplete = null;
			Content = null;
			SubjectInputField = null;
			AddOptionBtn = null;
			NextBtn = null;
			NotificationSubjectNull = null;
			NotificationOptionCount = null;
			NotificationOptionNull = null;
			NotificationOptionSame = null;
			
			mData = null;
		}
		
		public UIStartPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIStartPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIStartPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
