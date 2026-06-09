using Kinematics.Collision;
using Kinematics.Dynamics;
using Kinematics.Math;
using Kinematics.Utils;

namespace KinematicsWeb.Scenes;

public class SoftBodyScene : IDemoScene
{
    public string Name        => "Soft Body";
    public string Icon        => "🟦";
    public string Description => "A spring-based box falls and bounces. Click to push it!";

    public void Initialize(KinematicsController ctrl)
    {
        // Box — CCW in screen-space: top-left → bottom-left → bottom-right → top-right
        var boxShape = new Shape();
        boxShape.Begin(true);
        boxShape.Add(new Vector2(-1f, -1f));
        boxShape.Add(new Vector2(-1f,  1f));
        boxShape.Add(new Vector2( 1f,  1f));
        boxShape.Add(new Vector2( 1f, -1f));
        boxShape.End();

        var box = new SpringBody(boxShape, 1f,
            edgeSpringK: 200f, edgeSpringDamp: 15f,
            shapeSpringK: 300f, shapeSpringDamp: 20f);
        box.Position = new Vector2(0f, 3f);  // half-size 1 → top at Y=2, clear of top wall
        box.Gravity  = new Vector2(0f, 9.8f);
        ctrl.Add(box);
    }
}
