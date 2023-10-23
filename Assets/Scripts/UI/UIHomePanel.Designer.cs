using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ProbabilityTest
{
	// Generate Id:50779785-f58e-4e58-9cca-4d469910f2f6
	public partial class UIHomePanel
	{
		public const string Name = "UIHomePanel";
		
		[SerializeField]
		public UnityEngine.UI.Button NewSubjectBtn;
		[SerializeField]
		public UnityEngine.UI.Button ContinueEditBtn;
		[SerializeField]
		public UnityEngine.UI.Button ViewHistoryBtn;
		[SerializeField]
		public UnityEngine.UI.Button SettingsBtn;
		[SerializeField]
		public NotificationNewSubject NotificationNewSubject;
		
		private UIHomePanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			NewSubjectBtn = null;
			ContinueEditBtn = null;
			ViewHistoryBtn = null;
			SettingsBtn = null;
			NotificationNewSubject = null;
			
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
