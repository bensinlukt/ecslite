using System;

namespace Leopotam.EcsLite {
    [Serializable]
    public sealed class SerializedEcsWorld {
        public short[] Entities { get; set; }
        public int EntitiesItemSize { get; set; }
        public int EntitiesCount { get; set; }
        public int[] RecycledEntities { get; set; }
        public int RecycledEntitiesCount { get; set; }
        public SerializedEcsPool[] Pools { get; set; }
        public short PoolsCount { get; set; }

        private SerializedEcsWorld() { }

        public SerializedEcsWorld(in EcsWorld world) {
            Entities = world.GetRawEntities();
            EntitiesItemSize = world.GetRawEntityItemSize();
            EntitiesCount = world.GetEntitiesCount();
            RecycledEntities = world.GetRecycledEntities();
            RecycledEntitiesCount = world.GetRecycledEntitiesCount();
            IEcsPool[] pools = null;
            _ = world.GetAllPools(ref pools);
            Pools = new SerializedEcsPool[pools.Length];
            for (var i = 0; i < pools.Length; i++) {
                Pools[i] = new SerializedEcsPool(pools[i]);
            }
            PoolsCount = (short)world.GetPoolsCount();
        }
    }

    [Serializable]
    public sealed class SerializedEcsPool {
        public string Type { get; set; }
        public short Id { get; set; }
        public object[] DenseItems { get; set; }
        public int[] SparseItems { get; set; }
        public int DenseItemsCount { get; set; }
        public int[] RecycledItems { get; set; }
        public int RecycledItemsCount { get; set; }

        private SerializedEcsPool() { }

        public SerializedEcsPool(in IEcsPool pool) {
            Type = pool.GetComponentType().FullName;
            Id = (short)pool.GetId();
            DenseItems = pool.GetDenseItems();
            SparseItems = pool.GetSparseItems();
            DenseItemsCount = pool.GetDenseItemCount();
            RecycledItems = pool.GetRecycledItems();
            RecycledItemsCount = pool.GetRecycledItemCount();
        }

        public IEcsPool ToPool(EcsWorld world) {
            var type = System.Type.GetType(Type);
            var pool = Activator.CreateInstance(type, new object[] { world, this });
            return pool as IEcsPool;
        }
    }
    
    [Serializable]
    public sealed class SerializedEcsEntity {
        public int Id { get; set; }
        public int Gen { get; set; }

        private SerializedEcsEntity() { }
        
        public SerializedEcsEntity(in EcsPackedEntity entity) {
            Id = entity.Id;
            Gen = entity.Gen;
        }
    }
}