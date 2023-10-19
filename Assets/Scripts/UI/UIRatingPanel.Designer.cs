using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ProbabilityTest
{
	// Generate Id:d057bd89-b54a-4188-9475-85f9e45a361a
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
		public OptionRatingHolder OptionRatingHolder;
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
			OptionRatingHolder = null;
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
