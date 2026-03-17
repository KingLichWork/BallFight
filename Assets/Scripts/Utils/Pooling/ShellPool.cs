using UnityEngine;

namespace Utils.Pooling
{
    public class ShellPool : ObjectPool<ParticleSystem>
    {
        public ShellPool(ParticleSystem prefab, int initialSize, Transform holder) : base(prefab, initialSize, holder)
        {
        }
    }
}
