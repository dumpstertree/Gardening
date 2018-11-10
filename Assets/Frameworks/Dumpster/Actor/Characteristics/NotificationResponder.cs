using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dumpster.Characteristics {

	public abstract class NotificationResponder : Dumpster.Core.Characteristic {

		public List<string> ListeningToNotifications {
			get{ return _listeningToNofications; }
		}
		public void SetListeningToNofications ( List<string> listeningToNotifcations ) {

			_listeningToNofications = listeningToNotifcations;
		}
		public override void RecieveNotification( string notification ) {

			if ( _listeningToNofications.Contains( notification ) ) {
				Respond ();
			}
		}


		[HideInInspector] [SerializeField] private List<string> _listeningToNofications = new List<string>();

		
		protected abstract void Respond ();
	}
}