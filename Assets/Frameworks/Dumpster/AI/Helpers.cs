using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dumpster.AI {
	
	public static class Helpers {

		public static Vector3 GetXZDirectionTowardsObject( Vector3 start, Vector3 other, float offset = 0f ) {
			
			var rawDirection = (start - other).normalized;
			rawDirection.y = 0;
			var xzDir = rawDirection.normalized;

			if ( !Mathf.Approximately( offset, 0f ) ) {
				xzDir = Quaternion.AngleAxis( offset, Vector3.up ) * xzDir;
			}

			return xzDir;
		}
		public static Vector3 GetXZDirectionAwayFromObject( Vector3 start, Vector3 other, float offset = 0f ) {
			
			var rawDirection = (start - other).normalized;
			rawDirection.y = 0;
			var xzDir = rawDirection.normalized;

			if ( !Mathf.Approximately( offset, 0f ) ) {
				xzDir = Quaternion.AngleAxis( offset, Vector3.up ) * xzDir;
			}

			return xzDir;
		}
	}
}