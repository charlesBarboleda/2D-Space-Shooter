

public interface IObjective
{
    string Description { get; }
    bool IsCompleted { get; }
    int Reward { get; }
    void Initialize();
    void UpdateObjective();
}
