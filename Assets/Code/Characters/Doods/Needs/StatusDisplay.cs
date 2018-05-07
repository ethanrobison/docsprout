using System;
using UnityEngine;

namespace Code.Characters.Doods.Needs
{
    public class StatusDisplay
    {
        private readonly GameObject _water, _shade;

        public StatusDisplay (GameObject go) {
            _water = go.transform.Find("Canvas/Icons/Water").gameObject;
            _shade = go.transform.Find("Canvas/Icons/Shade").gameObject;
        }

        public void SetIconOfType (NeedType need, int status) {
            switch (need) {
                case NeedType.Water:
                    _water.SetActive(status < 0);
                    break;
                case NeedType.Sun:
                    _shade.SetActive(status < 0);
                    break;
                case NeedType.Fun:
                    break;
                //default:
                    //throw new ArgumentOutOfRangeException("need", need, null);
            }
        }
    }
}