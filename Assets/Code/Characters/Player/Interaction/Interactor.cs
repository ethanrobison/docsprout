using UnityEngine;

namespace Code.Interaction
{
    /// <inheritdoc />
    /// <summary>
    ///  The interactor that approaches or departs IApproachable's.
    /// </summary>
    public class Interactor : MonoBehaviour
    {
        private void OnTriggerEnter (Collider other) {
            if (other.GetComponent<IApproachable>() != null) {
                other.GetComponent<IApproachable>().OnApproach();
            }
        }

        private void OnTriggerExit (Collider other) {
            if (other.GetComponent<IApproachable>() != null) {
                other.GetComponent<IApproachable>().OnDepart();
            }
        }
    }
}