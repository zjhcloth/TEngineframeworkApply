using UnityEngine;
using UnityEngine.UI;
using TEngine;
using AudioType = TEngine.AudioType;

namespace GameLogic
{
	[Window(UILayer.UI, fromResources: false, location: "UIMenu")]
	class UIMenu : UIWindow
	{
		#region 脚本工具生成的代码
		private Button m_btnStartGame;
		private Text m_textStartGame;
		private Button m_btnSetting;
		private Text m_textSetting;
		
		private Button m_btnChangeLanguage;
		private Text m_textChangeLanguage;
		protected override void ScriptGenerator()
		{
			m_btnStartGame = FindChildComponent<Button>("m_btnStartGame");
			m_textStartGame = FindChildComponent<Text>("m_btnStartGame/m_textStartGame");
			m_btnSetting = FindChildComponent<Button>("m_btnSetting");
			m_textSetting = FindChildComponent<Text>("m_btnSetting/m_textSetting");
			m_btnChangeLanguage = FindChildComponent<Button>("m_btnChangeLanguage");
			m_textChangeLanguage = FindChildComponent<Text>("m_btnChangeLanguage/m_textChangeLanguage");
			m_btnStartGame.onClick.AddListener(OnClickStartGameBtn);
			m_btnSetting.onClick.AddListener(OnClickSettingBtn);
			m_btnChangeLanguage.onClick.AddListener(OnClickChangeLanguageBtn);
			
			
		}
		#endregion
		
		protected override void OnCreate()
		{
			//添加事件监听测试
			AddUIEvent<string>(IUIMenu_Event.onSettingChange, OnSettingChange);

		}

		private void OnSettingChange(string value)
		{
			Debug.Log("-----------------"+value);
			//播放背景音乐测试
			//GameModule.Audio.Stop(AudioType.Sound, true);
			//GameModule.Audio.Play(AudioType.Sound, "bgm");
			GameModule.Audio.Play(AudioType.UISound, "Explosion", bAsync: true);
		}
		
		
		private void OnClickChangeLanguageBtn()
		{
			Language currLanguage = GameModule.Localization.Language;
			Debug.Log(GameModule.Localization.Language);
			if (Language.English == currLanguage)
			{
				GameModule.Localization.SetLanguage(Language.ChineseSimplified);
			}
			else
			{
				GameModule.Localization.SetLanguage(Language.English);
			}
		}
		

		private void OnClickStartGameBtn()
		{
			m_textStartGame.text = "开始游戏";
			//事件发送测试
			GameEvent.Get<IUIMenu>().onSettingChange("开始游戏");
			GameModule.Scene.LoadScene("Test");
			Close();
		}
		private void OnClickSettingBtn()
		{
			m_textSetting.text = "设置";
			GameModule.UI.ShowUIAsync<UISetting>();
			Close();
		}
	}
}