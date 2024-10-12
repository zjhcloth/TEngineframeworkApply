using TEngine;

namespace GameLogic
{
    [EventInterface(EEventGroup.GroupUI)]
    public interface IUIMenu
    {
        public void onSettingChange(string data);
    }
}