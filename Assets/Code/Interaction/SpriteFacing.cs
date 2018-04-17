using UnityEngine;

namespace Code.Interaction
{
    /// <summary>
    /// Make sure the sprite is facing the camera.
    /// </summary>
    public class SpriteFacing : MonoBehaviour
    {
        private void Update () {
            transform.rotation = Quaternion.LookRotation(-Camera.main.transform.forward, Camera.main.transform.up);
        }
    }
}