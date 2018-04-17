using Code.Doods.AI;
using UnityEngine;

namespace Code.Characters.Doods.AI
{
    public class PlayerDistance : FilterSequence
    {
        private readonly float _minThresh;
        private readonly float _maxThresh;
        private readonly Player.Player _player;

        public PlayerDistance (Dood dood, float minimum, float maximum) : base(dood) {
            var ctx = Game.Ctx;
            _player = ctx.Player;
            _minThresh = minimum;
            _maxThresh = maximum;
        }

        protected override bool Precondition () {
            var dist = Vector3.Distance(Dood.transform.position, _player.transform.position);
            return _minThresh < dist && dist < _maxThresh;
        }
    }
}