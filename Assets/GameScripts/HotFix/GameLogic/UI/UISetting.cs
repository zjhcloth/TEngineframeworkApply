using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TEngine;

namespace GameLogic
{
	partial class UISetting : UIWindow
	{
		#region 脚本工具生成的代码
		private Button m_btnCancel;
		private Text m_textCancel;
		private Button m_btnConfirm;
		private Text m_textConfirm;
		private Toggle m_togChinese;
		private Text m_textChinese;
		private Toggle m_togEngLish;
		private Text m_textEnglish;
		private Text m_textMusic;
		private Toggle m_togMusic;
		private Slider m_sliderMusic;
		private Text m_textSoundEffect;
		private Toggle m_togSoundEffect;
		private Slider m_sliderSoundEffect;
		private Text m_textUISoundEffect;
		private Toggle m_togUISoundEffect;
		private Slider m_sliderUISoundEffect;
		protected override void ScriptGenerator()
		{
			m_btnCancel = FindChildComponent<Button>("m_btnCancel");
			m_textCancel = FindChildComponent<Text>("m_btnCancel/m_textCancel");
			m_btnConfirm = FindChildComponent<Button>("m_btnConfirm");
			m_textConfirm = FindChildComponent<Text>("m_btnConfirm/m_textConfirm");
			m_togChinese = FindChildComponent<Toggle>("ToggleGroup/m_togChinese");
			m_textChinese = FindChildComponent<Text>("ToggleGroup/m_togChinese/m_textChinese");
			m_togEngLish = FindChildComponent<Toggle>("ToggleGroup/m_togEngLish");
			m_textEnglish = FindChildComponent<Text>("ToggleGroup/m_togEngLish/m_textEnglish");
			m_textMusic = FindChildComponent<Text>("Music/m_textMusic");
			m_togMusic = FindChildComponent<Toggle>("Music/m_togMusic");
			m_sliderMusic = FindChildComponent<Slider>("Music/m_sliderMusic");
			m_textSoundEffect = FindChildComponent<Text>("SoundEffect/m_textSoundEffect");
			m_togSoundEffect = FindChildComponent<Toggle>("SoundEffect/m_togSoundEffect");
			m_sliderSoundEffect = FindChildComponent<Slider>("SoundEffect/m_sliderSoundEffect");
			m_textUISoundEffect = FindChildComponent<Text>("UISoundEffect/m_textUISoundEffect");
			m_togUISoundEffect = FindChildComponent<Toggle>("UISoundEffect/m_togUISoundEffect");
			m_sliderUISoundEffect = FindChildComponent<Slider>("UISoundEffect/m_sliderUISoundEffect");
			m_btnCancel.onClick.AddListener(OnClickCancelBtn);
			m_btnConfirm.onClick.AddListener(OnClickConfirmBtn);
			m_togChinese.onValueChanged.AddListener(OnTogglem_togChineseChange);
			m_togEngLish.onValueChanged.AddListener(OnTogglem_togEngLishChange);
			m_togMusic.onValueChanged.AddListener(OnTogglem_togMusicChange);
			m_sliderMusic.onValueChanged.AddListener(OnSliderMusicChange);
			m_togSoundEffect.onValueChanged.AddListener(OnTogglem_togSoundEffectChange);
			m_sliderSoundEffect.onValueChanged.AddListener(OnSliderSoundEffectChange);
			m_togUISoundEffect.onValueChanged.AddListener(OnTogglem_togUISoundEffectChange);
			m_sliderUISoundEffect.onValueChanged.AddListener(OnSliderUISoundEffectChange);

			//设置默认值
			m_togChinese.isOn = Language.ChineseSimplified == GameModule.Localization.Language;
			m_togEngLish.isOn = Language.English == GameModule.Localization.Language;
		}
		#endregion

		private void OnClickCancelBtn()
		{
			m_togEngLish.isOn = Language.English == GameModule.Localization.Language;
			m_togChinese.isOn = Language.ChineseSimplified == GameModule.Localization.Language;
		}
		private void OnClickConfirmBtn()
		{
			GameModule.UI.ShowUIAsync<UIMenu>();
			Close();
		}
		private void OnTogglem_togChineseChange(bool value)
		{
			Log.Debug($"OnTogglem_togChineseChange{value}");
			if (Language.English == GameModule.Localization.Language)
			{
				GameModule.Localization.SetLanguage(Language.ChineseSimplified);
			}
		}
		private void OnTogglem_togEngLishChange(bool value)
		{
			Log.Debug($"OnTogglem_togEngLishChange{value}");
			if (Language.ChineseSimplified == GameModule.Localization.Language)
			{
				GameModule.Localization.SetLanguage(Language.English);
			}
		}
		
		private void OnTogglem_togMusicChange(bool value)
		{
			Log.Debug($"OnTogglem_togMusicChange{value}");
			m_sliderMusic.gameObject.SetActive(value);
		}
		private void OnSliderMusicChange(float value)
		{
			Log.Debug($"OnSliderMusicChange{value}");
		}
		
		private void OnTogglem_togSoundEffectChange(bool value)
		{
			Log.Debug($"OnTogglem_togSoundEffectChange{value}");
			m_sliderSoundEffect.gameObject.SetActive(value);
		}
		private void OnSliderSoundEffectChange(float value)
		{
			Log.Debug($"OnSliderSoundEffectChange{value}");
		}

		private void OnTogglem_togUISoundEffectChange(bool value)
		{
			Log.Debug($"OnTogglem_togUISoundEffectChange{value}");
			m_sliderUISoundEffect.gameObject.SetActive(value);
		}
		private void OnSliderUISoundEffectChange(float value)
		{
			Log.Debug($"OnSliderUISoundEffectChange{value}");
		}



	}
}