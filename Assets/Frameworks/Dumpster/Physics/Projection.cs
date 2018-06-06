using UnityEngine;

namespace Dumpster.Physics {
	
	public struct Projection {

		public Ray Ray { get; }
		public bool IsColiding { get; }
		public float ProjectedLength { get; }
		public Vector3 ProjectedPoint { get; }
		public Vector3 Normal {
			get { return _hit.normal; }
		}
		private RaycastHit _hit;

		public Projection ( Ray ray, LayerMask mask, float length  ) {

			Ray = ray;

			IsColiding = false;	
			ProjectedPoint = ray.origin + ray.direction * length;
			ProjectedLength = length;


			if( UnityEngine.Physics.Raycast( ray, out _hit, Mathf.Infinity, mask )) {
				
				if ( _hit.distance <= ( length  + .01f ) ) {
					
					// set is colliding
					IsColiding = true;
					
					if ( IsColiding ) {

						ProjectedPoint = _hit.point;
						ProjectedLength  = _hit.distance;
					}
				}
			}
		}
	}
}