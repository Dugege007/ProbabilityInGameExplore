using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ProbabilityTest
{
	// Generate Id:27eed060-a544-4116-a6fb-543f19421e6d
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
		public RectTransform NotificationSubjectNull;
		[SerializeField]
		public RectTransform NotificationOptionCount;
		[SerializeField]
		public RectTransform NotificationOptionNull;
		
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
