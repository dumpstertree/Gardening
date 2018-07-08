using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ObjectRandomizer : EditorWindow {

	[MenuItem ("Custom/Object Randomizer")]
	public static void  ShowWindow () {
        EditorWindow.GetWindow(typeof(ObjectRandomizer));
    }

    public enum Mode {
		Painting,
		Selection
	}

	private bool _canRotateX;
	private bool _canRotateY;
	private bool _canRotateZ;
	private bool _canScaleX;
	private bool _canScaleY;
	private bool _canScaleZ;

	private float _minXRotation = -180f;
	private float _maxXRotation = 180f;
	private float _minYRotation = -180f;
	private float _maxYRotation = 180f;
	private float _minZRotation = -180f;
	private float _maxZRotation = 180f;

	private float _minXScale = 0.8f;
	private float _maxXScale = 1.2f;
	private float _minYScale = 0.8f;
	private float _maxYScale = 1.2f;
	private float _minZScale = 0.8f;
	private float _maxZScale = 1.2f;

	private float _paintingDensity = 1.0f;
	[SerializeField] private List<GameObject> _paintingPrefabs = new List<GameObject>();

	private Mode _mode;
	private bool _shiftDown;
	private bool _mouseDown;
	private KeyCode _paintKey = KeyCode.Space;
	private float _densityTimer = 0f;
	private bool _timerIsDone {
		get{ return _densityTimer >= _paintingDensity; }
	}


	private void OnEnable () {
		
		SceneView.onSceneGUIDelegate += SceneGUI;
  	}
	private void OnGUI() {

		ProccessEvents ();
		
		GUILayout.Label ("Object Randomizer", EditorStyles.boldLabel);
 		
 		// Rotation GUI
 		GUILayout.Space( 20f );
 		GUILayout.Label ("Rotation", EditorStyles.boldLabel);
 		CreateXRotationGUI ();
		CreateYRotationGUI();
		CreateZRotationGUI();

		GUILayout.Space( 20f );
		GUILayout.Label ("Scale", EditorStyles.boldLabel);
		CreateXScaleGUI ();
		CreateYScaleGUI();
		CreateZScaleGUI();

		GUILayout.Space( 20f );	
		 _mode = (Mode)EditorGUILayout.EnumPopup( "Mode", _mode );
		
		GUILayout.Space( 20f );	
		switch ( _mode ) {
			
			case Mode.Painting :		
				CreatePaintingGUI ();
				//DrawPaintBrush ();
				break;
			
			case Mode.Selection :
				CreateSelectionGUI ();
				break;
		}
	}
	private void SceneGUI( SceneView sceneView ) {
		
		ProccessEvents ();
		
		if ( _shiftDown && _timerIsDone ) {
			CreateObject ();
			ResetDensityTimer ();
		}
	}
	private void ProccessEvents () {

		int controlID = GUIUtility.GetControlID ( FocusType.Passive );
		Event events = Event.current;

		if ( events.GetTypeForControl( controlID ) == EventType.MouseDown ){
		}
		
		if ( events.GetTypeForControl( controlID ) == EventType.MouseUp ){
		}
		
		if ( events.GetTypeForControl( controlID ) == EventType.KeyDown ){
			if ( events.keyCode == _paintKey ) {
				_shiftDown = true;
				events.Use();
				ProgressDensityTimer ();
			}
		}
		
		if ( events.GetTypeForControl( controlID ) == EventType.KeyUp ){
			if ( events.keyCode == _paintKey ) {
				_shiftDown = false;
				ResetDensityTimer ();
			}
		}

		
		// switch (e.GetTypeForControl (controlID)) {
			
		// 	case EventType.MouseUp:
		// 		_mouseDown = false;
		// 		break;

		// 	case EventType.MouseDown:
		// 		_mouseDown = true;
		// 		if ( _shiftDown ) e.Use ();
		// 		break;

		
		// 	case EventType.MouseDrag:
		// 		_mouseDown = true;
		// 		if ( _shiftDown ) e.Use ();
		// 		break;

		
		// 	case EventType.KeyDown:
		// 		if( e.keyCode == KeyCode.Space ){  _shiftDown = true; }
		// 		break;

		// 	case EventType.KeyUp:
		// 		if( e.keyCode == KeyCode.Space ){  _shiftDown = false; }
		// 		break;
		// }
	}


	// Custom GUI
	private void CreateXRotationGUI () {
		
		_canRotateX = EditorGUILayout.BeginToggleGroup ("Can Rotate X", _canRotateX);
           _minXRotation = EditorGUILayout.Slider( "Min X Rotation", _minXRotation, -180, 180 );
           _maxXRotation = EditorGUILayout.Slider( "Max X Rotation", _maxXRotation, -180, 180 );
		EditorGUILayout.EndToggleGroup ();
	}
	private void CreateYRotationGUI () {
		
		_canRotateY = EditorGUILayout.BeginToggleGroup ("Can Rotate Y", _canRotateY);
           _minYRotation = EditorGUILayout.Slider( "Min Y Rotation", _minYRotation, -180, 180 );
           _maxYRotation = EditorGUILayout.Slider( "Max Y Rotation", _maxYRotation, -180, 180 );
		EditorGUILayout.EndToggleGroup ();
	}
	private void CreateZRotationGUI () {
		
		_canRotateZ = EditorGUILayout.BeginToggleGroup ("Can Rotate Z", _canRotateZ);
           _minZRotation = EditorGUILayout.Slider( "Min Z Rotation", _minZRotation, -180, 180 );
           _maxZRotation = EditorGUILayout.Slider( "Max Z Rotation", _maxZRotation, -180, 180 );
		EditorGUILayout.EndToggleGroup ();
	}
	private void CreateXScaleGUI () {
		
		_canScaleX = EditorGUILayout.BeginToggleGroup ("Can Scale X", _canScaleX);
           _minXScale = EditorGUILayout.Slider( "Min X Scale", _minXScale, 0, 10 );
           _maxXScale = EditorGUILayout.Slider( "Max X Scale", _maxXScale, 0, 10 );
		EditorGUILayout.EndToggleGroup ();
	}
	private void CreateYScaleGUI () {
		
		_canScaleY = EditorGUILayout.BeginToggleGroup ("Can Rotate Y", _canScaleY);
           _minYScale = EditorGUILayout.Slider( "Min Y Scale", _minYScale, 0, 10 );
           _maxYScale = EditorGUILayout.Slider( "Max Y Scale", _maxYScale, 0, 10 );
		EditorGUILayout.EndToggleGroup ();
	}
	private void CreateZScaleGUI () {
		
		_canScaleZ = EditorGUILayout.BeginToggleGroup ("Can Scale Z", _canScaleZ);
           _minZScale = EditorGUILayout.Slider( "Min Z Scale", _minZScale, 0, 10 );
           _maxZScale = EditorGUILayout.Slider( "Max Z Scale", _maxZScale, 0, 10 );
		EditorGUILayout.EndToggleGroup ();
	}
	private void CreatePaintingGUI () {

		_paintingDensity = EditorGUILayout.Slider( "Painting Density", _paintingDensity, 0.1f, 10f );
				
		if (_paintingPrefabs.Count > 0 ) {
			
			for( int i = _paintingPrefabs.Count-1; i>=0; i--){
				
				var go = _paintingPrefabs[ i ];
				
				// remove empty items
				if ( go == null ){ _paintingPrefabs.RemoveAt( i ); }
				
				// reassign old items and listen for changes
				var item = EditorGUILayout.ObjectField( (GameObject)go , typeof(GameObject), true) as GameObject;
				_paintingPrefabs[ i ] = item;
			}
		}


		// add open slot
		var newItem = EditorGUILayout.ObjectField( null, typeof( GameObject ), true) as GameObject;
		if ( newItem != null ) { _paintingPrefabs.Add( newItem ); }


		GUILayout.Label ("Hold SHIFT to start painting", EditorStyles.boldLabel);
	}
	private void CreateSelectionGUI () {
		
		if ( GUILayout.Button( "Randomize Selection" ) ) { 	
			foreach( Transform t in Selection.transforms ) {
				UpdateTransform( t ); 
			}
		}
	}

	// Actions
	private void UpdateTransform ( Transform t ) {

		var rotX = t.localRotation.eulerAngles.x;
		if ( _canRotateX ) { rotX = Random.Range( _minXRotation, _maxXRotation ); }

		var rotY = t.localRotation.eulerAngles.y;
		if ( _canRotateY ) { rotY = Random.Range( _minYRotation, _maxYRotation ); }

		var rotZ = t.localRotation.eulerAngles.z;
		if ( _canRotateZ ) { rotZ = Random.Range( _minZRotation, _maxZRotation ); }

		var scaleX = t.localScale.x;
		if ( _canScaleX ) { scaleX = Random.Range( _minXScale, _maxXScale ); }

		var scaleY = t.localScale.y;
		if ( _canScaleY ) { scaleY = Random.Range( _minYScale, _maxYScale ); }

		var scaleZ = t.localScale.z;
		if ( _canScaleZ ) { scaleZ = Random.Range( _minZScale, _maxZScale ); }

		t.localRotation = Quaternion.Euler( rotX, rotY, rotZ );
		t.localScale = new Vector3( scaleX, scaleY, scaleZ );
	}

	private void CreateObject () {
		
		RaycastHit hit;
	    Ray ray = HandleUtility.GUIPointToWorldRay( Event.current.mousePosition );

	    if ( Physics.Raycast ( ray, out hit, 100.0f ) ) {

	    	if ( _paintingPrefabs.Count > 0 ) {
				
		    	var prefab = _paintingPrefabs[ Random.Range( 0, _paintingPrefabs.Count ) ];
				var instance = PrefabUtility.InstantiatePrefab( prefab ) as GameObject;

				instance.name = prefab.name;
				UpdateTransform( instance.transform );

				instance.transform.position = hit.point;
			}
	    }
	}
	private void ProgressDensityTimer () {

		_densityTimer += Time.deltaTime;
	}
	private void ResetDensityTimer () {

		_densityTimer = 0f;
	}
}
