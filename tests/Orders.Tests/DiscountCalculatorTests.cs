using Orders.Services;

namespace Orders.Tests;

// Тесты для класса Orders.Services.DiscountCalculator
// Всего тестов: 2 из таблицы (№8–№9).
// Уровни: Б — №8, №9 (2 теста); С — нет; П — нет.
public class DiscountCalculatorTests
{
    private readonly DiscountCalculator _calculator = new();

    // №8 | Уровень: Б | Тип: Theory (4 кейса: 0, 999.99, 1000, 2000)
    // ВНИМАНИЕ: кейс 1000 падает, пока в DiscountCalculator стоит ">" вместо ">=".
    [Theory]
    [InlineData(0, 0)]
    [InlineData(999.99, 0)]
    [InlineData(1000, 100)]
    [InlineData(2000, 200)]
    public void Calculate_ReturnsExpected(decimal total, decimal expected)
    {
        var discount = _calculator.Calculate(total);

        Assert.Equal(expected, discount);
    }

    // №9 | Уровень: Б | Тип: Fact
    [Fact]
    public void Calculate_NegativeTotal_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _calculator.Calculate(-1m));
    }
}