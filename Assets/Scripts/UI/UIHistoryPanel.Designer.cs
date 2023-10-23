using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ProbabilityTest
{
	// Generate Id:7f49f726-c3ff-4550-81d0-145da63d8c16
	public partial class UIHistoryPanel
	{
		public const string Name = "UIHistoryPanel";
		
		[SerializeField]
		public RectTransform Content;
		[SerializeField]
		public UnityEngine.UI.Button NewSubjectBtn;
		[SerializeField]
		public HistoryBtnTemplete HistoryBtnTemplete;
		[SerializeField]
		public NotificationOpenHistory NotificationOpenHistory;
		
		private UIHistoryPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			Content = null;
			NewSubjectBtn = null;
			HistoryBtnTemplete = null;
			NotificationOpenHistory = null;
			
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
