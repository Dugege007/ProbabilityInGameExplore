using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ProbabilityTest
{
	// Generate Id:61024111-8766-466d-9261-bcd4a287084f
	public partial class UIRatingPanel
	{
		public const string Name = "UIRatingPanel";
		
		[SerializeField]
		public RatingHolderTemplete RatingHolderTemplete;
		[SerializeField]
		public RectTransform Content;
		[SerializeField]
		public TMPro.TextMeshProUGUI SubjectText;
		[SerializeField]
		public TMPro.TextMeshProUGUI OptionText;
		[SerializeField]
		public UnityEngine.UI.Button LastBtn;
		[SerializeField]
		public UnityEngine.UI.Button NextBtn;
		[SerializeField]
		public UnityEngine.UI.Button ComputeBtn;
		
		private UIRatingPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			RatingHolderTemplete = null;
			Content = null;
			SubjectText = null;
			OptionText = null;
			LastBtn = null;
			NextBtn = null;
			ComputeBtn = null;
			
			mData = null;
		}
		
		public UIRatingPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIRatingPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIRatingPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
