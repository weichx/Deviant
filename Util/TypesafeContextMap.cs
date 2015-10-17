using System;
using System.Collections.Generic;

public class TypeSafeContextMap {

    private readonly Dictionary<Type, object> values;

    public TypeSafeContextMap() {
        values = new Dictionary<Type, object>();
    }

    public void Put<T>(Key<T> key, T value) {
        values.Add(key.GetType(), value);
    }

    public T Get<T>(Key<T> key) {
        return (T)values[key.GetType()];
    }

    public struct Key<T> {
        public string id;
        public Type type;

        //public Key(string id) {
        //    this.id = id;
        //    this.type = T;
        //}
    }
}