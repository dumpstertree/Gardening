using Eden.Interactable;
using Eden.Life;

namespace Eden.Model.Life {
	
	public class Visual {

		
		// Alignment
		public Alignment Alignment { get; set; }

		
		// Health
		public int CurrentHealth { get; set; }
		public int MaxHealth { get; set; }


		// Equipment
		public int EquipedItemNumber { get; set; }
		public Item CurrentItemInHand { get; set; }


		// Interaction
		public InteractableObject InteractingWith { get; set; }
	}
}