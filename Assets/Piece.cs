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
	public bool IsValid {
		get{ foreach ( Block b in _blocks ) {
				if ( !b.IsValid ) return false;
			} 
			return true;
		}
	}



	// ****************** Private *************************
	
	[SerializeField] private Material _grabbedMaterial;
	[SerializeField] private Material _idleMaterial;
	[SerializeField] private Material _invalidMaterial;
	[SerializeField] private Vector3 _bounds;

	private bool _isGrabbed;
	private Vector3 _grabPoint;
	
	private Vector3 _grabOffset;
	private float _height = 1f;

	private MeshRenderer[] _renderers;
	private Block[] _blocks;
	private Projector[] _projectors;
	private Reciever[] _recievers;


	private void Awake () {

		_renderers = GetComponentsInChildren<MeshRenderer>();
		_blocks = GetComponentsInChildren<Block>();
		_projectors = GetComponentsInChildren<Projector>();
		_recievers = GetComponentsInChildren<Reciever>();

		foreach ( Reciever r in _recievers ) {
			r.OnRecieve += HandleRecieveEvent;
		}
		foreach ( Block b in _blocks ) {
			b.OnGrab += HandleGrabEvent;
			b.OnShift += HandleShiftEvent;
			b.OnRotate += HandleRotateEvent;
			b.OnRelease += HandleReleaseEvent;
		}
	}
	private void SetIsGrabbed ( bool isGrabbed ) {

		_isGrabbed = isGrabbed;
		_height = _isGrabbed ? 4 : 5;

		ReloadVisual ();
		ReloadPosition ();
	}
	private void SetGrabPoint ( Vector3 grabPoint_World ) {

		// _grabPoint = grabPoint;
		// _grabOffset = new Vector3( transform.InverseTransformPoint( grabPoint ).x, transform.InverseTransformPoint( grabPoint ).y, 0);

		_grabPoint = grabPoint_World;
		_grabOffset = transform.position - grabPoint_World;
	}
	private void ReloadVisual () {

		if ( IsGrabbed ) {
			if ( IsValid ) {
				foreach ( MeshRenderer r in _renderers ) {
					r.material = _grabbedMaterial;
				}
			} else {
				foreach ( MeshRenderer r in _renderers ) {
					r.material = _invalidMaterial;
				}
			}
		} else {
			foreach ( MeshRenderer r in _renderers ) {
				r.material = _idleMaterial;
			}
		}
	}
	private void ReloadPosition () {

		Shift( transform.position - _grabOffset );
	}
	private void Shift ( Vector3 cameraSpace ) {

		transform.position = new Vector3( cameraSpace.x, cameraSpace.y, _height) + _grabOffset;
		ReloadVisual ();
	}
	private void Rotate ( float rotation ) {

		transform.RotateAround( transform.TransformPoint( _grabOffset ), transform.forward, rotation );
		ReloadVisual ();
	}
	

	// ****************** Projection *************************
	
	private void HandleRecieveEvent () {

		foreach ( Projector p in _projectors ) {
			p.Project();
		}
	}
	private void HandleGrabEvent ( Vector3 grabPoint_World ) {

		SetGrabPoint( grabPoint_World );
		IsGrabbed = true;
	}
	private void HandleReleaseEvent ( Block block ) {

		IsGrabbed = false;
	}
	private void HandleRotateEvent ( float rotation ) {
		
		Rotate( rotation );
	}
	private void HandleShiftEvent ( Vector3 newCursorPoint  ) {

		Shift( newCursorPoint );
	}
}
