using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ProbabilityTest
{
	// Generate Id:e1dd3f5a-d3be-4907-a345-8c7f71eaac4e
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
		public TMPro.TMP_InputField DescriptionInputField;
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
			DescriptionInputField = null;
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
