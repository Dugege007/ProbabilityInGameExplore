using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ProbabilityTest
{
	// Generate Id:fda3c641-1fbf-4beb-82f2-891fd84fb27a
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
		public UnityEngine.UI.Button BackToHomeBtn;
		[SerializeField]
		public UnityEngine.UI.Button ShareBtn;
		
		private UIResultPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			OptionResultHolderTemplete = null;
			FocusScoreHolderTemplete = null;
			Content = null;
			SubjectText = null;
			BackToHomeBtn = null;
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
