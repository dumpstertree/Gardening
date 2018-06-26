using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunCrafting : MonoBehaviour {

	// **************** Private *******************

	[SerializeField] private Grid _grid;
	[SerializeField] private Camera _camera;
	[SerializeField] private Projector _rootProjector;

	private Piece _pieceInHand;
	private Block _grabbedBlock;

	private bool _isCarringPiece {
		get{ return _pieceInHand != null; }
	}

	private void Awake () {

		_grid.OnClicked += HandleGrabEvent;
		_grid.OnShift += HandleShiftEvent;
		_grid.OnRotate += HandleRotateEvent;
	}

	private void HandleShiftEvent ( Vector2 cameraPos ) {
		
		if ( _grabbedBlock != null ) {
			_grabbedBlock.Shift( _camera.transform.TransformPoint( cameraPos ) );
		}
	}
	private void HandleRotateEvent ( float rotation ) {

	}
	private void HandleGrabEvent ( Vector2 cameraPos ) {
		
		if ( _grabbedBlock != null ) {
			_grabbedBlock.Grab();
			return;
		}

		var block = GetBlockAtCameraPos( cameraPos );
		
		if ( block != null ) {
			block.OnRelease += HandleReleaseEvent;
			block.Grab();

			_grabbedBlock = block;
		}
	}
	private void HandleReleaseEvent ( Block block ) {
		
		_grabbedBlock = null;
		Recalculate();
	}
	private void HandleGrabbedEvent ( Block block ) {

		Recalculate();
	}
	private void Recalculate () {

		_rootProjector.Project();
	}


	private Block GetBlockAtCameraPos ( Vector2 cameraPosition ) {
		
		RaycastHit hit;
		var worldSpace = ConvertCameraSpaceToWorld( cameraPosition );
		var forward = _camera.transform.forward;

        if (Physics.Raycast( worldSpace, forward, out hit, Mathf.Infinity )) {

        	var block = hit.collider.GetComponent<Block>();
        	if ( block != null ) {
	        	
	        	return block;
	    	}
        } 

        return null;
	}
	private Vector2 ConvertCameraSpaceToWorld ( Vector2 camera ) {
		
		return _camera.transform.TransformPoint( camera );
	}
}








	// private void GrabPiece ( Piece piece, Vector3 cameraPosition  ) {

	// 	_pieceInHand = piece;
	// 	_pieceInHand.GrabPoint = ConvertCameraSpaceToWorld( cameraPosition );
	// 	_pieceInHand.IsGrabbed = true;

	// 	Recalculate();
	// }	
	// private void ReleasePiece () {

	// 	_pieceInHand.IsGrabbed = false;
	// 	_pieceInHand = null;

	// 	Recalculate();
	// }
	// private Vector2 ConvertCameraSpaceToWorld ( Vector2 camera ) {
		
	// 	return _camera.transform.TransformPoint( camera );
	// }
	// private void Recalculate () {

	// 	_rootProjector.Project();
	// }
	
	// // **************** Event Handlers *******************

	// private void HandleOnGridClicked ( Vector2 cameraPosition ) {

	// 	if ( _isCarringPiece ){
	//     	ReleasePiece ();
	//     	return;
	//     }

	// 	RaycastHit hit;
	// 	var worldSpace = ConvertCameraSpaceToWorld( cameraPosition );
	// 	var forward = _camera.transform.forward;

 //        if (Physics.Raycast( worldSpace, forward, out hit, Mathf.Infinity )) {

 //        	var p = hit.collider.GetComponent<Piece>();
 //        	if ( p != null ) {
	        		
	//         	if ( !_isCarringPiece ){

	// 				GrabPiece ( p, cameraPosition );
	//     		}
	//     	}
 //        } 
	// }
	// private void HandleOnShift ( Vector2 cameraPosition ) {

	// 	if ( _isCarringPiece ) {
	// 		_pieceInHand.Shift( ConvertCameraSpaceToWorld( cameraPosition ) );
	// 	}
	// }
	// private void HandleOnRotate ( float rotation ) {

	// 	if ( _isCarringPiece ) {
	// 		_pieceInHand.Rotate( rotation );
	// 	}
	// }
