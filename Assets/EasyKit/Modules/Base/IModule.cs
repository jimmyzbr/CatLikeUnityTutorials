namespace EasyKit
{
    public interface IModule
    {
        /// <summary>
        /// 创建模块
        /// </summary>
        void OnCreate(System.Object createParam);

        /// <summary>
        /// 模块启动
        /// </summary>
        void OnStart();

        /// <summary>
        /// 模块退出
        /// </summary>
        void Shutdown();

        /// <summary>
        /// 轮询模块
        /// </summary>
        void OnUpdate();

        /// <summary>
        /// GUI绘制
        /// </summary>
        void OnGUI();
    }
}