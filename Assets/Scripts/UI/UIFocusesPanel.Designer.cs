using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ProbabilityTest
{
	// Generate Id:a5ae9299-524f-45f5-afd0-f4a1f7ed15b6
	public partial class UIFocusesPanel
	{
		public const string Name = "UIFocusesPanel";
		
		[SerializeField]
		public TMPro.TMP_InputField FocusInputFieldTemplete;
		[SerializeField]
		public TMPro.TextMeshProUGUI Label;
		[SerializeField]
		public RectTransform Content;
		[SerializeField]
		public TMPro.TextMeshProUGUI InfoText;
		[SerializeField]
		public UnityEngine.UI.Button AddFocusBtn;
		[SerializeField]
		public UnityEngine.UI.Button NextBtn;
		
		private UIFocusesPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			FocusInputFieldTemplete = null;
			Label = null;
			Content = null;
			InfoText = null;
			AddFocusBtn = null;
			NextBtn = null;
			
			mData = null;
		}
		
		public UIFocusesPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIFocusesPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIFocusesPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
