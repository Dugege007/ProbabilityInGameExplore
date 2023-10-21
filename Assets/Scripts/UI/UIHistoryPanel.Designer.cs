using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ProbabilityTest
{
	// Generate Id:0da3efb8-d92b-4adb-8b89-e928e58550a9
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
