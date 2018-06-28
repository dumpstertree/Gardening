using UnityEngine;

public class Piece : MonoBehaviour {

	
	// ****************** Public *************************

	public delegate void GrabEvent ();
	public GrabEvent OnGrab;

	public delegate void ReleaseEvent ();
	public ReleaseEvent OnRelease;

	public void SetPart ( Part part, int startRow, int startCollumn, GunCrafting crafting ) {
		
		_stats = part.BuilderStats;
		_height = 5;
		_targetPos = new Vector3( transform.position.x, transform.position.y, _height );
		
		for( int x=0; x<part.Blocks.GetLength(0); x++) {
			
			for( int y=0; y<part.Blocks.GetLength(1); y++) {
				
				var inst = Block.GetInstance( part.Blocks[ x, y ] );
				
				if ( inst != null ) {
					
					var blockPos = crafting.PositionInWorldSpace( startRow + x, startCollumn + y );

					inst.transform.SetParent( transform );
					inst.transform.position = new Vector3( blockPos.x, blockPos.y );
				}
			}
		}

		_renderers = GetComponentsInChildren<MeshRenderer>();
		_blocks = GetComponentsInChildren<Block>();
		_projectors = GetComponentsInChildren<Projector>();
		_recievers = GetComponentsInChildren<Reciever>();
		_lights = GetComponentsInChildren<ActivationLight>( true );
		_crafting = crafting;

		foreach ( Reciever r in _recievers ) {
			r.OnRecieve += HandleRecieveEvent;
		}

		foreach ( Block b in _blocks ) {
			b.OnAction += HandlePerformActionEvent;
		}
	}
	public void RequestRelease () {

		if ( _isValid ) {
			Release ();
		}
	}
	public void RequestRotate ( float rotation ) {

		Rotate( rotation );
	}
	public void RequestShift ( int row, int collumn ) {

		Shift( row, collumn );
	}
	public void RequestReturnToOrigin () {

	}

	
	// ****************** Private *************************
	
	[SerializeField] private Material _grabbedMaterial;
	[SerializeField] private Material _idleMaterial;
	[SerializeField] private Material _invalidMaterial;

	private BuiltStats _stats;
	private bool _isGrabbed;
	private Vector3 _grabPoint;
	private Vector3 _grabOffset;
	private float _height = 0;

	private Vector2 _targetPos;
	private Quaternion _targetRotation = Quaternion.identity;

	private GunCrafting _crafting;
	private Block[] _blocks;
	private Reciever[] _recievers;
	private Projector[] _projectors;
	private MeshRenderer[] _renderers;
	private ActivationLight[] _lights;

	private bool _isValid {
		get{ foreach ( Block b in _blocks ) { if ( !b.IsValid ) return false; } 
			return true;
		}
	}
	
	private void Update () {

		transform.position = Vector3.Lerp( transform.position, new Vector3( _targetPos.x, _targetPos.y, _height), 0.5f );
		transform.rotation = Quaternion.Slerp( transform.rotation, _targetRotation, 0.5f );
		
		ActivateLights( false );
	}
	private void Grab ( Block block ) {
		
		_isGrabbed = true;
		_grabPoint = block.transform.position;
		_grabOffset = transform.position - _grabPoint;
		_height = _isGrabbed ? 4 : 5;

		FireGrabEvent ();
	}
	private void Release () {

		_isGrabbed = false;
		_height = _isGrabbed ? 4 : 5;

		FireReleaseEvent ();
	}
	private void Shift ( int row, int collumn ) {

		print( row );
		var pos = _crafting.PositionInWorldSpace( row, collumn );
		_targetPos = new Vector3( pos.x, pos.y, _height) + _grabOffset;
	}
	private void Rotate ( float rotation ) {
		
		_targetRotation = Quaternion.AngleAxis( rotation, Vector3.forward ) * _targetRotation; 
	}
	private void ActivateLights ( bool activation ) {
		
		foreach ( ActivationLight l in _lights ) {

			if  ( activation ) {
				l.Activate();
			} else {
				l.Deactivate();
			}
		}
	}

	
	// ****************** Block Events *************************
	
	private void HandlePerformActionEvent ( Block block ) {

		if ( !_isGrabbed ) {
			Grab( block );
		} else {
			Release();
		}
	}
	

	// ****************** Reciever Events *************************
	
	private void HandleRecieveEvent ( BuiltStats stats ) {

		stats.Add( _stats );

		foreach ( Projector p in _projectors ) {
			p.Project( stats );
		}

		ActivateLights( true );
	}


	// ****************** Fire Events *************************

	private void FireGrabEvent () {

		if ( OnGrab != null ) {
			OnGrab ();
		}
	}
	private void FireReleaseEvent () {

		if ( OnRelease != null ) {
			OnRelease ();
		}
	}
}
