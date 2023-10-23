using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ProbabilityTest
{
	// Generate Id:e289d959-9983-4091-8698-52a73f4f3e7e
	public partial class UIHistoryPanel
	{
		public const string Name = "UIHistoryPanel";
		
		[SerializeField]
		public RectTransform Content;
		[SerializeField]
		public UnityEngine.UI.Button NewSubjectBtn;
		[SerializeField]
		public HistoryBtnTemplete HistoryBtnTemplete;
		
		private UIHistoryPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			Content = null;
			NewSubjectBtn = null;
			HistoryBtnTemplete = null;
			
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
