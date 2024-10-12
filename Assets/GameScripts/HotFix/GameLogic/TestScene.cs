using TEngine;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic
{
    public class TestScene : MonoBehaviour
    {
        public Button m_btnBack;
        // Start is called before the first frame update
        void Start()
        {
            m_btnBack.onClick.AddListener(OnClickBack);
        }

        private void OnClickBack()
        {
            GameModule.Scene.LoadScene("Test2");
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

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
