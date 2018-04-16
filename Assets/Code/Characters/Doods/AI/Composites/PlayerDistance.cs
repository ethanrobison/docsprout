using Code.Characters.Player;
using UnityEngine;

namespace Code.Doods.AI {
	public class PlayerDistance : FilterSequence {
		readonly float _minThresh;
		readonly float _maxThresh;
		readonly Player _player;

		public PlayerDistance (Dood dood, float minimum, float maximum) : base (dood)
		{
			var ctx = Game.Ctx;
			_player = ctx.Player;
			_minThresh = minimum;
			_maxThresh = maximum;
		}

		protected override bool Precondition ()
		{
			var dist = Vector3.Distance (_dood.transform.position, _player.transform.position);
			return _minThresh < dist && dist < _maxThresh;
		}
	}
}