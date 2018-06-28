using UnityEngine;

public class GunCrafting : MonoBehaviour {

	public enum State {
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
	public Vector2 PositionInWorldSpace ( int row, int collumn ) {

		var xSpacing = (_camera.orthographicSize*2)/_grid.Rows;
		var ySpacing = (_camera.orthographicSize*2)/_grid.Collumns;
		var startPos = new Vector2( -_camera.orthographicSize, -_camera.orthographicSize );
		var edgePadding = new Vector2( xSpacing/2, ySpacing/2 );

		return _camera.transform.TransformPoint( startPos + edgePadding + new Vector2( xSpacing * collumn, ((_grid.Rows-1) - row ) * ySpacing ) );
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

	[SerializeField] private Grid _grid;
	[SerializeField] private Camera _camera;
	[SerializeField] private Projector _rootProjector;
	[SerializeField] private PartList _partList;
	[SerializeField] private Transform _workspaceContent;
	[SerializeField] private StatBlock _gunStatsBlock;
	[SerializeField] private PartOverlay _partOverlay;

	private int _startRow = 0;
	private int _startCollumn = 0;
	private int _spawnRow = 5;
	private int _spawnCollumn = 5;

	private Piece _grabbedPiece;
	private BuiltStats _stats = new BuiltStats();


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
	private void LateUpdate () {
		
		_stats = new BuiltStats();
		_rootProjector.Project( _stats );

		_gunStatsBlock.SetBlock( _stats );
	}
	private void CreatePart ( Part part ) {

		var piece = new GameObject( part.Name ).AddComponent<Piece>();
		
		piece.transform.SetParent( _workspaceContent, false );
		piece.transform.position = PositionInWorldSpace( _spawnRow, _spawnCollumn );
		
		piece.SetPart( part, _spawnRow, _spawnCollumn, this );
		piece.OnGrab += () => { HandleGrabEvent( piece ); };
		piece.OnRelease += () => { HandleReleaseEvent( piece ); };

		_grid.SetCollumnAndRow( _spawnRow, _spawnCollumn );

		_grid.Enabled = true;
		_partList.Enabled = false;

		HandleActionEvent( _spawnRow, _spawnCollumn );
	}
	
	
	// **************** Grid Events *******************

	private void HandleShiftEvent ( int row, int collumn ) {

		if ( _grabbedPiece != null ) {
			_grabbedPiece.RequestShift( row, collumn );
		}
	}
	private void HandleRotateEvent ( float rotation ) {

		if ( _grabbedPiece != null ) {
			_grabbedPiece.RequestRotate( rotation );
		}
	}
	private void HandleActionEvent ( int row, int collumn ) {
			
		if ( _grabbedPiece != null ) {
			_grabbedPiece.RequestRelease();
			return;
		}

		var block = GetBlockAtCameraPos( row, collumn );
		if (  block != null ) {
			block.PerformAction();
		}
	}
	private void HandleTriedToBreakFreeLeft () {

		if ( _grabbedPiece == null ) {
			
			_grid.Enabled = false;
			_partList.Enabled = true;
		}
	}
	
	
	// **************** Piece Events *******************

	private void HandleGrabEvent ( Piece piece ) {

		_grabbedPiece = piece;
	}
	private void HandleReleaseEvent ( Piece piece ) {

		if( _grabbedPiece == piece ) {
			_grabbedPiece = null;
		}
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

		_grid.Enabled = true;
		_partList.Enabled = false;
	}
}
