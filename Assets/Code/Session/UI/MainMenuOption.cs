using Code;
using Code.Characters.Player;
using UnityEngine;

public class MainMenuOption : MonoBehaviour {
	public enum MenuOption {
		StartGame,
		LoadGame,
		Options,
		Acknowledgements,
		QuitGame
	}

	public MenuOption Option;

	GameObject _active;
	bool _state {
		get { return _active.activeInHierarchy; }
		set { _active.SetActive (value); }
	}

	void Start ()
	{
		_active = transform.Find ("Active").gameObject;
		_state = false;
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.GetComponent<Player> () == null) { return; }
		SetOptionState (true);
	}

	void OnTriggerExit (Collider other)
	{
		if (other.gameObject.GetComponent<Player> () == null) { return; }
		SetOptionState (false);
	}

	void SetOptionState (bool state)
	{
		_state = state;
	}


	void Update ()
	{
		if (!_state) { return; }
		// todo this should be pressing the "action" button
		if (Input.GetKeyDown (KeyCode.Space)) {
			FulfillPurpose ();
		}
	}

	// cheekily named
	protected virtual void FulfillPurpose ()
	{
		StartNewGame ();
	}

	protected virtual void ChangeMenuItem ()
	{
		if (Input.GetKeyDown (KeyCode.Q)) {

		} else if (Input.GetKeyDown (KeyCode.E)) {

		}
	}


	void StartNewGame ()
	{
		Game.Sesh.StartGame (1);
	}

	void LoadGame () { }

	static void OpenOptions () { }

	static void QuitGame ()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit(); // Complains about dead code. Boo.
#endif
	}

	void LoadAcknowledgements () { }

}
