using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;

namespace Wordki.Helpers {
  [Serializable]
  public class ObservableDictionary<TKey, TValue> : INotifyPropertyChanged, IDictionary<TKey, TValue> {
    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string pPropertyName) {
      PropertyChangedEventHandler lHandler = PropertyChanged;
      if (lHandler != null) {
        lHandler(this, new PropertyChangedEventArgs(pPropertyName));
      }
    }
    #endregion

    private List<TKey> _keys;
    private List<TValue> _values;

    public ObservableDictionary() {
      _keys = new List<TKey>();
      _values = new List<TValue>();
    }

    public bool Add(TKey pKey) {
      return Add(pKey, default(TValue));
    }

    public bool Add(TKey pKey, TValue pValue) {
      if (_keys.Contains(pKey))
        return false;
      _keys.Add(pKey);
      _values.Add(pValue);
      return true;
    }

    public bool Remove(TKey pKey) {
      if (!_keys.Contains(pKey))
        return false;
      int lId = _keys.IndexOf(pKey);
      _keys.Remove(pKey);
      _values.RemoveAt(lId);
      return true;
    }

    public bool Set(TKey pKey, TValue pValue) {
      if (!_keys.Contains(pKey))
        return false;
      int lId = _keys.IndexOf(pKey);
      _values[lId] = pValue;
      OnPropertyChanged(Binding.IndexerName);
      return true;
    }

    public TValue this[TKey pKey] {
      get {
        if (!_keys.Contains(pKey))
          throw new KeyNotFoundException();
        int lId = _keys.IndexOf(pKey);
        return _values[lId];
      }
      set {
        if (!_keys.Contains(pKey))
          throw new KeyNotFoundException();
        int lId = _keys.IndexOf(pKey);
        if (!_values[lId].Equals(value)) {
          _values[lId] = value;
          OnPropertyChanged(Binding.IndexerName);
        }
      }
    }

    public TValue this[int i] {
      get {
        if (_keys.Count - 1 < i) {
          throw new IndexOutOfRangeException(String.Format("{0} < {1}", _keys.Count - 1, i));
        }
        return _values[i];
      }
      set {
        if (_keys.Count - 1 < i) {
          throw new IndexOutOfRangeException(String.Format("{0} < {1}", _keys.Count - 1, i));
        }
        if (_values[i].Equals(value))
          return;
        _values[i] = value;
        OnPropertyChanged(Binding.IndexerName);
      }
    }

    void IDictionary<TKey, TValue>.Add(TKey key, TValue value) {
      throw new NotImplementedException();
    }

    public bool ContainsKey(TKey key) {
      if (!_keys.Contains(key))
        return false;
      return true;
    }

    public ICollection<TKey> Keys {
      get { return _keys; }
    }

    public bool TryGetValue(TKey key, out TValue value) {
      if (!_keys.Contains(key)) {
        value = default(TValue);
        return false;
      }
      int lId = _keys.IndexOf(key);
      value = _values[lId];
      return true;
    }

    public ICollection<TValue> Values {
      get { return _values; }
    }

    public void Add(KeyValuePair<TKey, TValue> item) {
      throw new NotImplementedException();
    }

    public void Clear() {
      _keys.Clear();
      _values.Clear();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item) {
      throw new NotImplementedException();
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
      throw new NotImplementedException();
    }

    public int Count {
      get { return _keys.Count; }
    }

    public bool IsReadOnly {
      get { throw new NotImplementedException(); }
    }

    public bool Remove(KeyValuePair<TKey, TValue> item) {
      throw new NotImplementedException();
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
      throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator() {
      throw new NotImplementedException();
    }
  }
}
