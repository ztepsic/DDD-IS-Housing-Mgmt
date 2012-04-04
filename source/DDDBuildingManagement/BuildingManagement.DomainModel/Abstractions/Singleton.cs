using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildingManagement.DomainModel.Abstractions {
	/// <summary>
	/// Singleton pattern
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Singleton<T> where T : class, new() {

		private Singleton(){}

		// lazy instatiation
		static class SingletonCreator {
			internal static readonly T Instance = new T();
		}

		public T Instance {
			get { return SingletonCreator.Instance; }
		}

	}
}
