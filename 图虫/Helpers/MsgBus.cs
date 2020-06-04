using System;

namespace 图虫.Helpers
{
    public class MsgBus
    {
        private static readonly Lazy<MsgBus>
               lazy = new Lazy<MsgBus>(() => new MsgBus());
        public static MsgBus Instance => lazy.Value;

        /// <summary>
        /// 查看摄影师主页时使用的 ID
        /// </summary>
        public string PhotographerID { get; set; }

        public string  UserId { get; set; }

        /// <summary>
        /// 用于标识跳转到"我的喜欢"时是否使用缓存的数据，默认不刷新，当从BlankPage点击按钮进入时才会刷新
        /// </summary>
        public bool ShouldRefreshMyLike { get; set; } = false;
        public bool ShouldRefreshMyFollowing { get; set; } = false;
        public bool ShouldRefreshMyFans { get; set; } = false;
    }
}
