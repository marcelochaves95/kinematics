using Kinematics.Collision;
using Kinematics.Dynamics;
using Kinematics.Math;
using Kinematics.Utils;

namespace KinematicsWeb.Scenes;

public class PressureBodyScene : IDemoScene
{
    public string Name        => "Pressure Body";
    public string Icon        => "🟣";
    public string Description => "A gas-inflated body bounces and deforms. Click to push it!";

    public void Initialize(KinematicsController ctrl)
    {
        // 12-gon balloon — CCW in screen-space (negate sin so points go right→up→left→down)
        const int   N      = 12;
        const float radius = 1.5f;
        var ballShape = new Shape();
        ballShape.Begin(true);
        for (int i = 0; i < N; i++)
        {
            float a = 2f * MathF.PI * i / N;
            ballShape.Add(new Vector2(radius * MathF.Cos(a), -radius * MathF.Sin(a)));
        }
        ballShape.End();

        var ball = new PressureBody(ballShape, 0.5f,
            gasPressure:   30f,   // reduced from 40 → less rocking energy on the floor
            edgeSpringK:  150f, edgeSpringDamp:  10f,
            shapeSpringK: 200f, shapeSpringDamp: 15f);
        ball.Position = new Vector2(0f, 4.5f);  // radius 1.5 → top at Y=3, clear of top wall
        ball.Gravity  = new Vector2(0f, 9.8f);
        ctrl.Add(ball);
    }

    // Linear drag applied each frame to all non-static PM velocities.
    // This dissipates the small rocking energy injected by gas pressure so the
    // ball settles instead of bouncing indefinitely.
    //
    // Stability: drag×dt/mass = 2×(1/60)/0.5 = 0.067.  The per-PM velocity
    // reduction is 1 − 0.067 = 0.933 per frame — well inside the stable range.
    public void PreUpdate(KinematicsController ctrl, double elapsed)
    {
        const float drag = 2f;   // N·s/m  (linear air resistance)
        foreach (Body body in ctrl.BodyList)
        {
            if (body.IsStatic) continue;
            foreach (PointMass pm in body.PointMassList)
            {
                pm.Force.X -= drag * pm.Velocity.X;
                pm.Force.Y -= drag * pm.Velocity.Y;
            }
        }
    }
}
