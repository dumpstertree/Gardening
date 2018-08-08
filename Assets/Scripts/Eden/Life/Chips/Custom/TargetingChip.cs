using System;
using UnityEngine;

namespace Eden.Life.Chips {
	
	public class TargetingChip : MonoBehaviour {

		[SerializeField] private Transform _itemToAssistInX;
		[SerializeField] private Transform _itemToAssistInY;
		
		private float _pullPower = .01f;
		private float _pullDistance = 150;

		private void Update () {
    			
			// var point = new Vector3( Screen.width/2f, Screen.height/2f, 0 );
			// var offset = EdensGarden.Instance.Targeting.GetScreenOffsetFromClosestTargetable( transform.position, point );
			// var distance = Vector2.Distance( point, point + new Vector3( offset.x, offset.y, 0 ) );

			// if ( distance < _pullDistance ) {

			// 	var distanceMult = 1 - (distance/_pullDistance);

			// 	var up = Vector3.up;
			// 	var forward = _itemToAssistInY.forward;
			// 	var left = -Vector3.Cross( up, forward );

			// 	_itemToAssistInX.rotation = Quaternion.AngleAxis( (-offset.y * _pullPower * distanceMult), left ) * _itemToAssistInX.rotation; 
			// 	_itemToAssistInY.rotation = Quaternion.AngleAxis( (-offset.x * _pullPower * distanceMult), up ) * _itemToAssistInY.rotation; 
			// }
		}
	}
}