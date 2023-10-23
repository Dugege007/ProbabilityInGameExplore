using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ProbabilityTest
{
	// Generate Id:62137519-1be1-4771-9fbb-665b0be3f6cb
	public partial class UIDefaultPanel
	{
		public const string Name = "UIDefaultPanel";
		
		[SerializeField]
		public UnityEngine.UI.Button BackBtn;
		[SerializeField]
		public UnityEngine.UI.Button SettingsBtn;
		
		private UIDefaultPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			BackBtn = null;
			SettingsBtn = null;
			
			mData = null;
		}
		
		public UIDefaultPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIDefaultPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIDefaultPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
