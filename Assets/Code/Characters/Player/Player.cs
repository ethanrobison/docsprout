using UnityEngine;

namespace Code.Characters.Player {
	[RequireComponent (typeof (Walk))]
	[RequireComponent (typeof (CameraController))]
	public class Player : MonoBehaviour {

		public struct Buttons {
			public string moveX;
			public string moveY;
			public string camX;
			public string camY;
		}
		/// <summary>
		/// Stores the names of all of the input axes to use
		/// </summary>
		[HideInInspector] public Buttons buttons;

		public enum PlayerState {
			walking,
			selecting
		}

		[HideInInspector] public PlayerState state;

		Walk walkComponent;
		CameraController camController;


		void Start ()
		{
			walkComponent = GetComponent<Walk> ();
			camController = GetComponent<CameraController> ();

			// Initialize input axes
			buttons.moveX = "leftHorizontal";
			buttons.moveY = "leftVertical";
			buttons.camX = "rightHorizontal";
			buttons.camY = "rightVertical";
		}

		void Update ()
		{
			switch (state) {
			case PlayerState.walking:
				float x = Input.GetAxisRaw (buttons.moveX + Game.Sesh.Input.GetButtonSuffix ());
				float y = Input.GetAxisRaw (buttons.moveY + Game.Sesh.Input.GetButtonSuffix ());
				if (x * x + y * y < 0.01) {
					walkComponent.SetDir (Vector3.zero);
					break;
				}
				Vector3 forwards = Vector3.Cross (camController.camera.transform.right, Vector3.up).normalized;
				Vector2 dir = new Vector2 (x * camController.camera.transform.right.x + y * forwards.x,
									   x * camController.camera.transform.right.z + y * forwards.z);
				walkComponent.SetDir (dir);
				break;
			}

			float camX = Input.GetAxisRaw (buttons.camX + Game.Sesh.Input.GetButtonSuffix ());
			float camY = Input.GetAxisRaw (buttons.camY + Game.Sesh.Input.GetButtonSuffix ());
			if (camX * camX + camY * camY < 0.01) {
				camX = 0f;
				camY = 0f;
			}
			camController.moveCamera (camX * Time.deltaTime, camY * Time.deltaTime);
		}
	}
}

