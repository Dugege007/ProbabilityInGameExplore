using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ProbabilityTest
{
	// Generate Id:a8a88280-3aa2-40d5-80b5-fc84658fa34f
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
		public UnityEngine.UI.Button ShareBtn;
		
		private UIResultPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			OptionResultHolderTemplete = null;
			FocusScoreHolderTemplete = null;
			Content = null;
			SubjectText = null;
			SaveBtn = null;
			ShareBtn = null;
			
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
