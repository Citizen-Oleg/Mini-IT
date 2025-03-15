using SimpleEventBus;
using SimpleEventBus.Interfaces;

namespace Tools.SimpleEventBus
{
    public static class EventStreams
    {
        public static IEventBus UserInterface
        {
            get
            {
                if (_userInterface == null)
                {
                    _userInterface = new EventBus();
                }

                return _userInterface;
            }
        }

        private static IEventBus _userInterface;
    }
}