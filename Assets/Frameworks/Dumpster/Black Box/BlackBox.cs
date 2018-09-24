using UnityEngine;

namespace Dumpster.Core.Life {

	public abstract class BlackBox<T> : MonoBehaviour where T : class {


		public delegate void InitEvent();
		public InitEvent OnInit;

		public delegate void RunEvent();
		public RunEvent OnRun;

		public delegate void StartupEvent();
		public StartupEvent OnStartup;

		public delegate void ShutdownEvent();
		public ShutdownEvent OnShutDown;

		public delegate void ThinkEvent();
		public ThinkEvent OnThink;

		public delegate void GetVisualvent( T visual );
		public GetVisualvent OnGetVisual;

		public T Visual {
			get{ return GetVisual(); }
		}
		public bool IsPowered {
			get{ return _isPowered; }
		}

		
		
		// **************** Private ********************

		protected bool _isPowered = true;

		private void Awake () {

			Init ();
			Run ();
		}
		
		private void Update () {

			if( _isPowered ){
				Think();
			}
		}
		
		private void FireOnInit () {

			if ( OnInit != null ) {
				OnInit ();
			}
		}

		private void FireOnRun () {
			
			if ( OnRun != null ) {
				OnRun ();
			}
		}
		
		private void FireShutdownEvent() {

			if ( OnShutDown != null ) {
				OnShutDown ();
			}
		}

		private void FireStartupEvent () {
			
			if ( OnStartup != null ) {
				OnStartup ();
			}
		}

		private void FireGetVisualEvent ( T visual ) {

			if ( OnGetVisual != null ) {
				OnGetVisual( visual );
			}
		}
		private void FireThinkEvent () {

			if ( OnThink != null ) {
				OnThink ();
			}
		}


		// **************** Virtual ********************
		

		protected virtual void Init (){

			foreach ( Chip<T> chip in GetComponentsInChildren<Chip<T>>() ){
				chip.Install( this );
			}

			FireOnInit ();
			FireStartupEvent ();
		}
		
		protected virtual void Run () {

			FireOnRun ();
		}
		
		protected virtual void Think () {

			FireThinkEvent ();
		}
		
		protected virtual void Shutdown () {

			_isPowered = false;

			FireShutdownEvent();
		}
		
		protected virtual void Reboot () {

			_isPowered = true;

			FireStartupEvent();
		}		

		
		// **************** Abstract ********************

		protected abstract T GetVisualInstance ();

		private T GetVisual () {

			var visual = GetVisualInstance();
			FireGetVisualEvent( visual );

			return visual;
		}
	}
}