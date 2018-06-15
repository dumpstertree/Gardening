using System.Collections.Generic;
using Eden.UI.Elements.GunCrafting;

namespace Model {
	
	public class PartGraph {

		// *********** INIT ************

		readonly private int _numOfSlots;
		public PartGraph ( int numOfSlots ) {
			
			_numOfSlots = numOfSlots;
		}

		// ********** PUBLIC ***************

		public bool GetAvailable ( int x, int y ) {
			
			if ( x < 0 || y < 0 || x >= _slotGraph.GetLength(0) || y >= _slotGraph.GetLength(1) ){
				return false;
			}

			var slot = _slotGraph[ x, y ];

			if ( slot.Component == null && 
				 slot.Projectors.Count == 0 && 
				 slot.Recievers.Count == 0 ) {

				return true;
			}

			return false;
		}
		public void ClearGraph () {

			_slotGraph = new Slot[ _numOfSlots, _numOfSlots];
			
			for (int y = 0; y < _slotGraph.GetLength( 0 ); y++ ) {
				
				for (int x = 0; x < _slotGraph.GetLength( 1 ); x++ ) {
				
					_slotGraph[ x, y ] = new Slot(); 
				}
			}
		}


		public Part GetComponent( int x, int y ) {

			if ( x < 0 || y < 0 || x >= _slotGraph.GetLength(0) || y >= _slotGraph.GetLength(1) ){
				return null;
			}

			return _slotGraph[ x, y ].Component;
		}
		public void SetComponent( int x, int y, Part component ) {
			
			if ( x < 0 || y < 0 || x >= _slotGraph.GetLength(0) || y >= _slotGraph.GetLength(1) ){
				return;
			}

			_slotGraph[ x, y ].Component = component;
		}


		public List<Eden.UI.Elements.GunCrafting.Projector> GetProjectors( int x, int y ) {

			if ( x < 0 || y < 0 || x >= _slotGraph.GetLength(0) || y >= _slotGraph.GetLength(1) ){
				return new List<Eden.UI.Elements.GunCrafting.Projector>();
			}

			return _slotGraph[ x, y ].Projectors;
		}
		public void AddProjector( int x, int y, Eden.UI.Elements.GunCrafting.Projector projector ) {
			
			if ( x < 0 || y < 0 || x >= _slotGraph.GetLength(0) || y >= _slotGraph.GetLength(1) ){
				return;
			}
			
			_slotGraph[ x, y ].Projectors.Add( projector );
		}
		public void RemoveProjector( int x, int y, Eden.UI.Elements.GunCrafting.Projector projector ) {
			
			if ( x < 0 || y < 0 || x >= _slotGraph.GetLength(0) || y >= _slotGraph.GetLength(1) ){
				return;
			}
			
			if ( _slotGraph[ x, y ].Projectors.Contains( projector ) ) {
				_slotGraph[ x, y ].Projectors.Remove( projector ); 
			}
		}


		public List<Eden.UI.Elements.GunCrafting.Reciever> GetRecievers( int x, int y ) {

			if ( x < 0 || y < 0 || x >= _slotGraph.GetLength(0) || y >= _slotGraph.GetLength(1) ){
				return new List<Eden.UI.Elements.GunCrafting.Reciever>();
			}

			return _slotGraph[ x, y ].Recievers;
		}
		public void AddReciever( int x, int y, Eden.UI.Elements.GunCrafting.Reciever reciever ) {

			if ( x < 0 || y < 0 || x >= _slotGraph.GetLength(0) || y >= _slotGraph.GetLength(1) ){
				return;
			}

			_slotGraph[ x, y ].Recievers.Add( reciever );
		}
		public void RemoveReciever( int x, int y, Eden.UI.Elements.GunCrafting.Reciever reciever ) {
			
			if ( x < 0 || y < 0 || x >= _slotGraph.GetLength(0) || y >= _slotGraph.GetLength(1) ){
				return;
			}

			if ( _slotGraph[ x, y ].Recievers.Contains( reciever ) ) {
				_slotGraph[ x, y ].Recievers.Remove( reciever ); 
			}
		}


		// ************* PRIVATE ************

		private Slot[,] _slotGraph;

		// ************* DATA **************

		private class Slot {

			public Part Component;
			public List<Eden.UI.Elements.GunCrafting.Projector> Projectors;
			public List<Eden.UI.Elements.GunCrafting.Reciever> Recievers; 

			public Slot () {

				Projectors = new List<Eden.UI.Elements.GunCrafting.Projector>();
				Recievers = new List<Eden.UI.Elements.GunCrafting.Reciever>(); 
			}
		}
	}
}