﻿using UnityEngine;
using Dumpster.Core.BuiltInModules.Input;

public class CameraController : Dumpster.Core.BuiltInModules.CameraController, IInputReciever<Eden.Input.Package> {
	
	
	// **************** Public ******************
	
	public void RecieveInput ( Eden.Input.Package package ) {

		_horizontal = package.RightAnalog.Horizontal;
		_vertical = package.RightAnalog.Vertical;

		if ( package.BackLeft.Bumper_Down ) {
			_strafing= true;
		}
		if ( package.BackLeft.Bumper_Up ) {
			_strafing= false;
		}
	}
	public void EnteredInputFocus () {
	}
	public void ExitInputFocus () {
	}

	
	// **************** Private ******************
	
	private float _distanceToTarget ( Transform cameraInstance, Transform focus ){
		return Vector3.Distance( cameraInstance.position, focus.position );
	}
	private bool _targetOutOfRange ( Transform cameraInstance, Transform focus ){
		return !(_distanceToTarget( cameraInstance, focus ) > _targetDistance + OUT_OF_RANGE && _distanceToTarget( cameraInstance, focus ) < _targetDistance - OUT_OF_RANGE);
	}
	private float _targetDistance {
		get{ return _strafing ? _strafeDistance : _followDistance; }
	}
	private float _lerpSpeed {
		get { return _strafing ? _strafeLerpSpeed : _followLerpSpeed; }
	}


	[Header( "Limits" )]
	[SerializeField] private float _minVerticalRotation = -80f;
	[SerializeField] private float _maxVerticalRotation =  80f;

	[SerializeField] private float _followDistance = 6f;
	[SerializeField] private float _strafeDistance = 3f;

	[Header( "Sensitivity" )]
	[SerializeField] private float _horizontalSensitivity = 180f;
	[SerializeField] private float _verticalSensitivity = 100f;

	[Header( "Lerping" )]
	[SerializeField] private float _followLerpSpeed = 0.3f;
	[SerializeField] private float _strafeLerpSpeed = 0.6f;
	
	[Header( "Raycast" )]
	[SerializeField] private float _minDistanceToCollider = 1f;
	
	private const float OUT_OF_RANGE = 0.01f;

	
	private float _horizontal;
	private float _vertical;
	private float _verticalRot;
	private bool _strafing;

	private void Start () {

		EdensGarden.Instance.Input.RegisterToInputLayer( EdensGarden.Constants.InputLayers.Player, this );
		EdensGarden.Instance.Input.RequestInput( EdensGarden.Constants.InputLayers.Player );
	}
	
	public override void Control ( Transform cameraInstance, Transform focus ){

		RotateHorizontal( cameraInstance, focus, _horizontal );
		RotateVertical( cameraInstance, focus, _vertical );

		if ( _targetOutOfRange( cameraInstance, focus )) {

			var right = Vector3.Cross( cameraInstance.forward, Vector3.up ).normalized; 
			var forward = Vector3.Cross( right, Vector3.up ).normalized;
			var direction = Quaternion.AngleAxis( _verticalRot, right ) * forward;
			var targetPos =  focus.position + (direction * _targetDistance);

			var collisionTargetPos = AccountForCollision( _minDistanceToCollider, _targetDistance, focus.position, targetPos );
			
			cameraInstance.position = Vector3.Lerp( cameraInstance.position, collisionTargetPos, _lerpSpeed );
			cameraInstance.rotation = Quaternion.Slerp( cameraInstance.rotation, Quaternion.LookRotation( -direction ), _lerpSpeed );
		}
	}
	
	private Vector3 AccountForCollision( float minDistanceToCollider, float distanceFromCamera, Vector3 startPos, Vector3 targetPosition ) {

		var dir = targetPosition - startPos;

		RaycastHit hit;
		if ( Physics.Raycast( startPos, dir, out hit, distanceFromCamera ) ) {

			return hit.point + (-dir * minDistanceToCollider);
		}

		return targetPosition;
	} 

	private void RotateHorizontal ( Transform cameraInstance, Transform cameraTarget, float horizontalInput ) {

		// rotate around the target
		cameraInstance.RotateAround( cameraTarget.position, Vector3.up, horizontalInput * (_horizontalSensitivity * Time.fixedDeltaTime) );
	}
	private void RotateVertical ( Transform cameraInstance, Transform cameraTarget, float verticalInput ) {
		
		// project what the next angle will be
		var projectedRot = _verticalRot + (verticalInput * (_verticalSensitivity * Time.fixedDeltaTime) );
		
		// as long as the projected angle fits in the rules do it
		if ( projectedRot < _maxVerticalRotation && projectedRot > _minVerticalRotation ) {

			// save new rotation
			_verticalRot = projectedRot;
		} 
	}
}
