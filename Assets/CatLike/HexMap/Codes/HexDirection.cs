namespace CatLike.HexMap.Codes
{
    /// <summary>
    /// 六边形地图方向
    /// </summary>
    public enum HexDirection : sbyte
    {
        NE = 0,  //东北
        E,       //东
        SE,      //东南
        SW,      //西南
        W,       //西
        NW,      //西北
    }


    public static class HexDirectionExtensions
    {
        /// <summary>
        /// 获取dir的相反方向
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static HexDirection Opposite(this HexDirection dir)
        {
            var iDir = (int)dir;
            return (HexDirection)(iDir < 3 ? iDir + 3 : iDir - 3);
        }

        /// <summary>
        /// 获取上一个方向
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static HexDirection Pre(this HexDirection dir)
        {
            if (dir == HexDirection.NE)
                return HexDirection.NW;
            return dir - 1;
        }
    
        /// <summary>
        /// 获取下一个方向
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static HexDirection Next(this HexDirection dir)
        {
            if (dir == HexDirection.NW)
                return HexDirection.NE;
            return dir + 1;
        }
    }
}