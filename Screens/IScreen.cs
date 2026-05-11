namespace KungFuClone.Screens;

public interface IScreen
{
    void Update(float dt);
    void Draw();
    void OnEnter();
    void OnExit();
}
