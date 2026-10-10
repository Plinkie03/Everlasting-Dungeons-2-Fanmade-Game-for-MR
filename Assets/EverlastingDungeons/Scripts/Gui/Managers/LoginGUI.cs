using Assets.EverlastingDungeons.Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.EverlastingDungeons.Scripts.Gui.Managers
{
    public class LoginGUI : BaseGUI
    {
        public TextMeshProUGUI info;
        public Button login;
        public TMP_InputField username;
        public TMP_InputField password;
        public Toggle rememberMe;

        public override void Init()
        {
            login.onClick.AddListener(OnLogin);

            if (Application.isMobilePlatform)
            {
                username.text = "user";
                password.text = "user";
                username.interactable = false;
                password.interactable = false;
            }
            else
            {

                if (PlayerPrefs.GetInt(nameof(RememberMe)) == 1)
                {
                    username.text = PlayerPrefs.GetString(nameof(username));
                    password.text = PlayerPrefs.GetString(nameof(password));
                    rememberMe.isOn = true;
                }
            }
        }

        private bool RememberMe => rememberMe.isOn;

        public async void OnLogin()
        {
            if (username.text.Length == 0 || password.text.Length == 0)
            {
                DisplayText("Please fill in all fields.", Color.red);
                return;
            }

            login.interactable = false;
            DisplayText("Logging in...", Color.white);
            if (AuthenticationManager.Instance.Authenticate(username.text, password.text))
            {
                DisplayText("Login successful!", Color.green);
                if (RememberMe)
                {
                    PlayerPrefs.SetInt(nameof(RememberMe), 1);
                    PlayerPrefs.SetString(nameof(username), username.text);
                    PlayerPrefs.SetString(nameof(password), password.text);
                }
                else
                {
                    PlayerPrefs.SetInt(nameof(RememberMe), 0);
                    PlayerPrefs.DeleteKey(nameof(username));
                    PlayerPrefs.DeleteKey(nameof(password));
                }
                await SceneManager.Instance.MoveToScene(SceneType.TownScene);
            }
            else
            {
                DisplayText("Invalid username or password.", Color.red);
                login.interactable = true;
            }
        }

        public void DisplayText(string msg, Color color)
        {
            info.gameObject.SetActive(true);
            info.text = msg;
            info.color = color;
        }
    }
}
