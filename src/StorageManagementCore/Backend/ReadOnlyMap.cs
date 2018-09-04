using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace StorageManagementCore.Backend {
	public class ReadOnlyMap<T1, T2> : IEnumerable<KeyValuePair<T1, T2>>, ISerializable 
		{
		protected readonly Dictionary<T2, T1> _backward = new Dictionary<T2, T1>();
		protected readonly Dictionary<T1, T2> _forward = new Dictionary<T1, T2>();

		public ReadOnlyIndexer<T1, T2> Forward { get; protected set; }
		public ReadOnlyIndexer<T2, T1> Backward { get; protected set; }

		public T1 this[T2 backward] {
			get => _backward[backward];
			set => _backward[backward] = value;
		}

		public T2 this[T1 forward] {
			get => _forward[forward];
			set => _forward[forward] = value;
		}

		public ReadOnlyMap() {
			Forward = new ReadOnlyIndexer<T1, T2>(_forward);
			Backward = new ReadOnlyIndexer<T2, T1>(_backward);
		}

			public ReadOnlyMap(IEnumerable<KeyValuePair<T1, T2>> src): this() {
				foreach (KeyValuePair<T1,T2> keyValuePair in src) {
					_forward.Add(keyValuePair.Key,keyValuePair.Value);
					_backward.Add(keyValuePair.Value,keyValuePair.Key);
				}
			}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public IEnumerator<KeyValuePair<T1, T2>> GetEnumerator() => _forward.GetEnumerator();

		public void GetObjectData(SerializationInfo info, StreamingContext context) {
			_forward.GetObjectData(info, context);
		}
		public bool Contains(T1 key) => _forward.ContainsKey(key);
		public bool Contains(T2 value) => _backward.ContainsKey(value);

		public class ReadOnlyIndexer<T3, T4> {
			protected readonly Dictionary<T3, T4> _dictionary;

			public T4 this[T3 index] {
				get => _dictionary[index];
				}

			public ReadOnlyIndexer(Dictionary<T3, T4> dictionary) => _dictionary = dictionary;

			public bool Contains(T3 key) => _dictionary.ContainsKey(key);
		}
	}
	}
