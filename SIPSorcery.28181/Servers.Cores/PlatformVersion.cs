using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIPSorcery.GB28181.Servers.Cores
{
    /// <summary>
    /// 平台版本
    /// </summary>
    public enum PlatformVersion : byte
    {
        /// <summary>
        /// 国标(2011)
        /// </summary>
        VERSION_2011 = 1,
        /// <summary>
        /// 国标(2014)
        /// </summary>
        VERSION_2014 = 2,
        /// <summary>
        /// 国标(2016)
        /// </summary>
        VERSION_2016 = 3
    }

    /// <summary>
    /// 平台类型
    /// </summary>
    public enum PlatformType : byte
    {
        /// <summary>
        /// 上级平台
        /// </summary>
        UpPlatform = 1,
        /// <summary>
        /// 下级平台
        /// </summary>
        DownPlatform = 2
    }

    /// <summary>
    /// 云台控制命令
    /// </summary>
    public enum PTZCommand : int
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,
        /// <summary>
        /// 上
        /// </summary>
        [Description("上")]
        Up = 1,
        /// <summary>
        /// 左上
        /// </summary>
        [Description("左上")]
        UpLeft = 2,
        /// <summary>
        /// 右下
        /// </summary>
        [Description("右上")]
        UpRight = 3,
        /// <summary>
        /// 下
        /// </summary>
        [Description("下")]
        Down = 4,
        /// <summary>
        /// 左下
        /// </summary>
        [Description("左下")]
        DownLeft = 5,
        /// <summary>
        /// 右下
        /// </summary>
        [Description("右下")]
        DownRight = 6,
        /// <summary>
        /// 左
        /// </summary>
        [Description("左")]
        Left = 7,
        /// <summary>
        /// 右
        /// </summary>
        [Description("右")]
        Right = 8,
        /// <summary>
        /// 聚焦+
        /// </summary>
        [Description("聚焦+")]
        Focus1 = 9,
        /// <summary>
        /// 聚焦-
        /// </summary>
        [Description("聚焦-")]
        Focus2 = 10,
        /// <summary>
        /// 变倍+
        /// </summary>
        [Description("变倍+")]
        Zoom1 = 11,
        /// <summary>
        /// 变倍-
        /// </summary>
        [Description("变倍-")]
        Zoom2 = 12,
        /// <summary>
        /// 光圈开
        /// </summary>
        [Description("光圈Open")]
        Iris1 = 13,
        /// <summary>
        /// 光圈关
        /// </summary>
        [Description("光圈Close")]
        Iris2 = 14
    }

    /// <summary>
    /// SIP服务状态
    /// </summary>
    public enum SipServiceStatus:byte
    {
        /// <summary>
        /// 等待
        /// </summary>
        Wait = 1,
        /// <summary>
        /// 初始化完成
        /// </summary>
        Complete = 2
    }
}
