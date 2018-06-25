using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunCrafting : MonoBehaviour {

	// **************** Private *******************

	[SerializeField] private Grid _grid;
	[SerializeField] private Camera _camera;

	private Piece _pieceInHand;

	private bool _isCarringPiece {
		get{ return _pieceInHand != null; }
	}

	private void Awake () {

		_grid.OnClicked += HandleOnGridClicked;
		_grid.OnShift += HandleOnShift;
		_grid.OnRotate += HandleOnRotate;
	}
	private void GrabPiece ( Piece piece, Vector3 cameraPosition  ) {

		_pieceInHand = piece;
		_pieceInHand.GrabPoint = ConvertCameraSpaceToWorld( cameraPosition );
		_pieceInHand.IsGrabbed = true;
	}	
	private void ReleasePiece () {

		_pieceInHand.IsGrabbed = false;
		_pieceInHand = null;
	}
	private Vector2 ConvertCameraSpaceToWorld ( Vector2 camera ) {
		return _camera.transform.TransformPoint( camera );
	}

	
	// **************** Event Handlers *******************

	private void HandleOnGridClicked ( Vector2 cameraPosition ) {

		if ( _isCarringPiece ){
	    	ReleasePiece ();
	    	return;
	    }

		RaycastHit hit;
		var worldSpace = ConvertCameraSpaceToWorld( cameraPosition );
		var forward = _camera.transform.forward;

        if (Physics.Raycast( worldSpace, forward, out hit, Mathf.Infinity )) {

        	var p = hit.collider.GetComponent<Piece>();
        	if ( p != null ) {
	        		
	        	if ( !_isCarringPiece ){

					GrabPiece ( p, cameraPosition );
	    		}
	    	}
        } 
	}
	private void HandleOnShift ( Vector2 cameraPosition ) {

		if ( _isCarringPiece ) {
			_pieceInHand.Shift( ConvertCameraSpaceToWorld( cameraPosition ) );
		}
	}
	private void HandleOnRotate ( float rotation ) {

		if ( _isCarringPiece ) {
			_pieceInHand.Rotate( rotation );
		}
	}
}
