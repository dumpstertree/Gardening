using System.Collections.Generic;
using UnityEngine;
using Eden.Properties;

namespace Eden {

	public class Targeting : Dumpster.Core.Module {


		private float _depthWeight = 0.8f;
		public void RegisterTargetableForFrame( Targetable targetable ) {

			_targetableObjects.Add( targetable );
		}
		public Vector2 GetScreenOffsetFromClosestTargetable( Vector3 position, Vector2 point ) {

			var closest = GetClosestTargetableToPoint( position, point );
			var closestCamera = Camera.main.WorldToScreenPoint( closest.transform.position );
			var offset = point - new Vector2( closestCamera.x, closestCamera.y );

			return offset;
		}
		public Targetable GetClosestTargetableToPoint( Vector3 position, Vector2 point  ) {

			var closestDistance = Mathf.Infinity;
			Targetable closestTargetable = null;
			
			foreach ( Targetable t in  _lastFrameTargetableObjects ) {

				var distance = GetDistanceFromPoint( t, position, point );
				
				if ( distance < closestDistance ) {
					closestDistance = distance;
					closestTargetable = t;
				}
			}

			return closestTargetable;
		}


		protected override void OnInit () {

			_targetableObjects = new List<Targetable>();
		}

		private List<Targetable> _lastFrameTargetableObjects;
		private List<Targetable> _targetableObjects;
		
		private float GetDistanceFromPoint( Targetable t, Vector3 position, Vector2 point ) {

			var screenPos = Camera.main.WorldToScreenPoint( t.transform.position );
			var screenSpaceDistance = Vector2.Distance( point, screenPos );
			var worldSpaceDepth = Vector3.Distance( position, t.transform.position ) * _depthWeight;
			
			return screenSpaceDistance + worldSpaceDepth;
		}

		private void LateUpdate () {

			_lastFrameTargetableObjects = _targetableObjects;
			_targetableObjects = new List<Eden.Properties.Targetable>();
		}
	}
}