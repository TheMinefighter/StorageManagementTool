using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace StorageManagementCore.Backend {
	//The following class is mostly copied from https://stackoverflow.com/a/41907561/6730162 last access 20.06.2018
	public class Map<T1, T2> : ReadOnlyMap<T1,T2> {

		public new T1 this[T2 backward] {
			get => _backward[backward];
			set => _backward[backward] = value;
		}

		public new T2 this[T1 forward] {
			get => _forward[forward];
			set => _forward[forward] = value;
		}

		public Map() {
			Forward = new Indexer<T1, T2>(_forward);
			Backward = new Indexer<T2, T1>(_backward);
		}

		public void Add(T1 t1, T2 t2) {
			_forward.Add(t1, t2);
			_backward.Add(t2, t1);
		}

		public void Clear() {
			_forward.Clear();
			_backward.Clear();
		}

		public bool Remove(T1 forward) {
			if (!_forward.ContainsKey(forward)) {
				return false;
			}

			T2 v2 = _forward[forward];
			_forward.Remove(forward);
			_backward.Remove(v2);
			return true;
		}

		public bool Remove(T2 backward) {
			if (!_backward.ContainsKey(backward)) {
				return false;
			}

			T1 v1 = _backward[backward];
			_backward.Remove(backward);
			_forward.Remove(v1);
			return true;
		}

		public class Indexer<T3, T4> : ReadOnlyIndexer<T3,T4> {

			public new T4 this[T3 index] {
				get => _dictionary[index];
				set => _dictionary[index] = value;
			}

			public Indexer(Dictionary<T3, T4> dictionary) : base(dictionary) {}

		}
	}
}