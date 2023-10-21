using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ProbabilityTest
{
	// Generate Id:4e699b7b-d70f-48b2-b709-edf195a4525b
	public partial class UIResultPanel
	{
		public const string Name = "UIResultPanel";
		
		[SerializeField]
		public OptionResultHolderTemplete OptionResultHolderTemplete;
		[SerializeField]
		public FocusScoreHolderTemplete FocusScoreHolderTemplete;
		[SerializeField]
		public RectTransform Content;
		[SerializeField]
		public TMPro.TextMeshProUGUI SubjectText;
		[SerializeField]
		public UnityEngine.UI.Button SaveBtn;
		[SerializeField]
		public UnityEngine.UI.Button BackToHomeBtn;
		
		private UIResultPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			OptionResultHolderTemplete = null;
			FocusScoreHolderTemplete = null;
			Content = null;
			SubjectText = null;
			SaveBtn = null;
			BackToHomeBtn = null;
			
			mData = null;
		}
		
		public UIResultPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIResultPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIResultPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
