using Code.Session;
using UnityEngine;

namespace Code.Characters.Player
{
	public class TreatGiver : MonoBehaviour
	{

		private Object _treat;

		void Start () {
			_treat = Resources.Load("Environment/Donut");
			Game.Sesh.Input.Monitor.RegisterMapping(ControllerButton.RightBumper, GiveTreat);
		}

		private void GiveTreat () {
			Vector3 treatPos = transform.position + transform.forward * 2f + transform.up;
			Quaternion treatRot = Quaternion.Euler(-60f, 0f, 0f);
			Object.Instantiate(_treat, treatPos, treatRot);
		}
	}
}
