using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TEngine;
using YooAsset;

namespace GameLogic
{
	[Window(UILayer.UI, fromResources: false, location: "UILoading")]
	partial class UILoading : UIWindow
	{
		private SceneHandle handle;//场景数据
		
		#region 脚本工具生成的代码
		private Image m_imgBg;
		private Text m_textTips;
		private Text m_textProcess;
		private Slider m_sliderProcess;
		private Image m_imgLogo;
		protected override void ScriptGenerator()
		{
			m_imgBg = FindChildComponent<Image>("m_imgBg");
			m_textTips = FindChildComponent<Text>("m_sliderProcess/m_textTips");
			m_textProcess = FindChildComponent<Text>("m_sliderProcess/m_textProcess");
			m_sliderProcess = FindChildComponent<Slider>("m_sliderProcess");
			m_imgLogo = FindChildComponent<Image>("m_imgLogo");
			//m_sliderProcess.onValueChanged.AddListener(OnSliderProcessChange);
		}
		#endregion


		protected override void OnCreate()
		{
			base.OnCreate();
			handle = userDatas[0] as SceneHandle;
			handle.Completed += Completed;
			m_textTips.text = "this is textTips";
			
		}


		private async void Completed(SceneHandle handle)
		{
			Log.Debug($"SceneName：{handle.SceneName}    process：{handle.Progress}");
			await FightMemoryManager.Instance.LoadAsync();
			OnClose();
		}

		private async void OnClose()
		{
			//2秒后关闭
			//await UniTask.Delay(2000);
			Close();
		}

		protected override void OnUpdate()
		{
			m_sliderProcess.value = handle.Progress;
			m_textProcess.text = $"{handle.Progress*10000f/100}%";
		}
		//OnSliderProcessChange
	}
}