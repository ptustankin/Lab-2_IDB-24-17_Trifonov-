public interface INewsObserver
{
    void Update(string articleTitle);
}

public interface INewsPublisher
{
    void Subscribe(INewsObserver observer);
    void Unsubscribe(INewsObserver observer);
    void Notify(string articleTitle);
}


public class NewsPublisher : INewsPublisher
{
    private readonly List<INewsObserver> _observers = [];
    private readonly List<string> _publishedArticles = [];

    public IReadOnlyList<string> PublishedArticles => _publishedArticles;

    public void Subscribe(INewsObserver observer)
    {
        if (!_observers.Contains(observer))
            _observers.Add(observer);
    }

    public void Unsubscribe(INewsObserver observer) => _observers.Remove(observer);

    public void Notify(string articleTitle)
    {
        foreach (var observer in _observers)
            observer.Update(articleTitle);
    }

    public void Publish(string articleTitle)
    {
        _publishedArticles.Add(articleTitle);
        Console.WriteLine($"\n[NewsPublisher] Опубликована новость: \"{articleTitle}\"");
        Notify(articleTitle);
    }
}


public class EmailSubscriber : INewsObserver
{
    private readonly string _email;
    public EmailSubscriber(string email) => _email = email;

    public void Update(string articleTitle)
        => Console.WriteLine($"\t[Email -> {_email}] Новая новость: \"{articleTitle}\"");
}

public class SmsSubscriber : INewsObserver
{
    private readonly string _phone;
    public SmsSubscriber(string phone) => _phone = phone;
            
    public void Update(string articleTitle)
        => Console.WriteLine($"\t[SMS -> {_phone}] Новая новасть: \"{articleTitle}\"");
}

public class PushSubscriber : INewsObserver
{
    private readonly string _deviceId;
    public PushSubscriber(string deviceId) => _deviceId = deviceId;

    public void Update(string articleTitle)
        => Console.WriteLine($"\t[Push -> {_deviceId}] Новая новасть: \"{articleTitle}\"");
}
