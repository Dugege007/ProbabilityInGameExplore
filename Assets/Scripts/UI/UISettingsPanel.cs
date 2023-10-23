using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ProbabilityTest
{
	public class UISettingsPanelData : UIPanelData
	{
	}
	public partial class UISettingsPanel : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UISettingsPanelData ?? new UISettingsPanelData();
			// please add init code here
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
		}
		
		protected override void OnShow()
		{
		}
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
		}
	}
}
