using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Characters.Player
{
    public class MultiFocusCam : MonoBehaviour
    {
        public Transform target;
        public Transform player;
        public float ScreenBorder;
        public float minAngle;
        public float minDist;
        public float angleChange;
        public float stiffness = 15f;
        
        Quaternion _goalRot;
        Vector3 _goalPos;

        // Update is called once per frame
        void FixedUpdate () {
            Vector3 targetDir = target.position - player.position;
            float targetY = targetDir.y;
            targetDir.y = 0f;
            float targetDist = targetDir.magnitude;
            float yAngle = 90f - (90f - minAngle) * targetDist / (targetDist + angleChange);
            _goalRot = Quaternion.LookRotation(targetDir) *
                                             Quaternion.AngleAxis(yAngle, Vector3.right);

            Vector4 playerPos = player.position;
            playerPos.w = 1f;
            playerPos = Camera.main.worldToCameraMatrix * playerPos;
            float camDist = -1.2f* playerPos.y / Mathf.Tan(Mathf.Deg2Rad * Camera.main.fieldOfView / 2f);
            camDist = Mathf.Max(minDist, camDist);
            targetDir.y = targetY;
            camDist += Vector3.Dot(Camera.main.transform.forward, targetDir);

            _goalPos = target.position - camDist * Camera.main.transform.forward;
            
            Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, _goalRot, 
                stiffness*Time.fixedDeltaTime);
            
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, _goalPos,
                stiffness*Time.fixedDeltaTime);
        }
        
        public void RelinquishControl(CameraController ctrl) {
            Vector3 targetDir = target.position - player.position;
            targetDir.y = 0f;
            float targetDist = targetDir.magnitude;
            float yAngle = 90f - (90f - minAngle) * targetDist / (targetDist + angleChange);
            ctrl.AcceptControl(Quaternion.LookRotation(targetDir), yAngle);
        }
    }
}