# Kinematics

A lightweight 2D soft-body physics engine for C#. Framework-agnostic — no external dependencies.

## How it works

Bodies are made of **PointMasses** connected by **Springs**. Each frame, spring forces and gravity are applied to the point masses, which are then integrated with semi-implicit Euler.

Three body types are available:

| Type | Description |
|------|-------------|
| `Body` | Rigid body (no internal springs) |
| `SpringBody` | Soft body with edge and shape springs |
| `PressureBody` | Soft body with internal gas pressure |

## Coordinate system

Screen-space: **Y increases downward**. Gravity points in the `+Y` direction.

## Usage

```csharp
var shape = new Shape();
shape.Begin(true);
shape.Add(new Vector2(-0.5f, -0.5f));
shape.Add(new Vector2( 0.5f, -0.5f));
shape.Add(new Vector2( 0.5f,  0.5f));
shape.Add(new Vector2(-0.5f,  0.5f));
shape.End();

var body = new SpringBody(shape, mass: 1f,
    edgeSpringK: 150f, edgeSpringDamp: 10f,
    shapeSpringK: 200f, shapeSpringDamp: 15f);

body.Position = new Vector2(0f, 0f);
body.Gravity  = new Vector2(0f, 9.8f);

var controller = new KinematicsController();
controller.Add(body);

// game loop
controller.Update(elapsed);
