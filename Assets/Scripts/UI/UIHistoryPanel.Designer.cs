using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ProbabilityTest
{
	// Generate Id:864ea1e4-696d-4a82-9b8c-8ad096e84b6b
	public partial class UIHistoryPanel
	{
		public const string Name = "UIHistoryPanel";
		
		[SerializeField]
		public HistoryBtnTemplete HistoryBtnTemplete;
		[SerializeField]
		public RectTransform Content;
		[SerializeField]
		public UnityEngine.UI.Button NewSubjectBtn;
		
		private UIHistoryPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			HistoryBtnTemplete = null;
			Content = null;
			NewSubjectBtn = null;
			
			mData = null;
		}
		
		public UIHistoryPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIHistoryPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIHistoryPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
