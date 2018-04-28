using UnityEngine;

namespace Code.Characters.Player.Interaction
{
    /// <summary>
    /// Make sure the sprite is facing the camera.
    /// </summary>
    public class SpriteFacing : MonoBehaviour
    {
        public bool Reverse;

        private void Update () {
            transform.rotation = Quaternion.LookRotation(
                (Reverse ? -1 : 1) * Camera.main.transform.forward,
                Camera.main.transform.up
            );
        }
    }
}