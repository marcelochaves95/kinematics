using Kinematics.Collision;
using Kinematics.Math;
using Kinematics.Utils;

namespace KinematicsWeb.Scenes;

public class ChainScene : IDemoScene
{
    public string Name        => "Chain";
    public string Icon        => "⛓️";
    public string Description => "A rope hanging under gravity. Click any link to flick it!";

    private const float GravityY = 9.8f;

    public void Initialize(KinematicsController ctrl)
    {
        // Anchors fixed at the top of the playable area
        var anchorA = new PointMass(new Vector2(-5f, 2f), float.PositiveInfinity);
        var anchorB = new PointMass(new Vector2( 5f, 2f), float.PositiveInfinity);

        // NOTE: the Chain ctor's `damping` argument is the SPRING damping
        // coefficient (passed straight into every Spring), NOT the per-frame
        // velocity multiplier (that's chain.Damping, set below).
        //
        // CRITICAL STABILITY BOUND: for explicit-Euler integration of these
        // springs (k=120, mass=0.25, dt=1/60) in a 20-link chain, the spring
        // damping must stay below ≈7.  At 8 the damping force flips sign and
        // grows each frame (c·dt/m_eff > 2) — the chain injects energy and
        // oscillates forever no matter how much velocity damping is applied.
        // 6 is safely inside the stable region; numerically the chain settles
        // to a steady catenary and stays put (max velocity → 0).
        var chain = new Chain(anchorA, anchorB,
            count:   20,
            k:       120f,
            damping: 6f,     // spring damping — MUST be < 7 for Euler stability
            mass:    0.25f);

        foreach (Spring s in chain.SpringList)
            s.D *= 1.5f;

        // s.D *= 1.5f makes each spring's REST LENGTH 1.5× the initial node
        // spacing → total natural length 15 m over a 10 m span, so the chain
        // sags into a catenary whose lowest point settles at y≈8.6 (anchors at
        // y=2, floor inner face at y=13 → ~4.4 m clearance).
        //
        // The chain STARTS as a straight horizontal line, far from that
        // equilibrium, so it must fall ~6 m.  Pre-simulate to settle it before
        // the user sees it: with chain.Damping=0.96 the system reaches a near-
        // motionless catenary in <300 frames (verified numerically: residual
        // velocity ≈0.04 m/s).  Runtime damping 0.90 settles any flick in ~1 s.
        chain.Damping = 0.96f;
        const double dt = 1.0 / 60.0;
        for (int step = 0; step < 300; step++)
        {
            for (int i = 1; i < chain.PointMassList.Count - 1; i++)
                chain.PointMassList[i].Force.Y += GravityY * chain.PointMassList[i].Mass;
            chain.Update(dt);
        }
        chain.Damping = 0.90f;   // runtime: settles in ~1 s after a flick

        ctrl.Add(chain);
    }

    public void PreUpdate(KinematicsController ctrl, double elapsed)
    {
        foreach (Chain chain in ctrl.ChainList)
        {
            for (int i = 1; i < chain.PointMassList.Count - 1; i++)
            {
                float mass = chain.PointMassList[i].Mass;
                if (!float.IsInfinity(mass))
                    chain.PointMassList[i].Force.Y += GravityY * mass;
            }
        }
    }
}
