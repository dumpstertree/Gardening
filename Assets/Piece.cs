using UnityEngine;

public class Piece : MonoBehaviour {

	// ****************** Public *************************

	public bool IsGrabbed { 
		set { SetIsGrabbed( value ); }
		get { return _isGrabbed; } 
	}
	public Vector3 GrabPoint {
		set { SetGrabPoint( value ); }
		get { return _grabPoint; }
	}

	public void Shift ( Vector3 cameraSpace ) {

		transform.position = new Vector3( cameraSpace.x, cameraSpace.y, 5) - _grabOffset;
	}
	public void Rotate ( float rotation ) {

		transform.RotateAround( transform.TransformPoint( _grabOffset ), transform.forward, rotation );
	}


	// ****************** Private *************************
	
	[SerializeField] private Material _grabbedMaterial;
	[SerializeField] private Material _idleMaterial;
	[SerializeField] private Material _invalidMaterial;
	[SerializeField] private Vector3 _bounds;

	private bool _isGrabbed;
	private Vector3 _grabPoint;
	private MeshRenderer[] _renderers;
	private Vector3 _grabOffset;


	private void Awake () {

		_renderers = GetComponentsInChildren<MeshRenderer>();
	}
	private void SetIsGrabbed ( bool isGrabbed ) {

		_isGrabbed = isGrabbed;

		if ( isGrabbed ) {
			foreach ( MeshRenderer r in _renderers ) {
				r.material = _grabbedMaterial;
			}
		} else {
			foreach ( MeshRenderer r in _renderers ) {
				r.material = _idleMaterial;
			}
		}
	}
	private void SetGrabPoint ( Vector3 grabPoint ) {

		_grabPoint = grabPoint;
		_grabOffset = new Vector3( transform.InverseTransformPoint( grabPoint ).x, transform.InverseTransformPoint( grabPoint ).y, 0);
	}
}
