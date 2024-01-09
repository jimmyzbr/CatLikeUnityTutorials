namespace EasyKit.ECS
{
    public class SystemsManager
    {
        private const int Factions = 2;
        
        public GameManager GameManager { get; private set; }
        
        public EntitySystem EntitySystem { get; private set; }
        
        public SystemsManager(GameManager gameManager)
        {
            GameManager = gameManager;
        }

        public void Setup()
        {
            EntitySystem = new EntitySystem(Factions, this);
            EntitySystem.DoAwake();
        }

        public void DoStart()
        {
            EntitySystem.DoStart();
        }
        
        public void DoStop()
        {
        }

        public void DoUpdate(float deltaTime)
        {
            
        }

        public void DoDestroy()
        {
            EntitySystem?.DoDestroy();
        }
        
    }
}