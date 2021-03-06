using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hyperlab.Core.UI;
using Hyperlab.Managers;
namespace Hyperlab.UI
{
    public class SoundButtonToggle : CoreButtonToggle
    {
        protected override void Start()
        {
            base.Start();
            ToggleImage();
        }
        public override void OnClickBtn()
        {
            base.OnClickBtn();
            SettingsManager.Instance.SetOnOffSound(m_IsOn, true);
        }
        public override void ToggleOn()
        {
            base.ToggleOn();
            m_OffImage.gameObject.SetActive(false);
        }
        public override void ToggleOff()
        {
            base.ToggleOff();
            m_OffImage.gameObject.SetActive(true);
        }
    }
}
