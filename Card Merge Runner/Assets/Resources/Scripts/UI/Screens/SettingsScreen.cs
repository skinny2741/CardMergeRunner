using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hyperlab.Core.UI;
using Hyperlab.UI;
using Hyperlab.Managers;
using Hyperlab.Core;
using System;
#if UNITY_IOS
using UnityEngine.iOS;
#endif
namespace Hyperlab.UI
{
    public class SettingsScreen : CoreScreen<SettingsScreen>
    {
        public SoundButtonToggle m_SoundBtn;
        public TapticButtonToggle m_TapticBtn;
        protected override void Awake()
        {
            base.Awake();
            DataManager.Instance.onDataLoad += OnDataLoad;
#if UNITY_IOS
            if ((Device.generation.ToString()).IndexOf("iPad") > -1)
            {

                m_TapticBtn.gameObject.SetActive(false);
            }
#endif
        }

        protected override void Start()
        {
            base.Start();
        }

        private void OnDataLoad(GameData gameData)
        {
            m_SoundBtn.SetOnOff(gameData.m_SettingsData.m_IsSoundON);
            m_TapticBtn.SetOnOff(gameData.m_SettingsData.m_IsHapticON);
        }

        public override void Show()
        {
            base.Show();
        }

    }
}
