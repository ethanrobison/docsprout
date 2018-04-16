using UnityEngine;

namespace Code.Characters.Player {
	[RequireComponent (typeof (Walk))]
	[RequireComponent (typeof (CameraController))]
	public class Player : MonoBehaviour {
		public enum PlayerState {
			Walking,
			Selecting
		}

		[HideInInspector] public PlayerState state;

		Walk _walkComponent;
		CameraController _camController;


		void Start ()
		{
			_walkComponent = GetComponent<Walk> ();
			_camController = GetComponent<CameraController> ();

			if (Game.Ctx != null) { Game.Ctx.SetPlayer (this); }
		}

		void Update ()
		{
			switch (state) {
			case PlayerState.Walking:
				float x = Game.Sesh.Input.Monitor.LeftH;
				float y = Game.Sesh.Input.Monitor.LeftV;
				if (x * x + y * y < 0.01) {
					_walkComponent.SetDir (Vector3.zero);
					break;
				}
				Vector3 forwards = Vector3.Cross (_camController.camera.transform.right, Vector3.up).normalized;
				Vector2 dir = new Vector2 (x * _camController.camera.transform.right.x + y * forwards.x,
									   x * _camController.camera.transform.right.z + y * forwards.z);
				_walkComponent.SetDir (dir);
				break;
			}

			float camX = Game.Sesh.Input.Monitor.RightH;
			float camY = Game.Sesh.Input.Monitor.RightV;
			if (camX * camX + camY * camY < 0.01) {
				camX = 0f;
				camY = 0f;
			}
			_camController.moveCamera (camX * Time.deltaTime, camY * Time.deltaTime);
		}
	}
}

