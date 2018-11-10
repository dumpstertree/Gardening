using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Dumpster.Core;
using Dumpster.Characteristics;

namespace Dumpster.Core.Editor {

	[CustomEditor( typeof( NotificationResponder ), true )]
	public class NotificationResponderInspector : UnityEditor.Editor {

		private NotificationResponder _notificationResponder {
			get { return target as NotificationResponder; }
		}

		public override void OnInspectorGUI() {

			var actor = _notificationResponder.GetComponentInParent<Actor>();
			if ( actor == null ) {
				return;
			}

			var allNotifications = actor.GetNotifications();
			if ( allNotifications.Count > 0 ) {

				EditorGUILayout.Space();
				EditorGUILayout.LabelField( "Respond To Events", EditorStyles.boldLabel );

				var bitmaskValue = GetBitmaskValue( allNotifications, _notificationResponder.ListeningToNotifications );
				var newBitmaskValue = EditorGUILayout.MaskField( "Notifications", bitmaskValue, allNotifications.ToArray() );
				_notificationResponder.SetListeningToNofications( GetNotificationsValues( newBitmaskValue, allNotifications ) );
			}

			EditorGUILayout.Space();
			DrawDefaultInspector();
		}

		private string[] GetOptions ( List<string> notifications ) {
			
			var options = new string[ notifications.Count ];
			
			for( int i=0; i<options.Length; i++ ){
				options[ i ] = ( notifications[ i ] );
			}

			return options;
		}
		private int GetBitmaskValue ( List<string> allNotifications, List<string> listeningToNotifications ) {

			var mask = 0;
			for( int i=0; i<allNotifications.Count; i++ ) {
				
				var n = allNotifications[ i ];
				if ( listeningToNotifications.Contains( n ) ) {
					mask |= 1 << i;
				}
			}

			return mask;
		}
		private List<string> GetNotificationsValues ( int mask, List<string> allNotifications ) {

			var notifications = new List<string>();
			for( int i=0; i<allNotifications.Count; i++ ) {
				if ((mask & (1 << i)) > 0) {
					notifications.Add( allNotifications[ i ] );
				}
			}

			return notifications;
		}
	}
}