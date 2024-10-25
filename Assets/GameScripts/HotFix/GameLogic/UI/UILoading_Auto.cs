using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TEngine;

namespace GameLogic
{
	partial class UILoading
	{
		#region 脚本工具生成的代码
		private Image m_imgBg;
		private Slider m_sliderProcess;
		private Text m_textProcess;
		private Text m_textTips;
		private Image m_imgLogo;
		protected override void ScriptGenerator()
		{
			m_imgBg = FindChildComponent<Image>("m_imgBg");
			m_sliderProcess = FindChildComponent<Slider>("m_sliderProcess");
			m_textProcess = FindChildComponent<Text>("m_sliderProcess/m_textProcess");
			m_textTips = FindChildComponent<Text>("m_sliderProcess/m_textTips");
			m_imgLogo = FindChildComponent<Image>("m_imgLogo");
			//m_sliderProcess.onValueChanged.AddListener(OnSliderProcessChange);
		}
		#endregion
	}
}