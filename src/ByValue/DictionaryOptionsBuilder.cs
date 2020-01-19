using System.Collections.Generic;
using System.ComponentModel;

namespace ByValue
{
    public class DictionaryOptionsBuilder<TKey, TValue>
    {
        private IEqualityComparer<TKey>? _keysComparer;
        private IEqualityComparer<TValue>? _valuesComparer;

        public DictionaryOptionsBuilder<TKey, TValue> UseComparer(IEqualityComparer<TKey> keysComparer)
        {
            _keysComparer = keysComparer ?? throw new System.ArgumentNullException(nameof(keysComparer));
            return this;
        }

        public DictionaryOptionsBuilder<TKey, TValue> UseComparer(IEqualityComparer<TValue> valuesComparer)
        {
            _valuesComparer = valuesComparer ?? throw new System.ArgumentNullException(nameof(valuesComparer));
            return this;
        }

        public DictionaryOptions<TKey, TValue> Build()
        {
            return new DictionaryOptions<TKey, TValue>(_keysComparer, _valuesComparer);
        }

        #region Hidden System.Object members

        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        /// <returns> A string that represents the current object. </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString() => base.ToString();

        /// <summary>
        ///     Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj"> The object to compare with the current object. </param>
        /// <returns> true if the specified object is equal to the current object; otherwise, false. </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => base.Equals(obj);

        /// <summary>
        ///     Serves as the default hash function.
        /// </summary>
        /// <returns> A hash code for the current object. </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => base.GetHashCode();

        #endregion Hidden System.Object members
    }
}