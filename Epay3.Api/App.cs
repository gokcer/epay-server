using System;
using DevExpress.Xpo;

namespace Epay3.Api { 
	
	public class App : XPObject {
		public App() : base() {
			// This constructor is used when an object is loaded from a persistent storage.
			// Do not place any code here.
		}

		public App(Session session) : base(session) {
			// This constructor is used when an object is loaded from a persistent storage.
			// Do not place any code here.
		}

		public override void AfterConstruction() { 
			base.AfterConstruction(); 
			// Place here your initialization code.
		}
	}

}