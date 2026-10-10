using Assets.EverlastingDungeons.Scripts.Extensions;
using Assets.EverlastingDungeons.Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.UI;

namespace Assets.EverlastingDungeons.Scripts.Gui.Managers.Selective
{
    public class SettingsGUI : BaseSelectiveGUI
    {
        public Slider sfx, music;
        public Button disconnect;

        private void Start()
        {
            disconnect.onClick.AddListener(() => _ = SceneManager.Instance.MoveToScene(SceneType.LoginScene));
            sfx.onValueChanged.AddListener(OnSFXChanged);
            music.onValueChanged.AddListener(OnMusicChanged);
        }

        private void OnMusicChanged(float arg0)
        {
            SettingsManager.Instance.SetVolume(SettingsManager.Instance.music, arg0);
            music.GetComponentInChildren<TextMeshProUGUI>().text = (arg0 * 100d).Format() + "%";
        }

        private void OnSFXChanged(float arg0)
        {
            SettingsManager.Instance.SetVolume(SettingsManager.Instance.sfx, arg0);
            sfx.GetComponentInChildren<TextMeshProUGUI>().text = (arg0 * 100d).Format() + "%";
        }

        public override void Refresh()
        {
            sfx.value = SettingsManager.Instance.GetVolumeAsSlider(SettingsManager.Instance.sfx);
            music.value = SettingsManager.Instance.GetVolumeAsSlider(SettingsManager.Instance.music);
            OnMusicChanged(music.value);
            OnSFXChanged(sfx.value);
        }
    }
}
