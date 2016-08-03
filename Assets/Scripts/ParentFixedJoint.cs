using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class ParentFixedJoint : MonoBehaviour {

	public Rigidbody rigidBodyAttachPoint;
	public Transform sphere;

	SteamVR_TrackedObject trackedObj;
	SteamVR_Controller.Device device;

	FixedJoint fixedJoint;

	void Awake () {
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
	}

	void FixedUpdate () {
		device = SteamVR_Controller.Input ((int)trackedObj.index);

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
		if (fixedJoint == null && device.GetTouch (SteamVR_Controller.ButtonMask.Trigger)) {
			fixedJoint = col.gameObject.AddComponent<FixedJoint> ();
			fixedJoint.connectedBody = rigidBodyAttachPoint;
		} else if (fixedJoint != null && device.GetTouchUp (SteamVR_Controller.ButtonMask.Trigger)) {
			GameObject go = fixedJoint.gameObject;
			Rigidbody rigidBody = go.GetComponent<Rigidbody> ();
			Object.Destroy (fixedJoint); // Breaks off joint
			fixedJoint = null;
			tossObject (rigidBody);
		}
	}

	// Method to toss an object simply using the STEAMVR controller velocity variables
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
