using System.Linq;
using Dot;
using UnityEngine;

namespace Upgrade.UpgradeSos
{
    [CreateAssetMenu(fileName = "New tether upgrade", menuName = "Upgrades/Tether", order = 0)]
    public class TetherUpgrade : UpgradeSo
    {
        [SerializeField] private LayerMask ballLayer, dotLayer;
        [SerializeField] private float radius;
        [SerializeField] private GameObject tetherPrefab, tether;
        private LineRenderer _lr;

        public override void OnEnableUpgrade(GameObject gameObject)
        {
            tether = Instantiate(tetherPrefab);
            _lr = tether.GetComponent<LineRenderer>();
        }

        public override void OnDisableUpgrade(GameObject gameObject)
        {
            Destroy(tether);
        }

        public override void UpdateUpgrade(GameObject gameObject)
        {
            var position = gameObject.transform.position;
            var balls = Physics2D.OverlapCircleAll(position, radius, ballLayer).Where(c => c.gameObject != gameObject).ToArray();
            _lr.positionCount = balls.Length * 2;
            for (var i = 0; i < balls.Length; i++)
            {
                var ballPosition = balls[i].transform.position;

                var index = i * 2;

                _lr.SetPosition(index, position);
                _lr.SetPosition(index + 1, ballPosition);

                Debug.DrawLine(position, ballPosition);
                foreach (var dot in Physics2D.LinecastAll(position, ballPosition, dotLayer))
                {
                    dot.collider.gameObject.GetComponent<DotController>().Destroy();
                }
            }
        }
    }
}