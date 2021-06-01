using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Upgrade.UpgradeSos
{
    [CreateAssetMenu(fileName = "New missile upgrade", menuName = "Upgrades/Missile", order = 0)]
    public class MissileUpgrade : UpgradeSo
    {
        [SerializeField] private LayerMask whatIsDot;
        [SerializeField] private Sprite missileSprite;
        [SerializeField] private GameObject projectileGo;
        [SerializeField] private float missileSpeed, range, rotationSpeed;
        [SerializeField] private Vector2 timeToShoot;
        private List<Missile> missiles = new List<Missile>();
        private float timer, tts;

        public override void OnEnableUpgrade(GameObject gameObject)
        {
            timer = 0f;
            missiles = new List<Missile>();
            tts = Random.Range(timeToShoot.x, timeToShoot.y);
        }

        public override void UpdateUpgrade(GameObject gameObject)
        {
            timer += Time.deltaTime;
            if (timer < tts) return;
            Shoot(gameObject.transform.position);
            tts = Random.Range(timeToShoot.x, timeToShoot.y);
            timer = 0;
        }

        public override void FixedUpdateUpgrade(GameObject gameObject)
        {
            foreach (var missile in missiles)
            {
                missile.Process();
            }
        }

        private void Shoot(Vector3 startPoint)
        {
            var missile = Instantiate(projectileGo, startPoint, quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)));
            var missileSr = missile.GetComponent<SpriteRenderer>();
            var missileTrail = missile.GetComponent<TrailRenderer>();
            var replaced = missiles.Any(m => m.TryReplaceGo(missile));
            if (!replaced) missiles.Add(new Missile(missile, range, rotationSpeed, missileSpeed, whatIsDot));
            missileSr.sprite = missileSprite;
            missileTrail.startColor = color;
            Destroy(missile, 15f);
        }
    }

    public class Missile
    {
        private LayerMask whatIsDot;
        private GameObject gameObject, target;
        private float radius, rotationSpeed, speed, timer;
        private State state;
        private Rigidbody2D rb;

        private enum State
        {
            Roaming,
            Targeting,
            Attacking
        }

        public Missile(GameObject missile, float range, float rotSpeed, float missileSpeed, LayerMask dotLayer)
        {
            timer = 0f;
            state = State.Roaming;
            gameObject = missile;
            radius = range;
            rotationSpeed = rotSpeed;
            whatIsDot = dotLayer;
            rb = missile.GetComponent<Rigidbody2D>();
            speed = missileSpeed;
        }

        public void Process()
        {
            if (!gameObject) return;
            timer += Time.deltaTime;
            switch (state)
            {
                case State.Roaming:
                    if (timer >= 2f) state = State.Targeting;
                    rb.velocity = gameObject.transform.right.normalized * (speed / 2f * 100f * Time.fixedDeltaTime);
                    break;
                case State.Targeting:
                    target = Physics2D.OverlapCircle(gameObject.transform.position, radius, whatIsDot)?.gameObject;
                    if (target && target.activeSelf) state = State.Attacking;
                    break;
                case State.Attacking:
                    if (!target) return;
                    var direction = target.transform.position - gameObject.transform.position;
                    direction.Normalize();
                    gameObject.transform.right = Vector2.Lerp(gameObject.transform.right, direction, Time.fixedDeltaTime * rotationSpeed);
                    rb.velocity = gameObject.transform.right.normalized * (speed * 100f * Time.fixedDeltaTime);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public bool TryReplaceGo(GameObject newMissile)
        {
            if (gameObject) return false;
            gameObject = newMissile;
            rb = gameObject.GetComponent<Rigidbody2D>();
            target = null;
            timer = 0f;
            state = State.Roaming;
            return true;
        }
    }
}