using Orders.Models;

namespace Orders.Tests;

// Тесты для класса Orders.Models.Order
// Всего тестов в этом файле: 6 из таблицы (№1–№6) + 1 готовый пример.
// Уровни: Б — №1,2,3,4,5 (5 тестов); С — №6 (1 тест); П — нет.
public class OrderTests
{
    // Готовый пример из задания (не входит в нумерацию таблицы)
    [Fact]
    public void Total_TwoItems_ReturnsSum()
    {
        var order = new Order();
        order.Add("Книга", 300m, 2);
        order.Add("Ручка", 50m, 1);

        var total = order.Total();

        Assert.Equal(650m, total);
    }

    // №1 | Уровень: Б | Тип: Fact
    [Fact]
    public void Total_EmptyOrder_ReturnsZero()
    {
        var order = new Order();

        var total = order.Total();

        Assert.Equal(0m, total);
    }

    // №2 | Уровень: Б | Тип: Theory (3 кейса: null, "", "   ")
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Add_InvalidName_Throws(string? name)
    {
        var order = new Order();

        Assert.Throws<ArgumentException>(() => order.Add(name!, 100m, 1));
    }

    // №3 | Уровень: Б | Тип: Theory (2 кейса: 0, -1)
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Add_NonPositivePrice_Throws(decimal price)
    {
        var order = new Order();

        Assert.Throws<ArgumentOutOfRangeException>(() => order.Add("Товар", price, 1));
    }

    // №4 | Уровень: Б | Тип: Theory (3 кейса: 0, -1, 101)
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(101)]
    public void Add_InvalidQuantity_Throws(int quantity)
    {
        var order = new Order();

        Assert.Throws<ArgumentOutOfRangeException>(() => order.Add("Товар", 100m, quantity));
    }

    // №5 | Уровень: Б | Тип: Fact
    [Fact]
    public void Add_ValidItem_AddsToItems()
    {
        var order = new Order();

        order.Add("Книга", 300m, 2);

        var item = Assert.Single(order.Items);
        Assert.Equal("Книга", item.Name);
        Assert.Equal(300m, item.Price);
        Assert.Equal(2, item.Quantity);
    }

    // №6 | Уровень: С | Тип: Fact
    [Fact]
    public void Constructor_WithItems_SameTotalAsAdd()
    {
        var items = new List<OrderItem>
        {
            new("Книга", 300m, 2),
            new("Ручка", 50m, 1)
        };

        var fromConstructor = new Order(items);

        var viaAdd = new Order();
        viaAdd.Add("Книга", 300m, 2);
        viaAdd.Add("Ручка", 50m, 1);

        var constructorTotal = fromConstructor.Total();
        var addTotal = viaAdd.Total();

        Assert.Equal(addTotal, constructorTotal);
        Assert.Equal(650m, constructorTotal);
    }
}