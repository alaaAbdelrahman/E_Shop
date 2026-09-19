using Basket.Basket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Basket.Data.Converter;

public class ShoppingCartConverter : JsonConverter<ShoppingCart>
{
    public override ShoppingCart? Read(
     ref Utf8JsonReader reader,
     Type typeToConvert,
     JsonSerializerOptions options)
    {
        var jsonDocument = JsonDocument.ParseValue(ref reader);

        var rootElement = jsonDocument.RootElement;

        var id = rootElement
            .GetProperty("id")
            .GetGuid();

        var userName = rootElement
            .GetProperty("userName")
            .GetString()!;

        var itemsElement = rootElement
            .GetProperty("items");

        var cart = ShoppingCart.Create(
            id,
            userName);

        foreach (var itemElement in itemsElement.EnumerateArray())
        {
            var item = JsonSerializer.Deserialize<ShoppingCartItem>(
                itemElement.GetRawText(),
                options);

            if (item is not null)
            {
                cart.AddItem(
                    item.ProductId,
                    item.Quantity,
                    item.Color,
                    item.ProductName,
                    item.Price);
            }
        }

        return cart;
    }

    public override void Write(Utf8JsonWriter writer, ShoppingCart value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WriteString("id",value.Id.ToString());
        writer.WriteString("userName", value.UserName.ToString());

        writer.WritePropertyName("items");
        JsonSerializer.Serialize(writer, value.Items, options);

        writer.WriteEndObject();

    }
}
