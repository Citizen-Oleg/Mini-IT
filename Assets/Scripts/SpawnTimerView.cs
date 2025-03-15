using System;
using UnityEngine.UI;
using Zenject;

namespace Tools
{
    public class SpawnTimerView : ITickable
    {
        private readonly Text _textTimer;
        private readonly Spawner _spawner;
        
        public SpawnTimerView(Settings settings, Spawner spawner)
        {
            _textTimer = settings.Timer;
            _spawner = spawner;
        }
        
        public void Tick()
        {
            _textTimer.text = _spawner.CurrentTime.ToString("F1");
        }

        [Serializable]
        public class Settings
        {
            public Text Timer;
        }
    }
}