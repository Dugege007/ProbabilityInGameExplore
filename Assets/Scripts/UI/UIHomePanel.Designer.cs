using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ProbabilityTest
{
	// Generate Id:e22638ac-8d53-490e-9ac3-e0347b9442f7
	public partial class UIHomePanel
	{
		public const string Name = "UIHomePanel";
		
		[SerializeField]
		public UnityEngine.UI.Button NewSubjectBtn;
		[SerializeField]
		public UnityEngine.UI.Button ViewHistoryBtn;
		[SerializeField]
		public UnityEngine.UI.Button ClearHistoryBtn;
		
		private UIHomePanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			NewSubjectBtn = null;
			ViewHistoryBtn = null;
			ClearHistoryBtn = null;
			
			mData = null;
		}
		
		public UIHomePanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIHomePanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIHomePanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
