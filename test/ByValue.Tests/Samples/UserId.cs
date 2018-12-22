using System;

namespace ByValue
{
    public class UserId : SingleValueObject<int>
    {
        public UserId(int value)
            : base(value)
        {
            if (value == 0)
                throw new ArgumentOutOfRangeException(nameof(value));
        }

        public static explicit operator UserId(int value)
        {
            return new UserId(value);
        }

        public static implicit operator int(UserId userId)
        {
            return userId == null ? 0 : userId.Value;
        }

        public static implicit operator int? (UserId userId)
        {
            return userId == null ? (int?)null : userId.Value;
        }
    }

    public class DerivedUserId : UserId
    {
        public DerivedUserId(int value, string name)
            : base(value)
        {
            Name = name;
        }

        public string Name { get; }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj))
                return false;

            var other = obj as DerivedUserId;
            return other != null
                && Name == other.Name;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class DeletedUserId : UserId
    {
        public DeletedUserId(int value)
            : base(value)
        {
        }
    }

    public class VipUserId : UserId
    {
        public VipUserId(int value)
            : base(value)
        {
        }
    }
}