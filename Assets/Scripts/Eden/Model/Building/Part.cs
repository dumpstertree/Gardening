namespace Eden.Model.Building {
	
	abstract public class Part<T> {

		public string Name { get; protected set; }
		public char[,] Blocks { get; protected set; }
		public Stats.Gun BuilderStats { get; protected set; }

		protected Part( string name, char[,] blocks, Stats.Gun stats  ) {

			// Projectors 	⇡ ⇢ ⇣ ⇠
			// Recievers  	∪ ⊂ ∩ ⊃

			Name = name;
			Blocks = blocks;
			BuilderStats = stats; 
		}
	}
}