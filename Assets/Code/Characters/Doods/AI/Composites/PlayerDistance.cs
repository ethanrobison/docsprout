using Code.Session;
using Code.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Characters.Doods.AI
{
    public class PlayerDistance : FilterSelector
    {
        private readonly float _minThresh, _maxThresh;

        public PlayerDistance (Dood dood, float minimum, float maximum) : base(dood) {
            _minThresh = minimum;
            _maxThresh = maximum;
        }

        protected override bool Precondition () {
            if (Game.Ctx.Player == null) {
                return false;
            }

            var dist = Vector3.Distance(Dood.transform.position, Game.Ctx.Player.transform.position);
            return _minThresh < dist && dist < _maxThresh;
        }
    }
}