using System.Collections;
using System.Collections.Generic;

namespace StorageManagementCore.Backend {
	//The following class is mostly copied from https://stackoverflow.com/a/41907561/6730162 last access 20.06.2018
	public class Map<T1, T2> : IEnumerable<KeyValuePair<T1, T2>>
	{
		private readonly Dictionary<T1, T2> _forward = new Dictionary<T1, T2>();
		private readonly Dictionary<T2, T1> _backward = new Dictionary<T2, T1>();

		public Map()
		{
			Forward = new Indexer<T1, T2>(_forward);
			Backward = new Indexer<T2, T1>(_backward);
		}

		public Indexer<T1, T2> Forward { get; }
		public Indexer<T2, T1> Backward { get; }

		public void Add(T1 t1, T2 t2)
		{
			_forward.Add(t1, t2);
			_backward.Add(t2, t1);
		}
		public T1 this[T2 backward] {
			get => _backward[backward];
			set => _backward[backward] = value;
		}
		public T2 this[T1 forward] {
			get => _forward[forward];
			set => _forward[forward] = value;
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public IEnumerator<KeyValuePair<T1, T2>> GetEnumerator() => _forward.GetEnumerator();

		public class Indexer<T3, T4>
		{
			private readonly Dictionary<T3, T4> _dictionary;

			public Indexer(Dictionary<T3, T4> dictionary) => _dictionary = dictionary;

			public T4 this[T3 index]
			{
				get => _dictionary[index];
				set => _dictionary[index] = value;
			}

			public bool Contains(T3 key) => _dictionary.ContainsKey(key);
		}
	}
}