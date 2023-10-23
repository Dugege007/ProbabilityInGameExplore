using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ProbabilityTest
{
	// Generate Id:06313fd8-2975-4065-9211-9b4280e3086e
	public partial class UISettingsPanel
	{
		public const string Name = "UISettingsPanel";
		
		
		private UISettingsPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			
			mData = null;
		}
		
		public UISettingsPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UISettingsPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UISettingsPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
