using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace DefaultNamespace
{
    [BurstCompile]
    public partial struct SpawnerSystem : ISystem
    {
        // [BurstCompile]
        // public void OnCreate(ref SystemState state)
        // {
        // }
        //
        // [BurstCompile]
        // public void OnUpdate(ref SystemState state)
        // {
        //     foreach (RefRW<Spawner> spawner in SystemAPI.Query<RefRW<Spawner>>())
        //     {
        //         ProcessSpawner(ref state, spawner);
        //     }
        // }
        //
        // private void ProcessSpawner(ref SystemState state, RefRW<Spawner> spawner)
        // {
        //     if (spawner.ValueRO.NextSpawnTime < SystemAPI.Time.ElapsedTime)
        //     {
        //         Entity newEntity = state.EntityManager.Instantiate(spawner.ValueRO.Prefab);
        //         state.EntityManager.SetComponentData(newEntity, LocalTransform.FromPosition(spawner.ValueRO.SpawnPosition));
        //         spawner.ValueRW.NextSpawnTime = (float)SystemAPI.Time.ElapsedTime + spawner.ValueRO.SpawnRate;
        //     }
        // }
        //
        // [BurstCompile]
        // public void OnDestroy(ref SystemState state)
        // {
        // }
        public void OnCreate(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer.ParallelWriter ecb = GetEntityCommandBuffer(ref state);
            new ProcessSpawnerJob
            {
                ElapsedTime = SystemAPI.Time.ElapsedTime,
                Ecb = ecb
            }.ScheduleParallel();
        }

        private EntityCommandBuffer.ParallelWriter GetEntityCommandBuffer(ref SystemState state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
            return ecb.AsParallelWriter();
        }

        public void OnDestroy(ref SystemState state)
        {
        }
    }

    [BurstCompile]
    public partial struct ProcessSpawnerJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter Ecb;
        public double ElapsedTime;

        private void Execute([ChunkIndexInQuery] int chunkIndex, ref Spawner spawner)
        {
            if (spawner.NextSpawnTime < ElapsedTime)
            {
                Entity newEntity = Ecb.Instantiate(chunkIndex, spawner.Prefab);
                Ecb.SetComponent(chunkIndex, newEntity, LocalTransform.FromPosition(spawner.SpawnPosition));
                spawner.NextSpawnTime = (float)ElapsedTime + spawner.SpawnRate;
            }
        }
    }
}