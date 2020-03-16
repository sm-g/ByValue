# ByValue

[![Build status](https://ci.appveyor.com/api/projects/status/k6nmr1mdixf7xho6/branch/master?svg=true)](https://ci.appveyor.com/project/sm-g/byvalue/branch/master) [![Build Status](https://travis-ci.org/sm-g/ByValue.svg?branch=master)](https://travis-ci.org/sm-g/ByValue) [![NuGet](http://img.shields.io/nuget/v/ByValue.svg)](https://www.nuget.org/packages/ByValue/)

This library helps to create ValueObjects with properly implemented equality behavior:

1. Provides base `ValueObject` class.
2. Gives extension `ByValue()` for comparing collections with semantic of ValueObject (`IReadOnlyCollection`, `IReadOnlyDictionary`, `IDictionary` and `ISet` are supported).

## Example

```cs
public class MultilineAddress : ValueObject
{
    public MultilineAddress(IReadOnlyCollection<string> addressLines, string city, string postalCode)
    {
        AddressLines = addressLines ?? throw new ArgumentNullException(nameof(addressLines));
        City = city ?? throw new ArgumentNullException(nameof(city));
        PostalCode = postalCode ?? throw new ArgumentNullException(nameof(postalCode));

        if (addressLines.Count < 1 || addressLines.Count > 3)
            throw new ArgumentOutOfRangeException(nameof(addressLines), addressLines, "Multiline address should have from 1 to 3 address lines");
    }

    public IReadOnlyCollection<string> AddressLines { get; }
    public string City { get; }
    public string PostalCode { get; }

    // here you should return values, which will be used in Equals() and GetHashCode()
    protected override IEnumerable<object> Reflect()
    {
        // by default collections compared with not strcit ordering
        yield return AddressLines.ByValue(Ordering.Strict);

        // you can transform object's properties when return them
        yield return City.ToUpperInvariant();

        yield return PostalCode;
    }
}
```

### SingleValueObject

Inherit from `SingleValueObject` to boost performance when ValueObject has only one property:

```cs
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
```

### Custom EqualityComparer

When using `ByValue()` to compare collections, elements will be compared using default `EqualityComparer`. Sometimes this is not acceptable.

Here is `AddressBook` which implements value object semantic:

```cs
public class AddressBook : ReadOnlyCollection<MultilineAddress>
{
    public AddressBook(IList<MultilineAddress> list)
        : base(list)
    {
    }

    public override bool Equals(object obj)
    {
        if (obj is null)
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != GetType())
            return false;

        var other = obj as AddressBook;
        var thisByValue = this.ByValue(); // not strict ordering
        var otherByValue = other.ByValue();
        return thisByValue.Equals(otherByValue);
    }

    public override int GetHashCode()
    {
        return Items.Count;
    }
}
```

But you need address book which will not treat address with city "Foo" equal to address with city "fOO". Possible solution is to use custom `EqualityComparer` for `MultilineAddress` in derived `EnhancedAddressBook`:

```cs
public class EnhancedAddressBook : AddressBook
{
    public EnhancedAddressBook(IList<MultilineAddress> list)
        : base(list)
    {
    }

    public override bool Equals(object obj)
    {
        if (obj is null)
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != GetType())
            return false;

        var other = obj as EnhancedAddressBook;
        var thisByValue = this.ByValue(x => x.UseComparer(EnhancedAddressComparer.Instance));
        var otherByValue = other.ByValue(x => x.UseComparer(EnhancedAddressComparer.Instance));
        return thisByValue.Equals(otherByValue);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    private class EnhancedAddressComparer : IEqualityComparer<MultilineAddress>
    {
        public static EnhancedAddressComparer Instance => new EnhancedAddressComparer();

        public bool Equals(MultilineAddress x, MultilineAddress y)
        {
            if (ReferenceEquals(x, y))
                return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                return false;

            return x.AddressLines.ByValue(Ordering.Strict).Equals(y.AddressLines.ByValue(Ordering.Strict))
                // do not ignore case of chars
                && x.City == y.City
                && x.PostalCode == y.PostalCode;
        }

        public int GetHashCode(MultilineAddress obj)
        {
            return 1;
        }
    }
}

```

More examples could be found in [tests](https://github.com/sm-g/ByValue/tree/master/test/ByValue.Tests/Samples).
