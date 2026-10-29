using Orders.Validation;

namespace Orders.Tests;

// Тесты для класса Orders.Validation.QuantityValidator
// Всего тестов: 1 из таблицы (№7).
// Уровни: Б — №7 (1 тест); С — нет; П — нет.
public class QuantityValidatorTests
{
    // №7 | Уровень: Б | Тип: Theory (5 кейсов: -1, 0, 1, 100, 101)
    [Theory]
    [InlineData(-1, false)]
    [InlineData(0, false)]
    [InlineData(1, true)]
    [InlineData(100, true)]
    [InlineData(101, false)]
    public void IsValidQty_ChecksBounds(int qty, bool expected)
    {
        var actual = QuantityValidator.IsValidQty(qty);

        Assert.Equal(expected, actual);
    }
}