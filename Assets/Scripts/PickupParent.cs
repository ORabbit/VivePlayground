using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class PickupParent : MonoBehaviour {

	SteamVR_TrackedObject trackedObj;
	SteamVR_Controller.Device device;

	void Awake () {
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
	}

	void FixedUpdate () {
		device = SteamVR_Controller.Input ((int)trackedObj.index);

		if (device.GetTouch (SteamVR_Controller.ButtonMask.Trigger)) {
			Debug.Log ("Trigger is held 'Touch'");
		}

		if (device.GetTouchDown (SteamVR_Controller.ButtonMask.Trigger)) {
			Debug.Log ("Trigger is held down 'Down'");
		}

		if (device.GetTouchUp (SteamVR_Controller.ButtonMask.Trigger)) {
			Debug.Log ("Trigger is released 'Up'");
		}

		if (device.GetPress (SteamVR_Controller.ButtonMask.Trigger)) {
			Debug.Log ("Trigger is pressed 'Press'");
		}

		if (device.GetPressDown (SteamVR_Controller.ButtonMask.Trigger)) {
			Debug.Log ("Trigger is pressed 'Down'");
		}

		if (device.GetPressUp (SteamVR_Controller.ButtonMask.Trigger)) {
			Debug.Log ("Trigger is press released 'Up'");
		}
	}

	void OnTriggerStay(Collider col) {
		Debug.Log ("Collided with " + col.name + " and activated OnTriggerStay");

		if (device.GetTouch (SteamVR_Controller.ButtonMask.Trigger)) {
			Debug.Log ("Collided with " + col.name + " while holding down Touch");
			// This way it is not effected by physics system. Instead it will be effected by us.
			col.attachedRigidbody.isKinematic = true;
			col.gameObject.transform.SetParent (this.gameObject.transform); // Our current game object is now the parent
		}
		if (device.GetTouchUp (SteamVR_Controller.ButtonMask.Trigger)) {
			Debug.Log ("Released Touch while collding with " + col.name);
			// Detach from us (parent) and turn on physics.
			col.gameObject.transform.SetParent (null);
			col.attachedRigidbody.isKinematic = false;
		}
	}
}
