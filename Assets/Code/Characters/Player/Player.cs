using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Walk))]
[RequireComponent(typeof(CameraController))]
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


    void Start () {
        walkComponent = GetComponent<Walk>();
        camController = GetComponent<CameraController>();


        // Figure out which set of input axes to use
        string platform = "";
        string controller = "";
        string[] controllers = Input.GetJoystickNames();
        if(controllers.Length > 0) {
            if(controllers[0].Contains("Xbox")) controller = "Xbox";
            if(Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer) {
                platform = "Mac";
            }
            else {
                platform = "Win";
            }
        }

        // Initialize input axes
        buttons.moveX = "leftHorizontal" + controller + platform;
        buttons.moveY = "leftVertical" + controller + platform;
        buttons.camX = "rightHorizontal" + controller + platform;
        buttons.camY = "rightVertical" + controller + platform;
    }
	
	void Update () {
        switch(state) {
            case PlayerState.walking:
                float x = Input.GetAxisRaw(buttons.moveX);
                float y = Input.GetAxisRaw(buttons.moveY);
                if(x*x + y*y < 0.01) {
                    walkComponent.walkingDir = Vector3.zero;
                    break;
                }
                Vector3 forwards = Vector3.Cross(camController.camera.transform.right, Vector3.up).normalized;
                walkComponent.walkingDir = x*camController.camera.transform.right + y*forwards;
                break;
        }

        float camX = Input.GetAxisRaw(buttons.camX);
        float camY = Input.GetAxisRaw(buttons.camY);
        if(camX*camX + camY*camY < 0.01) {
            camX = 0f;
            camY = 0f;
        }
        camController.moveCamera(camX*Time.deltaTime, camY*Time.deltaTime);

       
	}

	//void FixedUpdate()
	//{
 //       float camX = Input.GetAxis(buttons.camX);
 //       float camY = Input.GetAxisRaw(buttons.camY);
 //       if(camX*camX + camY*camY < 0.01) {
 //           camX = 0f;
 //           camY = 0f;
 //       }
 //       camController.moveCamera(camX*Time.fixedDeltaTime, camY*Time.fixedDeltaTime);

	//}


}
