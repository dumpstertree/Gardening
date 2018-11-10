using UnityEngine;

namespace Eden.UI.Subpanels.Building {

	public class Grid : BuildSubpanel {


		// ********************* Public *****************
		
		public override void ReciveInput( Input.Package package ) {

			if ( !Enabled )  {
				return;
			}

			if ( package.Dpad.Up_Down ) {
				ShiftUp();
			}
			if ( package.Dpad.Down_Down ) {
				ShiftDown();
			}
			if ( package.Dpad.Left_Down ) {
				ShiftLeft();
			}
			if ( package.Dpad.Right_Down ) {
				ShiftRight();
			}
			if ( package.Face.Down_Down ) {
				FireClickedEvent( PositionInCameraSpaceForRowAndCollumn( _currentRow, _currentCollumn ) );
			}
			if ( package.BackLeft.Bumper_Down ) {
				RotateLeft();
			}
			if ( package.BackRight.Bumper_Down) {
				RotateRight();
			}
		}
		public int Rows {
			get {
				return _rows; 
			}
		}
		public int Collumns {
			get {
				return _collumns; 
			}
		}

		public void SetCollumnAndRow ( int row, int collumn ) {
			
			_currentRow = row;
			_currentCollumn = collumn;

			_cursor.localPosition = PositionForRowAndCollumn( _currentRow, _currentCollumn );
		}	

		public delegate void ClickedEvent( int row, int collumn );
		public ClickedEvent OnClicked;

		public delegate void ShiftEvent( int row, int collumn );
		public ShiftEvent OnShift;

		public delegate void RotateEvent( float rotation );
		public RotateEvent OnRotate;

		public delegate void TriedToBreakFreeEvent ();
		public TriedToBreakFreeEvent OnTriedToBreakFreeUp;
		public TriedToBreakFreeEvent OnTriedToBreakFreeDown;
		public TriedToBreakFreeEvent OnTriedToBreakFreeLeft;
		public TriedToBreakFreeEvent OnTriedToBreakFreeRight;

		
		// ********************* Private *****************


		[SerializeField] private Eden.UI.Panels.Building _building;
		[SerializeField] private int _rows;
		[SerializeField] private int _collumns;
		[SerializeField] private int _currentRow;
		[SerializeField] private int _currentCollumn;
		[SerializeField] private Transform _cursor;
		[SerializeField] private Camera _camera;


		private bool _canShiftDown {
			get{ 
				return _currentRow < _rows - 1; 
			}
		}
		private bool _canShiftUp {
			get{ 
				return _currentRow > 0; 
			}
		}
		private bool _canShiftLeft {
			get{ 
				return _currentCollumn > 0; 
			}
		}
		private bool _canShiftRight {
			get{ 
				return _currentCollumn < _collumns - 1; 
			}
		}


		protected override void Enable () {
				
			_cursor.gameObject.SetActive( true );
		}
		protected override void Disable () {
				
			_cursor.gameObject.SetActive( false );
		}


		private void Update () {

			_cursor.localPosition = Vector3.Lerp( _cursor.localPosition , PositionForRowAndCollumn( _currentRow, _currentCollumn ), 0.5f );
		}
		private void ShiftUp () {

			if ( _canShiftUp ) {
				_currentRow--;
				FireShiftEvent( _currentRow, _currentCollumn );
			} else {
				FireTriedToBreakFreeUp ();
			}
		}
		private void ShiftDown () {

			if ( _canShiftDown ) {
				_currentRow++;
				FireShiftEvent( _currentRow, _currentCollumn );
			} else {
				FireTriedToBreakFreeDown ();
			}
		}
		private void ShiftLeft () {

			if ( _canShiftLeft ) {
				_currentCollumn--;
				FireShiftEvent( _currentRow, _currentCollumn );
			} else {
				FireTriedToBreakFreeLeft ();
			}
		}
		private void ShiftRight () {

			if ( _canShiftRight ) {
				_currentCollumn++;
				FireShiftEvent( _currentRow, _currentCollumn );
			} else {
				FireTriedToBreakFreeRight ();
			}
		}
		private void RotateLeft () {

			FireRotateEvent( 90f );
		}
		private void RotateRight () {
		
			FireRotateEvent( -90f );
		}	
		private void OnDrawGizmos () {

		 	Debug.DrawRay( _building.PositionInWorldSpace( _currentRow, _currentCollumn ), _camera.transform.forward * 100 );
		}


		
		// ********************* Helper *****************

		private Vector2 PositionForRowAndCollumn ( int row, int collumn ) {

			var spacingX = GetComponent<RectTransform>().rect.width / _collumns;
			var spacingY = GetComponent<RectTransform>().rect.height / _rows;
			var edgePadding = new Vector2( spacingX/2f, spacingY/2f );
			var startPos = new Vector2( -GetComponent<RectTransform>().rect.width/2, -GetComponent<RectTransform>().rect.height/2 );

			return startPos + edgePadding + new Vector2( collumn * spacingX, ((Rows - 1 ) - row) * spacingY);
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
				OnClicked( _currentRow, _currentCollumn );
			}
		}
		private void FireShiftEvent ( int row, int collumn ) {
				
			if ( OnShift != null ) {
				OnShift( row, collumn );
			}
		}
		private void FireRotateEvent ( float rotation ) {
			
			if ( OnRotate != null ) {
				OnRotate( rotation );
			}
		}
		private void FireTriedToBreakFreeUp () {

			if ( OnTriedToBreakFreeUp != null ) {
				OnTriedToBreakFreeUp ();
			}
		}
		private void FireTriedToBreakFreeDown () {

			if ( OnTriedToBreakFreeDown != null ) {
				OnTriedToBreakFreeDown ();
			}
		}
		private void FireTriedToBreakFreeLeft () {

			if ( OnTriedToBreakFreeLeft != null ) {
				OnTriedToBreakFreeLeft ();
			}
		}
		private void FireTriedToBreakFreeRight () {

			if ( OnTriedToBreakFreeRight != null ) {
				OnTriedToBreakFreeRight ();
			}
		}
	}
}
