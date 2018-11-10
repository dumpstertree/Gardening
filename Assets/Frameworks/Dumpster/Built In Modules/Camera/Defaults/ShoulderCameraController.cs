using UnityEngine;

namespace Dumpster.BuiltInModules.Camera.Defaults {

	public class ShoulderCameraController : Dumpster.Core.BuiltInModules.CameraController {

		
		// **************** Public ******************

		public float HorizontalInput {
			get; set;
		}
		public float VericalInput {
			get; set;
		}
		public bool IsStrafing {
			get; set;
		}

		public override void Control ( Transform cameraInstance, Transform focus ){


			RotateHorizontal( cameraInstance, focus, HorizontalInput );
			RotateVertical( cameraInstance, focus, VericalInput );


			if ( TargetOutOfRange( cameraInstance, focus )) {

				var right = Vector3.Cross( cameraInstance.forward, Vector3.up ).normalized; 
				var forward = Vector3.Cross( right, Vector3.up ).normalized;
				
				var direction =  Quaternion.AngleAxis( _verticalRot, right ) * Quaternion.AngleAxis( _horizontalRot, Vector3.up) * Vector3.forward;
				var targetPos =  focus.position + (direction * _targetDistance);

				var collisionTargetPos = AccountForCollision( _minDistanceToCollider, _targetDistance, focus.position, targetPos );
				
				cameraInstance.position = Vector3.Lerp( cameraInstance.position, collisionTargetPos, _lerpSpeed );
				cameraInstance.rotation = Quaternion.Slerp( cameraInstance.rotation, Quaternion.LookRotation( -direction ), _lerpSpeed );
			}
		}

		
		// **************** Private ******************
		

		private float _targetDistance {
			get{ return IsStrafing ? _strafeDistance : _followDistance; }
		}
		private float _lerpSpeed {
			get { return IsStrafing ? _strafeLerpSpeed : _followLerpSpeed; }
		}
		private float _horizontalSensitivity {
			get { return IsStrafing ? _horizontalStrafingSensitivity : _horizontalFollowSensitivity; }
		}
		private float _verticalSensitivity {
			get { return IsStrafing ? _verticalStrafingSensitivity : _verticalFollowSensitivity; }
		}


		[Header( "Limits" )]
		[SerializeField] private float _minVerticalRotation = -80f;
		[SerializeField] private float _maxVerticalRotation =  80f;

		[SerializeField] private float _followDistance = 6f;
		[SerializeField] private float _strafeDistance = 3f;

		[Header( "Sensitivity" )]
		[SerializeField] private float _horizontalStrafingSensitivity = 55;
		[SerializeField] private float _verticalStrafingSensitivity = 40;
		[SerializeField] private float _horizontalFollowSensitivity = 110;
		[SerializeField] private float _verticalFollowSensitivity = 80;

		[Header( "Lerping" )]
		[SerializeField] private float _followLerpSpeed = 0.3f;
		[SerializeField] private float _strafeLerpSpeed = 0.6f;
		
		[Header( "Raycast" )]
		[SerializeField] private float _minDistanceToCollider = 1f;

		
		private const float OUT_OF_RANGE = 0.01f;


		private float _verticalRot;
		private float _horizontalRot;


		private void RotateHorizontal ( Transform cameraInstance, Transform cameraTarget, float horizontalInput ) {

			// rotate around the target
			// cameraInstance.RotateAround( cameraTarget.position, Vector3.up, horizontalInput * (_horizontalSensitivity * Time.fixedDeltaTime) );

			_horizontalRot += horizontalInput * (_horizontalSensitivity * Time.fixedDeltaTime );
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
		private float DistanceToTarget ( Transform cameraInstance, Transform focus ){
			
			return Vector3.Distance( cameraInstance.position, focus.position );
		}
		private bool TargetOutOfRange ( Transform cameraInstance, Transform focus ){
			
			return !(DistanceToTarget( cameraInstance, focus ) > _targetDistance + OUT_OF_RANGE && DistanceToTarget( cameraInstance, focus ) < _targetDistance - OUT_OF_RANGE);
		}
		private Vector3 AccountForCollision( float minDistanceToCollider, float distanceFromCamera, Vector3 startPos, Vector3 targetPosition ) {

			var dir = targetPosition - startPos;

			RaycastHit hit;
			if ( Physics.Raycast( startPos, dir, out hit, distanceFromCamera ) ) {

				return hit.point + (-dir * minDistanceToCollider);
			}

			return targetPosition;
		} 
	}
}
