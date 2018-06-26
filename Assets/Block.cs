using UnityEngine;

public class Block : MonoBehaviour {
	
	
	// ****************** Events *********************
	
	public delegate void GrabEvent ( Vector3 grabPoint_World );
	public GrabEvent OnGrab;

	public delegate void ReleaseEvent ( Block block );
	public ReleaseEvent OnRelease;
	
	public delegate void ShiftEvent ( Vector3 newCursorPoint );
	public ShiftEvent OnShift;

	public delegate void RotateEvent ( float rotation );
	public RotateEvent OnRotate;

	
	// ****************** Public *********************

	public bool IsGrabbed { 
		set; get;
	}
	public bool IsValid {
		get { return GetIsValid(); }
	}

	
	// ****************** Private *********************

	private bool GetIsValid () {
		
		RaycastHit hit;
 		if (Physics.Raycast( transform.position, Vector3.forward, out hit, Mathf.Infinity )) {
 			return false;
 		} 

 		return true;
	}
	

	public void Grab () {

		if ( !IsGrabbed ) {

			if ( OnGrab != null ) {
				OnGrab ( transform.position );
			} 
		} else{
			
			if ( OnRelease != null ) {
				OnRelease ( this );
			} 
		}

		IsGrabbed = !IsGrabbed;
	}
	public void Shift ( Vector3 cameraSpace ) {

		if ( OnShift != null ) {
			OnShift ( cameraSpace );
		}
	}
	public void Rotate ( float rotation ) {

		if ( OnRotate != null ) {
			OnRotate ( rotation );
		}
	}
}
