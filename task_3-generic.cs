public class TreeNode<T>
{
    public T Value { get; }

    public List<TreeNode<T>> Children { get; } = [];

    public TreeNode(T value) => Value = value;

    public void AddChild(TreeNode<T> child) => Children.Add(child);

    public void PrintAll(int depth = 0)
    {
        Console.WriteLine($"{new string('\t', depth * 2)}{Value}");
        foreach (var child in Children)
            child.PrintAll(depth + 1);
    }
}