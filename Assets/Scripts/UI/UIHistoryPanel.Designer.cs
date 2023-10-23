using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ProbabilityTest
{
	// Generate Id:c19c3792-ad5f-4851-8202-0bfdf98a959c
	public partial class UIHistoryPanel
	{
		public const string Name = "UIHistoryPanel";
		
		[SerializeField]
		public HistoryBtnTemplete HistoryBtnTemplete;
		[SerializeField]
		public RectTransform Content;
		[SerializeField]
		public UnityEngine.UI.Button ClearAllHistoryBtn;
		[SerializeField]
		public UnityEngine.UI.Button NewSubjectBtn;
		[SerializeField]
		public NotificationOpenHistory NotificationOpenHistory;
		[SerializeField]
		public NotificationOpenHistory NotificationClearAllHistory;
		
		private UIHistoryPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			HistoryBtnTemplete = null;
			Content = null;
			ClearAllHistoryBtn = null;
			NewSubjectBtn = null;
			NotificationOpenHistory = null;
			NotificationClearAllHistory = null;
			
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
