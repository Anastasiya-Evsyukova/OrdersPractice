using Orders.Models;
using Orders.Repositories;

namespace Orders.Tests;

// Тесты для класса Orders.Repositories.InMemoryOrderRepository (моки не нужны — используем fake-репозиторий)
// Всего тестов: 3 из таблицы (№22–№24).
// Уровни: Б — №22, №23, №24 (3 теста); С — нет; П — нет.
public class InMemoryOrderRepositoryTests
{
    // №22 | Уровень: Б | Тип: Fact (async)
    [Fact]
    public async Task SaveAsync_ThenFindAsync_ReturnsSameOrder()
    {
        var repo = new InMemoryOrderRepository();
        var order = new Order();
        order.Add("Книга", 300m, 2);

        await repo.SaveAsync(order);
        var found = await repo.FindAsync(order.Id);

        Assert.Same(order, found);
    }

    // №23 | Уровень: Б | Тип: Fact (async)
    [Fact]
    public async Task FindAsync_UnknownId_ReturnsNull()
    {
        var repo = new InMemoryOrderRepository();

        var found = await repo.FindAsync(Guid.NewGuid());

        Assert.Null(found);
    }

    // №24 | Уровень: Б | Тип: Fact (async)
    [Fact]
    public async Task SaveAsync_Null_Throws()
    {
        var repo = new InMemoryOrderRepository();

        await Assert.ThrowsAsync<ArgumentNullException>(
            () => repo.SaveAsync(null!));
    }
}