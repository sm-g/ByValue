﻿using System.Collections.Generic;
using System.ComponentModel;

namespace ByValue
{
    public class OptionsBuilder<T>
    {
        private IEqualityComparer<T>? _comparer;
        private Ordering _ordering = Ordering.NotStrict;

        public OptionsBuilder<T> StrictOrdering()
        {
            _ordering = Ordering.Strict;
            return this;
        }

        public OptionsBuilder<T> UseComparer(IEqualityComparer<T> comparer)
        {
            _comparer = comparer ?? throw new System.ArgumentNullException(nameof(comparer));
            return this;
        }

        public Options<T> Build()
        {
            return new Options<T>(_ordering == Ordering.Strict, _comparer);
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