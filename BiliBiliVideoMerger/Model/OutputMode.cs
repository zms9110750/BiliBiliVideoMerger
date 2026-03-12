using System;
using System.Collections.Generic;
using System.Text;

namespace BiliBiliVideoMerger.Model;

/// <summary>
/// 输出模式
/// </summary>
[Flags]
public enum OutputMode
{
    /// <summary>
    /// 无特殊选项
    /// </summary>
    None = 0,

    /// <summary>
    /// 多p视频用序号代替副标题
    /// </summary>
    UseNumberForMulti = 1 << 0,  // 1

    /// <summary>
    /// 单p视频也加副标题或序号
    /// </summary>
    AlwaysAddMulti = 1 << 1,     // 2

    /// <summary>
    /// 跳过已经存在的文件（无法预检测是否完整）
    /// </summary>
    SkipExisting = 1 << 2,   // 4
    /// <summary>
    /// 多p视频置于独立文件夹
    /// </summary>
    MultiInSeparateFolder = 1 << 3, // 多p视频置于独立文件夹
    /// <summary>
    /// 设置输出文件的创建时间和修改时间与配置相同
    /// </summary>
    PreserveFileTime = 1 << 4  // 16
}