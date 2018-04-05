using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public struct Buttons {
        public string moveX;
        public string moveY;
        public string camX;
        public string camY;
    }
    [HideInInspector] public Buttons buttons;

    public enum PlayerState {
        walking,
        selecting
    }

    [HideInInspector] public PlayerState state;

    Walk walkComponent;
    CameraController camController;


    void Start () {
        walkComponent = GetComponent<Walk>();
        camController = GetComponent<CameraController>();

        buttons.moveX = "leftHorizontal";
        buttons.moveY = "leftVertical";
        buttons.camX = "rightHorizontal";
        buttons.camY = "rightVertical";
    }
	
	void Update () {
        switch(state) {
            case PlayerState.walking:
                float x = Input.GetAxisRaw(buttons.moveX);
                float y = Input.GetAxisRaw(buttons.moveY);
                Vector3 forwards = Vector3.Cross(camController.camera.transform.right, Vector3.up).normalized;
                walkComponent.walkingDir = x*camController.camera.transform.right + y*forwards;
                break;
        }

        camController.moveCamera(Input.GetAxis(buttons.camX), Input.GetAxisRaw(buttons.camY));

       
	}

	
}
