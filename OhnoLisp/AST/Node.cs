namespace OhnoLisp.AST;

public interface INode
{
    IReadOnlyList<INode> Children { get; }

    bool IsLeaf { get; }
}

public sealed record Command(IReadOnlyList<INode> Children, string CommandName) : INode
{
    public bool IsLeaf => false;
}

public sealed record Integer(int Value) : INode
{
    private static readonly IReadOnlyList<INode> emptyList = new List<INode>();

    public IReadOnlyList<INode> Children => emptyList;

    public bool IsLeaf => true;
}

public sealed record Variable(string Name) : INode
{
    private static readonly IReadOnlyList<INode> emptyList = new List<INode>();

    public IReadOnlyList<INode> Children => emptyList;

    public bool IsLeaf => true;
}