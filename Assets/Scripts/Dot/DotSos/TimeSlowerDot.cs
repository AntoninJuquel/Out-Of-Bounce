using UnityEngine;

namespace Dot.DotSos
{
    [CreateAssetMenu(fileName = "New time slower dot", menuName = "Dots/Time slower", order = 0)]
    public class TimeSlowerDot : DotSo
    {
        public override void Destroy(GameObject dot)
        {
            Time.timeScale = .5f / (level + 1);
            base.Destroy(dot);
        }
    }
}