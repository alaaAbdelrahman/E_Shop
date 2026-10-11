using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.ValueObjects;

public record Payment
{
    public string CardName { get; init; }=string.Empty;
    public string CardNumber { get; init; }
    public string Expiration { get; init; }=string.Empty;

    public string CVV { get; init; }=string.Empty;

    public int PaymentMethod { get; init; }=default!;

    protected Payment() { }

    private Payment(string cardName, string cardNumber, string expiration, string cvv, int paymentMethod)
    {
        CardName = cardName;
        CardNumber = cardNumber;
        Expiration = expiration;
        CVV = cvv;
        PaymentMethod = paymentMethod;
    }

    public static Payment Of(string cardName, string cardNumber, string expiration, string cvv, int paymentMethod)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cardName, nameof(cardName));
        ArgumentException.ThrowIfNullOrWhiteSpace(cardNumber, nameof(cardNumber));
        ArgumentException.ThrowIfNullOrWhiteSpace(expiration, nameof(expiration));
        ArgumentException.ThrowIfNullOrWhiteSpace(cvv, nameof(cvv));
        return new Payment(cardName, cardNumber, expiration, cvv, paymentMethod);
    }
}
