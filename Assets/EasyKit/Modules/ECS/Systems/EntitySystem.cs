using System.Collections.Generic;

namespace EasyKit.ECS
{
    public class EntitySystem : BaseSystem
    {
        private const int INIT_ENTITY_COUNT = 32;
        
        protected Dictionary<int, BaseEntity> mEntities;

        public int EntityCount { get; private set; }
        
        private  int mFactions;

        private SystemsManager mSystemsManager;
        
        public delegate void EntityRegister(BaseEntity entity);
        
        public event EntityRegister OnRegisterEntity;
        
        public event EntityRegister OnUnRegisterEntity;
        
        
        public EntitySystem(int factions, SystemsManager systemsManager)
        {
            mFactions = factions;
            mSystemsManager = systemsManager;
            mEntities = new Dictionary<int, BaseEntity>(INIT_ENTITY_COUNT);
            EntityCount = 0;
        }
        
        public override void DoAwake()
        {
        }

        public override void DoStart()
        {
        }

        public override void DoDestroy()
        {
            mSystemsManager = null;
        }
        
        public override void DoUpdate(float deltaTime)
        {
        }

        public virtual void RegisterEntity(BaseEntity entity)
        {
            if( entity == null)
                return;
            
            if (mEntities.ContainsKey(entity.entityId))
            {
                UnityEngine.Debug.LogError(typeof(BaseEntity).ToString() + " already registered.");
            }
            else
            {
                mEntities.Add(entity.entityId,entity);
                OnRegisterEntity?.Invoke(entity);
            }
            EntityCount = mEntities.Count;
        }

        public virtual void UnRegisterEntity(BaseEntity entity)
        {
            if(entity == null)
                return;

            if (!mEntities.ContainsKey(entity.entityId))
            {
                return;
            }
            else
            {
                if (entity.IsRunning)
                    entity.DoStop();
                
                OnUnRegisterEntity?.Invoke(entity);
                mEntities.Remove(entity.entityId);
            }

            EntityCount = mEntities.Count;
        }
        
        public void DestroyEntity(BaseEntity entity)
        {
            UnRegisterEntity(entity);
            if(entity != null)
                entity.DoDestroy();
        }
        

        public void SwitchFaction(BaseEntity entity, int faction)
        {
            if (faction < 0 || faction >= mFactions || entity.factionID == faction) 
                return;
            entity.ChangeFaction(faction);
        }

    }
}