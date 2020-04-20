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
    }
}
