using TEngine;
using YooAsset;

namespace GameLogic
{
	[Window(UILayer.UI, fromResources: false, location: "UILoading")]
	partial class UILoading : UIWindow
	{
		private SceneHandle handle;//场景数据
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
			//5秒后关闭
			//await UniTask.Delay(5000);
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