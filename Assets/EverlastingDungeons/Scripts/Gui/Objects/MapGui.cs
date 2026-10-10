using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.EverlastingDungeons.Scripts.Gui.Objects
{
    public class NewMonoBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Button clickBtn, minimapMinusBtn, minimapPlusBtn;

        [SerializeField]
        private GameObject bigMap;

        // Use this for initialization
        public void Start()
        {
            clickBtn.onClick.AddListener(() => bigMap.SetActive(!bigMap.activeSelf));
            minimapMinusBtn.onClick.AddListener(() => Camera.main.transform.GetChild(0).GetComponent<Camera>().orthographicSize *= 2);
            minimapPlusBtn.onClick.AddListener(() => Camera.main.transform.GetChild(0).GetComponent<Camera>().orthographicSize /= 2);
        }
    }
}