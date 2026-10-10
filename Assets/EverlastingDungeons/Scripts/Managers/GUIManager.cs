using Assets.EverlastingDungeons.Scripts.Gui.Managers;
using Assets.EverlastingDungeons.Scripts.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.EverlastingDungeons.Scripts.Managers
{
    public class GUIManager : BaseManager<GUIManager>
    {
        public LoginGUI login;
        public MainGUI main;
        public IntroductionGUI introduction;
        public Image loadScreen;
        public SelectiveGUI selective;
        private BaseGUI[] guis;

        public override void Init()
        {
            guis = CachedReflection.GetInstanceValues<BaseGUI>(this);
            foreach (var item in guis)
                item.Init();
            CloseAll();
            Open(login);
        }

        public BaseGUI Current => guis.FirstOrDefault(g => g.gameObject.activeSelf);
        public void CloseAll() => guis.ToList().ForEach(g => g.gameObject.SetActive(false));
        public void Open(BaseGUI gui)
        {
            CloseAll();
            gui.gameObject.SetActive(true);
        }
    }
}
