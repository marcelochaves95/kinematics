using Kinematics.Utils;

namespace KinematicsWeb.Scenes;

public interface IDemoScene
{
    string Name        { get; }
    string Icon        { get; }
    string Description { get; }

    void Initialize(KinematicsController ctrl);

    /// <summary>Called every frame BEFORE ctrl.Update() — use for per-frame setup (e.g. gravity on chains).</summary>
    void PreUpdate(KinematicsController ctrl, double elapsed) { }
}
