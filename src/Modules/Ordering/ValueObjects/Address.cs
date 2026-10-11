using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.ValueObjects;

public  record Address
{
    public string FirstName { get; init; }=string.Empty;
    public string LastName { get; init; }=string.Empty;

    public string EmailAddress { get; init; }=string.Empty;

    public string AddressLine { get; init; }=string.Empty;

    public string Country { get; init; }=string.Empty; 

    public string State { get; init; }=string.Empty;

    public string ZipCode { get; init; }=string.Empty;

    protected Address() { }
    private Address(string firstName, string lastName, string emailAddress, string addressLine, string country, string state, string zipCode)
    {
        FirstName = firstName;
        LastName = lastName;
        EmailAddress = emailAddress;
        AddressLine = addressLine;
        Country = country;
        State = state;
        ZipCode = zipCode;
    }

    public static Address Of(string firstName, string lastName, string emailAddress, string addressLine, string country, string state, string zipCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(emailAddress, nameof(emailAddress));
        ArgumentException.ThrowIfNullOrWhiteSpace(addressLine, nameof(addressLine));
        return new Address(firstName, lastName, emailAddress, addressLine, country, state, zipCode);
    }
}

