# ByValue

[![Build status](https://ci.appveyor.com/api/projects/status/k6nmr1mdixf7xho6/branch/master?svg=true)](https://ci.appveyor.com/project/sm-g/byvalue/branch/master) [![Build Status](https://travis-ci.org/sm-g/ByValue.svg?branch=master)](https://travis-ci.org/sm-g/ByValue)

This library helps to create ValueObjects with properly implemented equality behavior.

1. Provides base `ValueObject` class.
2. Gives extension `ByValue()` for comparing collections with semantic of ValueObject.

## Example

```cs
public class Address : ValueObject
{
    public Address(string[] addressLines, string city, string postalCode)
    {
        AddressLines = addressLines ?? throw new ArgumentNullException(nameof(addressLines));
        City = city ?? throw new ArgumentNullException(nameof(city));
        PostalCode = postalCode ?? throw new ArgumentNullException(nameof(postalCode));
    }

    public IReadOnlyCollection<string> AddressLines { get; }
    public string City { get; }
    public string PostalCode { get; }

    // here you should return values, which will be used in Equals and GetHashCode
    protected override IEnumerable<object> Reflect()
    {
        yield return AddressLines.ByValue(Ordering.NotStrict);
        yield return City.ToUpperInvariant();
        yield return PostalCode;
    }
}
```

## roadmap for version 1.0

- [ ] Add documentation
- [ ] Add usage sample
- [ ] Publish on nuget
- [ ] Add `HashSet` support
- [ ] Add support of custom `EqualityComparer` for collection elements