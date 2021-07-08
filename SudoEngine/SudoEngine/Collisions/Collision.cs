using SudoEngine.Core;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;
using Box2DSharp.Dynamics;
using System.Numerics;

namespace SudoEngine.Collisions
{
    public class Collision
    {
        public static void CreateWorld()
        {
            World world = new World(new Vector2(0, -10));

            BodyDef bodyDef = new BodyDef();
            bodyDef.BodyType = BodyType.DynamicBody;
            bodyDef.Position.Set(0, 4);
            Body body = world.CreateBody(bodyDef);

            PolygonShape dynamicBox = new PolygonShape();
            dynamicBox.SetAsBox(1, 1);

            FixtureDef fixtureDef = new FixtureDef();
            fixtureDef.Shape = dynamicBox;
            fixtureDef.Density = 1;
            fixtureDef.Friction = 0.3f;

            body.CreateFixture(fixtureDef);

            float timeStep = 1.0f / 60.0f;

            int velocityIterations = 6;
            int positionIterations = 2;

            BodyDef chainBodyDef = new BodyDef();
            chainBodyDef.BodyType = BodyType.StaticBody;
            chainBodyDef.Position.Set(0, 0);
            Body body2 = world.CreateBody(chainBodyDef); 
            ChainShape chainShape = new ChainShape();
            Vector2[] vertices = new Vector2[]
            {
                new Vector2(32, 32), new Vector2(224, 32), new Vector2(384, 160),
                new Vector2(384, 256), new Vector2(480, 256), new Vector2(480, 288),
                new Vector2(256, 288), new Vector2(256, 256), new Vector2(160, 256),
                new Vector2(160, 160), new Vector2(128, 160), new Vector2(128, 256),
                new Vector2(32, 256)
            };
            chainShape.CreateLoop(vertices);
            body2.CreateFixture(chainShape, 1);

            for (int i = 0; i < 60; i++)
            {
                world.Step(timeStep, velocityIterations, positionIterations);
                Vector2 position = body.GetPosition();
                float angle = body.GetAngle();
                Log.Info($"X : {position.X}  Y : {position.Y}  Angle : {angle}");
            }
        }
    }
}
