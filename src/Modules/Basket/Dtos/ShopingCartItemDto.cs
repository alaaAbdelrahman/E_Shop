namespace Basket.Dtos;

public  record ShopingCartItemDto
(
    Guid id,
    Guid ShoppingCartId,
    Guid ProductId,
    int Quantity,
    string Color,
    decimal Price ,
    string ProductName
 );