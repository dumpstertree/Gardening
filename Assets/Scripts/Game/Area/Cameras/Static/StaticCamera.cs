using UnityEngine;

public class StaticCamera : CameraSystem {
	
	[SerializeField] private float _lerpSpeed = 0.5f;
	[SerializeField] private StaticCameraPoint _startPoint;
	[SerializeField] private Camera _cameraPrefab;

	private Transform _cameraInstance;
	private StaticCameraPoint _targetPoint;

	protected override void OnInit () {

		// crate start point
		_targetPoint = _startPoint;

		// create camera
		_cameraInstance = Instantiate( _cameraPrefab, transform ).transform;
		_cameraInstance.transform.position = _targetPoint.TargetCameraPosition;
		_cameraInstance.transform.rotation = _targetPoint.TargetCameraRotation;

		// listen for new range enter events
		var points = GetComponentsInChildren<StaticCameraPoint>();
		foreach ( StaticCameraPoint p in points ) {
			var point = p;
			p.OnEnterRange += () => { EnterNewRange( point ); };
		}
	}

	private void EnterNewRange ( StaticCameraPoint point ) {

		_targetPoint = point;
	}

	private void Update () {

		if ( _cameraInstance != null ) {
		
			_cameraInstance.transform.position = Vector3.Lerp( 
				_cameraInstance.transform.position , 
				_targetPoint.TargetCameraPosition, 
				_lerpSpeed );

			_cameraInstance.transform.rotation = Quaternion.Slerp( 
				_cameraInstance.transform.rotation , 
				_targetPoint.TargetCameraRotation, 
				_lerpSpeed );

		}
	}
}
