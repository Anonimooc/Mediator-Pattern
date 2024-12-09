using System;
using System.Collections.Generic;
using System.Linq;

// Класс User: представляет пользователя чата
public class User
{
    public string Name { get; private set; }
    private ChatMediator _mediator;
    private List<string> _messageHistory;

    public User(string name)
    {
        Name = name;
        _messageHistory = new List<string>();
    }

    // Устанавливаем медиатор
    public void SetMediator(ChatMediator mediator)
    {
        _mediator = mediator;
    }

    // Метод для отправки сообщения
    public void SendMessage(string to, string message)
    {
        _mediator.SendMessage(this, to, message);
    }

    // Метод для получения сообщения
    public void ReceiveMessage(string from, string message)
    {
        string fullMessage = $"{from}: {message}";
        _messageHistory.Add(fullMessage);
        Console.WriteLine($"{Name} получил сообщение от {from}: {message}");
    }

    // Метод для отображения истории сообщений
    public void ShowMessageHistory()
    {
        Console.WriteLine($"История сообщений пользователя {Name}:");
        foreach (var msg in _messageHistory)
        {
            Console.WriteLine(msg);
        }
    }
}

// Класс ChatMediator: посредник, управляющий обменом сообщениями
public class ChatMediator
{
    private List<User> _users;

    public ChatMediator()
    {
        _users = new List<User>();
    }

    // Добавление пользователя в чат
    public void AddUser(User user)
    {
        _users.Add(user);
        user.SetMediator(this);
    }

    // Удаление пользователя из чата
    public void RemoveUser(User user)
    {
        _users.Remove(user);
    }

    // Отправка сообщения от одного пользователя другому
    public void SendMessage(User from, string to, string message)
    {
        var recipient = _users.FirstOrDefault(u => u.Name == to);
        if (recipient != null)
        {
            recipient.ReceiveMessage(from.Name, message);
        }
        else
        {
            Console.WriteLine($"Пользователь {to} не найден в чате.");
        }
    }

    // Отображение всех пользователей в чате
    public void ShowUsers()
    {
        Console.WriteLine("Пользователи в чате:");
        foreach (var user in _users)
        {
            Console.WriteLine(user.Name);
        }
    }
}

// Программа для демонстрации работы системы обмена сообщениями
class Program
{
    static void Main()
    {
        // Создаем медиатор
        ChatMediator mediator = new ChatMediator();

        // Создаем пользователей
        User alice = new User("Alice");
        User bob = new User("Bob");
        User charlie = new User("Charlie");

        // Добавляем пользователей в чат
        mediator.AddUser(alice);
        mediator.AddUser(bob);
        mediator.AddUser(charlie);

        // Отправляем сообщения
        alice.SendMessage("Bob", "Привет, Боб!");
        bob.SendMessage("Alice", "Привет, Алиса!");
        charlie.SendMessage("Alice", "Привет, Алиса, как дела?");

        // Печатаем историю сообщений для каждого пользователя
        alice.ShowMessageHistory();
        bob.ShowMessageHistory();
        charlie.ShowMessageHistory();

        // Печатаем список пользователей
        mediator.ShowUsers();

        // Удаляем пользователя из чата
        mediator.RemoveUser(charlie);
        Console.WriteLine("\nПосле удаления Чарли из чата:\n");
        mediator.ShowUsers();
    }
}
