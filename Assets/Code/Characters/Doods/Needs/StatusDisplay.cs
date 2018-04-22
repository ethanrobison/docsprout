using System;
using UnityEngine;

namespace Code.Characters.Doods.Needs
{
    public class StatusDisplay
    {
        private readonly GameObject _needMore;
        private readonly GameObject _needLess;

        public StatusDisplay (GameObject go) {
            _needMore = go.transform.Find("NeedWater").gameObject;
            _needLess = go.transform.Find("StopWater").gameObject;
        }

        public void Show (int status) {
            // lol this could be a bit manipulation // but m'clarity
            switch (status) {
                case 0:
                    SetDisplays(false, false);
                    break;
                case 1:
                    SetDisplays(false, true);
                    break;
                case -1:
                    SetDisplays(true, false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Hide () { SetDisplays(false, false); }

        private void SetDisplays (bool under, bool over) {
            _needMore.SetActive(under);
            _needLess.SetActive(over);
        }
    }
}