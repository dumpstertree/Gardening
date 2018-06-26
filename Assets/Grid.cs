using UnityEngine;

public class Grid : MonoBehaviour {


	// ********************* Public *****************
	
	public delegate void ClickedEvent( Vector2 cameraPosition );
	public ClickedEvent OnClicked;

	public delegate void ShiftEvent( Vector2 cameraPosition );
	public ShiftEvent OnShift;

	public delegate void RotateEvent( float rotation );
	public RotateEvent OnRotate;
	
	// ********************* Private *****************

	[SerializeField] private int _rows;
	[SerializeField] private int _collumns;
	[SerializeField] private int _currentRow;
	[SerializeField] private int _currentCollumn;
	[SerializeField] private Transform _cursor;
	[SerializeField] private Camera _camera;


	private bool _canShiftUp {
		get{ return _currentRow < _rows - 1; }
	}
	private bool _canShiftDown {
		get{ return _currentRow > 0; }
	}
	private bool _canShiftLeft {
		get{ return _currentCollumn > 0; }
	}
	private bool _canShiftRight {
		get{ return _currentCollumn < _collumns - 1; }
	}

	private void Update () {

		if ( Input.GetKeyDown( KeyCode.UpArrow ) ) {
			ShiftUp();
		}
		if ( Input.GetKeyDown( KeyCode.DownArrow ) ) {
			ShiftDown();
		}
		if ( Input.GetKeyDown( KeyCode.LeftArrow ) ) {
			ShiftLeft();
		}
		if ( Input.GetKeyDown( KeyCode.RightArrow ) ) {
			ShiftRight();
		}
		if ( Input.GetKeyDown( KeyCode.Space ) ) {
			FireClickedEvent( PositionInCameraSpaceForRowAndCollumn( _currentRow, _currentCollumn ) );
		}
		if ( Input.GetKeyDown( KeyCode.Q ) ) {
			RotateLeft();
		}
		if ( Input.GetKeyDown( KeyCode.W ) ) {
			RotateRight();
		}

		_cursor.localPosition = Vector3.Lerp( _cursor.localPosition , PositionForRowAndCollumn( _currentRow, _currentCollumn ), 0.5f );
	}
	private void ShiftUp () {

		if ( _canShiftUp ) {
			_currentRow++;
			FireShiftEvent( PositionInCameraSpaceForRowAndCollumn( _currentRow, _currentCollumn) );
		}
	}
	private void ShiftDown () {

		if ( _canShiftDown ) {
			_currentRow--;
			FireShiftEvent( PositionInCameraSpaceForRowAndCollumn( _currentRow, _currentCollumn) );		
		}
	}
	private void ShiftLeft () {

		if ( _canShiftLeft ) {
			_currentCollumn--;
			FireShiftEvent( PositionInCameraSpaceForRowAndCollumn( _currentRow, _currentCollumn) );
		}
	}
	private void ShiftRight () {

		if ( _canShiftRight ) {
			_currentCollumn++;
			FireShiftEvent( PositionInCameraSpaceForRowAndCollumn( _currentRow, _currentCollumn) );
		}
	}
	private void RotateLeft () {

		FireRotateEvent( -90f );
	}
	private void RotateRight () {
	
		FireRotateEvent( 90f );
	}

	
	// ********************* Helper *****************

	private Vector2 PositionForRowAndCollumn ( int row, int collumn ) {

		var spacingX = GetComponent<RectTransform>().rect.width / _collumns;
		var spacingY = GetComponent<RectTransform>().rect.height / _rows;

		var startPos = new Vector2( spacingX/2f, spacingY/2f ) + new Vector2( -GetComponent<RectTransform>().rect.width/2, -GetComponent<RectTransform>().rect.height/2 );

		return startPos + new Vector2(collumn * spacingY, row * spacingX);
	}
	private Vector2 PositionInCameraSpaceForRowAndCollumn ( int row, int collumn ) {

		var xSpacing = (_camera.orthographicSize*2)/_collumns;
		var ySpacing = (_camera.orthographicSize*2)/_rows;
		var startPos = new Vector2( -_camera.orthographicSize, -_camera.orthographicSize );
		var edgePadding = new Vector2( xSpacing/2, ySpacing/2 );

		return startPos + edgePadding + new Vector2( xSpacing * collumn, ySpacing * row );
	}


	// ********************* Fire Event *****************

	private void FireClickedEvent ( Vector2 cameraPosition ) {
		
		if ( OnClicked != null ) {
			OnClicked( cameraPosition );
		}
	}
	private void FireShiftEvent ( Vector2 cameraPosition ) {
		
		if ( OnShift != null ) {
			OnShift( cameraPosition );
		}
	}
	private void FireRotateEvent ( float rotation ) {
		
		if ( OnRotate != null ) {
			OnRotate( rotation );
		}
	}
	private void OnDrawGizmos () {

	 	Debug.DrawRay( _camera.transform.TransformPoint( PositionInCameraSpaceForRowAndCollumn( _currentRow, _currentCollumn)), _camera.transform.forward * 100 );
	}

}
