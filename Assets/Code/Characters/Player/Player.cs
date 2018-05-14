using Code.Characters.Doods;
using Code.Utils;
using UnityEngine;

namespace Code.Characters.Player
{
    [RequireComponent(typeof(CameraController))]
    public class Player : MonoBehaviour
    {
        private CameraController _camController;
        private Movement _movement;


        private void Start () {
            _camController = gameObject.GetRequiredComponent<CameraController>();
            _movement = gameObject.GetRequiredComponent<Movement>();

            if (Game.Ctx != null) { Game.Ctx.SetPlayer(this); }
        }

        private void Update () {
            var x = Game.Sesh.Input.Monitor.LeftH;
            var y = Game.Sesh.Input.Monitor.LeftV;
            if (x * x + y * y < 0.01) { _movement.SetDirection(Vector3.zero); }

            var forwards = Vector3.Cross(_camController.Camera.transform.right, Vector3.up).normalized;
            var dir = new Vector2(x * _camController.Camera.transform.right.x + y * forwards.x,
                x * _camController.Camera.transform.right.z + y * forwards.z);
            _movement.SetDirection(dir);

            var camX = Game.Sesh.Input.Monitor.RightH;
            var camY = Game.Sesh.Input.Monitor.RightV;
            if (camX * camX + camY * camY > 0.01) {
                _camController.MoveCamera(camX * Time.deltaTime, camY * Time.deltaTime);
            }
        }
    }
}