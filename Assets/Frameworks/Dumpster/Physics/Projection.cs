using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Dumpster.Physics {
	
	public struct Projection {

		public Ray Ray { get; }
		public bool IsColiding { get; }
		public float ProjectedLength { get; }
		public Vector3 ProjectedPoint { get; }

		public Projection ( Ray ray, LayerMask mask, float length, List<Collider> exclude ) {

			Ray = ray;

			IsColiding = false;	
			ProjectedPoint = ray.origin + ray.direction * length;
			ProjectedLength = length;

			var hits = UnityEngine.Physics.RaycastAll( ray ).OrderBy(h=>h.distance).ToArray();
			foreach ( RaycastHit hit in hits) { 

				if ( hit.collider == null ) {
					continue;
				}

				if ( exclude != null ) {
					if ( exclude.Contains( hit.collider ) ) {
						continue;
					}
				}
					
				if ( mask != (mask | (1 << hit.collider.gameObject.layer)) ) {
					continue;
				}


				if ( hit.distance <= ( length + .01f ) ) {
					
					IsColiding = true;
					
					if ( IsColiding ) {

						ProjectedPoint = hit.point;
						ProjectedLength  = hit.distance;
					}

					return;
				}
			}
		}
	}
}