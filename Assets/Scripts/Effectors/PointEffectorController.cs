using UnityEngine;

namespace Effectors
{
    public class PointEffectorController : EffectorController
    {
        [SerializeField] private CircleCollider2D circle;
        [SerializeField] private ParticleSystem innerPs;
        [SerializeField] private PointEffector2D pointEffector;
        public override void SetRadius(float radius)
        {
            circle.radius = radius;
            var shape = ps.shape;
            shape.radius = radius;
        }
        
        public override void SetForce(float force) => pointEffector.forceMagnitude = force;
        public override void SetColor(Color color)
        {
            var main = ps.main;
            var innerMain = innerPs.main;
            main.startColor = innerMain.startColor = color;
        }

        public void SetInnerRadius(float radius, float radiusThickness)
        {
            var shape = innerPs.shape;
            shape.radius = radius;
            shape.radiusThickness = radiusThickness;
        }

        public void SetInnerSpeed(float speed)
        {
            var main = innerPs.main;
            main.startSpeed = speed;
        }
    
        public void SetInnerLifeTime(float time)
        {
            var main = innerPs.main;
            main.startLifetime = time;
        }

        public void SetInnerColorOverLifeTime(ParticleSystem.MinMaxGradient color)
        {
            var colorOverLifeTime = innerPs.colorOverLifetime;
            colorOverLifeTime.color = color;
        }
    }
}