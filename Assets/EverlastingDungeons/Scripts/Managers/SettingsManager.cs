using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets.EverlastingDungeons.Scripts.Managers
{
    public class SettingsManager : BaseManager<SettingsManager>
    {
        public int fps = 30;
        public LayerMask entityLayer;
        public AudioMixerGroup sfx, music;

        public override void Init()
        {
            SetVolume(sfx, PlayerPrefs.GetFloat("SFX", 0f));
            SetVolume(music, PlayerPrefs.GetFloat("Background", 0f));
            LeanTween.init(3000);
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = fps;
        }

        private void OnApplicationFocus(bool focus)
        {
            if (!focus)
            {
                if (AuthenticationManager.Instance.data != null)
                {
                    DatabaseManager.Instance.UpdatePlayerData(AuthenticationManager.Instance.data);
                    Debug.Log($"Saved player");
                }
            }
        }

        public float GetVolumeAsSlider(AudioMixerGroup group)
        {
            group.audioMixer.GetFloat(group.name, out var val);
            return GetVolumeAsSlider(val);
        }

        public float GetVolumeAsSlider(float decibels)
        {
            // Clamp to Unity mixer range
            decibels = Mathf.Clamp(decibels, -80f, 0f);

            // Convert dB back to linear slider value
            return Mathf.Pow(10f, decibels / 20f);
        }

        public void SetVolume(AudioMixerGroup group, float sliderValue)
        {
            // Clamp slider between 0 and 1
            sliderValue = Mathf.Clamp01(sliderValue);

            // Convert linear slider to decibels
            float dB = (sliderValue <= 0.0001f)
                ? -80f
                : Mathf.Log10(sliderValue) * 20f;

            PlayerPrefs.SetFloat(group.name, sliderValue);

            // Set exposed parameter using the group's name
            group.audioMixer.SetFloat(group.name, dB);
        }
    }
}
