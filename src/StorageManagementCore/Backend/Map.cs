using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace StorageManagementCore.Backend {
	//The following class is mostly copied from https://stackoverflow.com/a/41907561/6730162 last access 20.06.2018
	public class Map<T1, T2> : IEnumerable<KeyValuePair<T1, T2>>, ISerializable {
		private readonly Dictionary<T2, T1> _backward = new Dictionary<T2, T1>();
		private readonly Dictionary<T1, T2> _forward = new Dictionary<T1, T2>();

		public Indexer<T1, T2> Forward { get; }
		public Indexer<T2, T1> Backward { get; }

		public T1 this[T2 backward] {
			get => _backward[backward];
			set => _backward[backward] = value;
		}

		public T2 this[T1 forward] {
			get => _forward[forward];
			set => _forward[forward] = value;
		}

		public Map() {
			Forward = new Indexer<T1, T2>(_forward);
			Backward = new Indexer<T2, T1>(_backward);
		}


		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public IEnumerator<KeyValuePair<T1, T2>> GetEnumerator() => _forward.GetEnumerator();

		public void GetObjectData(SerializationInfo info, StreamingContext context) {
			_forward.GetObjectData(info, context);
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

		public bool Contains(T1 key) => _forward.ContainsKey(key);
		public bool Contains(T2 value) => _backward.ContainsKey(value);

		public class Indexer<T3, T4> {
			private readonly Dictionary<T3, T4> _dictionary;

			public T4 this[T3 index] {
				get => _dictionary[index];
				set => _dictionary[index] = value;
			}

			public Indexer(Dictionary<T3, T4> dictionary) => _dictionary = dictionary;

			public bool Contains(T3 key) => _dictionary.ContainsKey(key);
		}
	}
}