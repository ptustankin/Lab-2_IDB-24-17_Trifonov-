using OOP_Lab1;

Console.WriteLine("-------------- ЗАДАНИЕ 1 --------------");

var a = new ComplexNumber(1.5, 2);
var b = new ComplexNumber(3, -1);
var zero = new ComplexNumber(0, 0);
var real = new ComplexNumber(5, 0);
var imag = new ComplexNumber(0, 4);

Console.WriteLine("Примеры:");
Console.WriteLine($"a = {a}");
Console.WriteLine($"b = {b}");
Console.WriteLine($"real = {real}");
Console.WriteLine($"imag = {imag}");
Console.WriteLine($"zero = {zero}");

Console.WriteLine("\nВычисления:");
Console.WriteLine($"a + b = {a + b}");
Console.WriteLine($"a - b = {a - b}");
Console.WriteLine($"a * b = {a * b}");
Console.WriteLine($"a / b = {a / b}");

Console.WriteLine("\nСравнения:");
Console.WriteLine($"a == b <-> {a} == {b} <-> {a == b}");


// задание 2
Console.WriteLine("\n\n-------------- ЗАДАНИЕ 2 --------------");

var publisher = new NewsPublisher();

var emailSub = new EmailSubscriber("fbi@usa.com");
var smsSub = new SmsSubscriber("+7-900-123-45-67");
var pushSub = new PushSubscriber("Smartphone Vivo");

publisher.Subscribe(emailSub);
publisher.Subscribe(smsSub);
publisher.Subscribe(pushSub);

publisher.Publish("ПТУ Станкин топ вуз планеты");


// задание 3
Console.WriteLine("\n\n-------------- ЗАДАНИЕ 3 --------------\n");

var root = new TreeNode<string>("Корень");
var branch1 = new TreeNode<string>("Ветка 1");
var branch2 = new TreeNode<string>("Ветка 2");
var leaf1 = new TreeNode<string>("Лист 1");
var leaf2 = new TreeNode<string>("Лист 2");
var leaf3 = new TreeNode<string>("Лист 3");

branch1.AddChild(leaf1);
branch1.AddChild(leaf2);
branch2.AddChild(leaf3);
root.AddChild(branch1);
root.AddChild(branch2);

root.PrintAll();