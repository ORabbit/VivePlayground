using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class PickupParent : MonoBehaviour {

	SteamVR_TrackedObject trackedObj;
	SteamVR_Controller.Device device;

	public Transform sphere;

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

		// On touchpad up, reset sphere position
		if (device.GetPressUp (SteamVR_Controller.ButtonMask.Touchpad)) {
			Debug.Log ("Touchpad is pressed up 'Up'");
			sphere.transform.position = new Vector3 (0, 0, 0);
			sphere.GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, 0); // or Vector3.zero
			sphere.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
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

			tossObject (col.attachedRigidbody);
		}
	}

	// Method to toss and object simply using the STEAMVR controller velocity variables
	void tossObject(Rigidbody rigidBody) {
		Transform origin = trackedObj.origin ? trackedObj.origin : trackedObj.transform.parent;
		if (origin != null) { // Toss using world space vector conversions
			rigidBody.velocity = origin.TransformVector (device.velocity);
			rigidBody.angularVelocity = origin.TransformVector (device.angularVelocity);
		} else { // Very simply toss
			rigidBody.velocity = device.velocity;
			rigidBody.angularVelocity = device.angularVelocity;
		}
	}
}
