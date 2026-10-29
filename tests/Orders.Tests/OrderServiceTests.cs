using Moq;
using Orders.Abstractions;
using Orders.Models;
using Orders.Services;

namespace Orders.Tests;

// Тесты для класса Orders.Services.OrderService (используют Moq)
// Всего тестов: 9 из таблицы (№10–№18).
// Уровни: Б — №13 (1 тест); С — №10,11,12,14,16,17,18 (7 тестов); П — №15 (1 тест).
// Тестов с Moq + Verify: 5 (№10, №11, №12, №14, №15, №19, №20 — см. также OrderCreatedHandlerTests).
public class OrderServiceTests
{
    private readonly Mock<IOrderRepository> _repo = new();
    private readonly Mock<IMessagePublisher> _pub = new();
    private readonly OrderService _sut;

    public OrderServiceTests()
    {
        _repo.Setup(r => r.SaveAsync(It.IsAny<Order>()))
            .Returns(Task.CompletedTask);

        _pub.Setup(p => p.PublishAsync(It.IsAny<OrderCreated>()))
            .Returns(Task.CompletedTask);

        _sut = new OrderService(_repo.Object, _pub.Object, new DiscountCalculator());
    }

    private static NewOrder Cmd() =>
        new(new List<OrderItem> { new("Книга", 300m, 2) });

    // №10 | Уровень: С | Тип: Fact + Moq (Verify SaveAsync — Times.Once)
    [Fact]
    public async Task CreateAsync_ValidOrder_SavesOrder()
    {
        await _sut.CreateAsync(Cmd());

        _repo.Verify(r => r.SaveAsync(It.IsAny<Order>()), Times.Once);
    }

    // №11 | Уровень: С | Тип: Fact + Moq (Verify PublishAsync — Times.Once + совпадение Id)
    [Fact]
    public async Task CreateAsync_ValidOrder_PublishesEventWithSameId()
    {
        var id = await _sut.CreateAsync(Cmd());

        _pub.Verify(
            p => p.PublishAsync(It.Is<OrderCreated>(e => e.OrderId == id)),
            Times.Once);
    }

    // №12 | Уровень: С | Тип: Fact + Moq (Times.Never — проверка отсутствия публикации)
    [Fact]
    public async Task CreateAsync_SaveFails_DoesNotPublish()
    {
        _repo.Setup(r => r.SaveAsync(It.IsAny<Order>()))
            .ThrowsAsync(new TimeoutException());

        await Assert.ThrowsAsync<TimeoutException>(() => _sut.CreateAsync(Cmd()));
        _pub.Verify(p => p.PublishAsync(It.IsAny<OrderCreated>()), Times.Never);
    }

    // №13 | Уровень: Б | Тип: Fact (без моков)
    [Fact]
    public async Task CreateAsync_NullCommand_Throws()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.CreateAsync(null!));
    }

    // №14 | Уровень: С | Тип: Fact + Moq (Times.Never — проверка, что SaveAsync не вызван)
    [Fact]
    public async Task CreateAsync_EmptyItems_ThrowsAndDoesNotSave()
    {
        var cmd = new NewOrder(Array.Empty<OrderItem>());

        await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateAsync(cmd));
        _repo.Verify(r => r.SaveAsync(It.IsAny<Order>()), Times.Never);
    }

    // №15 | Уровень: П | Тип: Fact + Moq (ThrowsAsync — проверка распространения исключения)
    [Fact]
    public async Task CreateAsync_PublishFails_ExceptionPropagates()
    {
        _pub.Setup(p => p.PublishAsync(It.IsAny<OrderCreated>()))
            .ThrowsAsync(new InvalidOperationException("publish failed"));

        await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.CreateAsync(Cmd()));
    }

    // №16 | Уровень: С | Тип: Fact + stub (ReturnsAsync — настройка ответа репозитория)
    [Fact]
    public async Task GetPayableAsync_TotalBelowThreshold_NoDiscount()
    {
        var order = new Order();
        order.Add("Товар", 500m, 1);

        _repo.Setup(r => r.FindAsync(order.Id)).ReturnsAsync(order);

        var payable = await _sut.GetPayableAsync(order.Id);

        Assert.Equal(500m, payable);
    }

    // №17 | Уровень: С | Тип: Fact + stub (ReturnsAsync)
    [Fact]
    public async Task GetPayableAsync_TotalAboveThreshold_AppliesDiscount()
    {
        var order = new Order();
        order.Add("Товар", 1500m, 1);

        _repo.Setup(r => r.FindAsync(order.Id)).ReturnsAsync(order);

        var payable = await _sut.GetPayableAsync(order.Id);

        Assert.Equal(1350m, payable);
    }

    // №18 | Уровень: С | Тип: Fact + stub (ReturnsAsync null)
    [Fact]
    public async Task GetPayableAsync_OrderNotFound_Throws()
    {
        var id = Guid.NewGuid();
        _repo.Setup(r => r.FindAsync(id)).ReturnsAsync((Order?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _sut.GetPayableAsync(id));
    }
}