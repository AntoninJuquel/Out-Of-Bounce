using UnityEngine;

namespace Effectors
{
    public class AreaEffectorController : EffectorController
    {
        [SerializeField] private BoxCollider2D box;
        [SerializeField] private ParticleSystem basePs, edgeL, edgeR;
        [SerializeField] private AreaEffector2D areaEffector;

        public override void SetRadius(float radius)
        {
            var shape = ps.shape;
            var baseShape = basePs.shape;
            shape.radius = baseShape.radius = radius;
            edgeL.transform.localPosition = Vector3.up * radius;
            edgeR.transform.localPosition = Vector3.up * -radius;
            box.size = new Vector2(20, radius * 2f);
        }

        public override void SetForce(float force) => areaEffector.forceMagnitude = force;

        public override void SetColor(Color color)
        {
            var main = ps.main;
            var baseMain = basePs.main;
            var edgeLMain = edgeL.main;
            var edgeRMain = edgeR.main;
            main.startColor = baseMain.startColor = edgeLMain.startColor = edgeRMain.startColor = color;
        }
    }
}