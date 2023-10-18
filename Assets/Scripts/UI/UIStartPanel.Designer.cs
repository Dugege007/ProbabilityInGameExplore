using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ProbabilityTest
{
	// Generate Id:50bd6cf6-d4d2-417e-8c2f-df23171429e7
	public partial class UIStartPanel
	{
		public const string Name = "UIStartPanel";
		
		[SerializeField]
		public TMPro.TMP_InputField OptionInputFieldTemplete;
		[SerializeField]
		public RectTransform Content;
		[SerializeField]
		public TMPro.TMP_InputField SubjectInputField;
		[SerializeField]
		public UnityEngine.UI.Button AddOptionBtn;
		[SerializeField]
		public UnityEngine.UI.Button NextBtn;
		
		private UIStartPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			OptionInputFieldTemplete = null;
			Content = null;
			SubjectInputField = null;
			AddOptionBtn = null;
			NextBtn = null;
			
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
