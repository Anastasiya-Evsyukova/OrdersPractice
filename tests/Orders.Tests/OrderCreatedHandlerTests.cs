using Moq;
using Orders.Abstractions;
using Orders.Handlers;
using Orders.Models;

namespace Orders.Tests;

// Тесты для класса Orders.Handlers.OrderCreatedHandler (используют Moq)
// Всего тестов: 3 из таблицы (№19–№21).
// Уровни: Б — №21 (1 тест); С — №19, №20 (2 теста); П — нет.
// Тестов с Moq + Verify: 2 (№19, №20).
public class OrderCreatedHandlerTests
{
    private readonly Mock<INotificationSender> _sender = new();
    private readonly OrderCreatedHandler _sut;

    public OrderCreatedHandlerTests()
    {
        _sender.Setup(s => s.SendAsync(It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        _sut = new OrderCreatedHandler(_sender.Object);
    }

    // №19 | Уровень: С | Тип: Fact + Moq (Verify SendAsync — Times.Once + проверка текста)
    [Fact]
    public async Task HandleAsync_ValidMessage_SendsNotification()
    {
        var id = Guid.NewGuid();

        await _sut.HandleAsync(new OrderCreated(id));

        _sender.Verify(
            s => s.SendAsync(It.Is<string>(text => text.Contains(id.ToString()))),
            Times.Once);
    }

    // №20 | Уровень: С | Тип: Fact + Moq (Times.Never — проверка, что уведомление не отправлено)
    [Fact]
    public async Task HandleAsync_EmptyOrderId_Throws()
    {
        var ex = await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.HandleAsync(new OrderCreated(Guid.Empty)));

        Assert.Contains("OrderId", ex.Message);
        _sender.Verify(s => s.SendAsync(It.IsAny<string>()), Times.Never);
    }

    // №21 | Уровень: Б | Тип: Fact (без моков)
    [Fact]
    public async Task HandleAsync_NullMessage_Throws()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _sut.HandleAsync(null!));
    }
}