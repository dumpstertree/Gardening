using UnityEngine;

public class GunCrafting : MonoBehaviour {

	public enum State {
		Invalid,
		PickingPartFromList,
		PickingPartFromGrid,
		MovingPartOnGrid
	}

	public Vector2 PositionInCameraSpaceForRowAndCollumn ( int row, int collumn ) {

		var xSpacing = (_camera.orthographicSize*2)/_grid.Rows;
		var ySpacing = (_camera.orthographicSize*2)/_grid.Collumns;
		var startPos = new Vector2( -_camera.orthographicSize, -_camera.orthographicSize );
		var edgePadding = new Vector2( xSpacing/2, ySpacing/2 );

		return startPos + edgePadding + new Vector2( xSpacing * collumn, ySpacing * row );
	}
	public Vector3 PositionInWorldSpace ( int row, int collumn ) {

		var xSpacing = (_camera.orthographicSize*2)/_grid.Rows;
		var ySpacing = (_camera.orthographicSize*2)/_grid.Collumns;
		var startPos = new Vector3( -_camera.orthographicSize, -_camera.orthographicSize );
		var edgePadding = new Vector3( xSpacing/2, ySpacing/2 );

		return _camera.transform.TransformPoint( startPos + edgePadding + new Vector3( xSpacing * collumn, ((_grid.Rows-1) - row ) * ySpacing, _camera.transform.position.z ) );
	}
	private Block GetBlockAtCameraPos ( int row, int collumn ) {
		
		RaycastHit hit;
		var worldSpace = PositionInWorldSpace( row, collumn );
		var forward = _camera.transform.forward;

        if (Physics.Raycast( worldSpace, forward, out hit, Mathf.Infinity )) {

        	var block = hit.collider.GetComponent<Block>();
        	if ( block != null ) {
	        	
	        	return block;
	    	}
        } 

        return null;
	}


	// **************** Private *******************

	[SerializeField] private Camera _camera;
	[SerializeField] private Projector _rootProjector;
	[SerializeField] private Transform _workspaceContent;
	
	[Header( "Subpanels" )]
	[SerializeField] private Grid _grid;
	[SerializeField] private PartList _partList;
	[SerializeField] private StatBlock _gunStatsBlock;
	[SerializeField] private PartOverlay _partOverlay;

	private int _spawnRow = 5;
	private int _spawnCollumn = 5;

	private State _state;
	private Piece _grabbedPiece;
	private BuiltStats _stats;


	private void Awake () {

		// grid events
		_grid.OnClicked += HandleActionEvent;
		_grid.OnShift += HandleShiftEvent;
		_grid.OnRotate += HandleRotateEvent;
		_grid.OnTriedToBreakFreeLeft += HandleTriedToBreakFreeLeft;

		// part list events
		_partList.OnPartClicked += HandlePartClickedEvent;
		_partList.OnTriedToBreakFreeRight += HandleTriedToBreakFreeRight;
		_partList.OnShift += HandlePartListShift;
	}
	private void Start () {

		ChangeState( State.PickingPartFromGrid );
	}
	private void LateUpdate () {
		
		_stats = new BuiltStats();
		_rootProjector.Project( _stats );

		_gunStatsBlock.SetBlock( _stats );
	}
	private void CreatePart ( Part part ) {

		var piece = new GameObject( part.Name ).AddComponent<Piece>();
		
		piece.transform.SetParent( _workspaceContent, false );
		
		piece.SetPart( part, _spawnRow, _spawnCollumn, this );
		piece.OnGrab += () => { HandleGrabEvent( piece ); };
		piece.OnRelease += () => { HandleReleaseEvent( piece ); };

		_grid.SetCollumnAndRow( _spawnRow, _spawnCollumn );

		HandleActionEvent( _spawnRow, _spawnCollumn );
	}


	// **************** States *******************

	private void ChangeState ( State state ) {
		
		if ( _state != state ) {
		
			switch ( _state ) {
		
				case State.PickingPartFromList:
					ExitPickingFromPartList ();
					break;
		
				case State.PickingPartFromGrid:
					ExitPickingFromGrid ();
					break;
		
				case State.MovingPartOnGrid:
					ExitMovingPartOnGrid ();
					break;
			}
			
			_state = state;
		
			switch ( _state ) {
		
				case State.PickingPartFromList:
					EnterPickingFromPartList ();
					break;
		
				case State.PickingPartFromGrid:
					EnterPickingFromGrid ();
					break;
		
				case State.MovingPartOnGrid:
					EnterMovingPartOnGrid ();
					break;
			}
		}
	}
	private void ExitPickingFromPartList () {
	}
	private void ExitPickingFromGrid () {
	}
	private void ExitMovingPartOnGrid () {
	}
	private void EnterPickingFromPartList () {
	
		_grid.Enabled = false;
		_partList.Enabled = true;
		_partOverlay.Enabled = true;
	}
	private void EnterPickingFromGrid () {
		
		_grid.Enabled = true;
		_partList.Enabled = false;
		_partOverlay.Enabled = false;
	}
	private void EnterMovingPartOnGrid () {
			
		_grid.Enabled = true;
		_partList.Enabled = false;
		_partOverlay.Enabled = false;
	}
	
	
	// **************** Grid Events *******************

	private void HandleShiftEvent ( int row, int collumn ) {

		if ( _state == State.MovingPartOnGrid) {
			_grabbedPiece.RequestShift( row, collumn );
		}
	}
	private void HandleRotateEvent ( float rotation ) {

		if ( _state == State.MovingPartOnGrid) {
			_grabbedPiece.RequestRotate( rotation );
		}
	}
	private void HandleActionEvent ( int row, int collumn ) {
				
		print("1"); 
		if ( _state == State.MovingPartOnGrid) {
			print( "2" );
			_grabbedPiece.RequestRelease();
			return;
		}
		print( row + " : " + collumn );

		var block = GetBlockAtCameraPos( row, collumn );
		if ( block != null ) {
			print( "action" );
			block.PerformAction();
		}
	}
	private void HandleTriedToBreakFreeLeft () {

		if ( _state == State.PickingPartFromGrid ) {
			
			ChangeState( State.PickingPartFromList );
		}
	}

	
	// **************** Piece Events *******************

	private void HandleGrabEvent ( Piece piece ) {

		// add refrence
		_grabbedPiece = piece;

		// change state
		ChangeState( State.MovingPartOnGrid );
	}
	private void HandleReleaseEvent ( Piece piece ) {

		// remove refrence
		if( _grabbedPiece == piece ) { _grabbedPiece = null; }

		// change state
		ChangeState( State.PickingPartFromGrid );
	}


	// **************** Part List Events *******************

	private void HandlePartListShift ( int newIndex ) {

		_partOverlay.SetPart( _partList.PeakAtPart( newIndex ) );
	}
	private void HandlePartClickedEvent ( int atIndex )  {

		var part = _partList.TakePart( atIndex );
		CreatePart( part );
	}
	private void HandleTriedToBreakFreeRight() {

		ChangeState( State.PickingPartFromGrid );
	}
}


public class BuildSubpanel : MonoBehaviour {

	public bool Enabled {
		get {
			return _enabled;
		}
		set { 
			if ( _enabled != value ) {
				_enabled = value;
				if ( value ) { Enable();
				} else { Disable(); }
			} 
		}
	}

	private bool _enabled = true;

	protected virtual void Enable () {}
	protected virtual void Disable () {}
}



public class Part {

	public string Name { get; }
	public char[,] Blocks { get; }
	public BuiltStats BuilderStats { get; }

	public Part( string name ) {

		// Projectors 	⇡ ⇢ ⇣ ⇠
		// Recievers  	∪ ⊂ ∩ ⊃

		Name = name;
		BuilderStats = new BuiltStats( Random.Range( -2, 2 ),
									   Random.Range( -2, 2 ),
									   Random.Range( -2, 2 ),
									   Random.Range( -2, 2 ),
									   Random.Range( -2, 2 ),
									   Random.Range( -2, 2 ),
									   Random.Range( -2, 2 ) );
		
		var r = Random.Range( 0, 3);

		if( r == 0 ) {
			Blocks = new char[3,3] {
				
				{ 'o','x','⇢' },
				{ 'x',' ',' ' },
				{ '∩',' ',' ' }
			};
		}
		if( r == 1 ) {
			Blocks = new char[3,3] {
				
				{ '⊃','o','⇢' },
				{ ' ',' ',' '},
				{ ' ',' ',' ' }
			};
		}
		if( r == 2 ) {
			Blocks = new char[3,3] {
				
				{ 'x','∪','x' },
				{ '⇣','o','⇣' },
				{ ' ',' ',' ' }
			};
		}
	}
}





