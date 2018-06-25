using System.Collections.Generic;
using UnityEngine;
using Eden.Properties;

namespace Eden {

	public class Targeting : Dumpster.Core.Module {


		private float _depthWeight = 0.8f;
		public void RegisterTargetableForFrame( Targetable targetable ) {

			_targetableObjects.Add( targetable );
		}
		public Vector2 GetOffsetFromPointToCloset( Vector3 position, Vector2 point ) {

			var closestDistance = Mathf.Infinity;
			var closestTargetable = Vector2.zero;
			
			foreach ( Targetable t in  _lastFrameTargetableObjects ) {

				var screenPos = Camera.main.WorldToScreenPoint( t.transform.position );
				var screenSpaceDistance = Vector2.Distance( point, screenPos );
				var worldSpaceDepth = Vector3.Distance( position, t.transform.position ) * _depthWeight;
				
				var distance = screenSpaceDistance + worldSpaceDepth;
				
				if ( distance < closestDistance ) {
					closestDistance = distance;
					closestTargetable = screenPos;
				}
			}

			var offset = point - closestTargetable;

			return offset;
		}


		protected override void OnInit () {

			_targetableObjects = new List<Eden.Properties.Targetable>();
		}

		private List<Targetable> _lastFrameTargetableObjects;
		private List<Targetable> _targetableObjects;
		
		private void LateUpdate () {

			_lastFrameTargetableObjects = _targetableObjects;
			_targetableObjects = new List<Eden.Properties.Targetable>();
		}
	}
}