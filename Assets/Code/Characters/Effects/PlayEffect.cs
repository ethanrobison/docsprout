using System;
using System.Collections;
using UnityEngine;

namespace Code.Characters.Effects
{
    public class PlayEffect
    {
        private readonly Action _before;
        private readonly Action _after;
        private readonly float _waitTime;

        public PlayEffect (Action before, Action after, float waitTime) {
            _before = before;
            _after = after;
            _waitTime = waitTime;
        }

        public void Play () { RunEffect(); }

        private IEnumerator RunEffect () {
            if (_before != null) { _before(); }

            yield return new WaitForSeconds(_waitTime);
            if (_after != null) { _after(); }
        }
    }
}