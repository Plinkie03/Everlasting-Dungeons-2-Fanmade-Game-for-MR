using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.Gui.Objects
{
    /// <summary>
    /// Texto flotante en la GUI
    /// </summary>
    public class GUIFloatingText : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI mesh;

        /// <summary>
        /// Establece el texto
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public GUIFloatingText SetText(string text)
        {
            mesh.text = text;
            return this;
        }

        /// <summary>
        /// Establece el color
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public GUIFloatingText SetColor(Color color)
        {
            mesh.color = color;
            return this;
        }

        /// <summary>
        /// Escala el texto
        /// </summary>
        /// <param name="scale"></param>
        /// <param name="duration"></param>
        /// <param name="loopBack"></param>
        /// <returns></returns>
        public GUIFloatingText SetScale(float scale, float duration, bool loopBack = false)
        {
            transform.LeanScale(Vector3.one * scale, duration)
                .setOnComplete(() =>
                {
                    if (loopBack)
                        SetScale(Vector3.one.y, duration);
                });
            return this;
        }

        /// <summary>
        /// Desvanece el texto con una duracion a un valor alpha
        /// </summary>
        /// <param name="to"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public GUIFloatingText SetFade(float to, float duration)
        {
            LeanTween.value(mesh.color.a, to, duration)
                .setOnUpdate(value =>
                {
                    Color color = mesh.color;
                    color.a = value;
                    mesh.color = color;
                });
            return this;
        }

        /// <summary>
        /// Hace flotar el texto una cierta distancia en un tiempo determinado
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public GUIFloatingText SetFloat(float offset, float duration)
        {
            transform.LeanMoveY(transform.position.y + offset, duration)
                .setOnComplete(() => Destroy(gameObject));
            return this;
        }
    }
}