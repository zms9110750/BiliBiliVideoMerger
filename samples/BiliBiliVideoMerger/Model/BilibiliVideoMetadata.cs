

using System.ComponentModel.DataAnnotations;
using System.IO;
using Windows.ApplicationModel.Contacts;

namespace BiliBiliVideoMerger.Model;

/// <summary>
/// 哔哩哔哩视频缓存文件元数据
/// </summary>
/// <param name="MediaType">媒体类型</param>
/// <param name="HasDashAudio">是否包含DASH音频流</param>
/// <param name="IsCompleted">是否已完成缓存</param>
/// <param name="TotalBytes">视频总字节数</param>
/// <param name="DownloadedBytes">已下载字节数</param>
/// <param name="Title">视频标题</param>
/// <param name="TypeTag">视频类型标签</param>
/// <param name="Cover">封面图片URL</param>
/// <param name="VideoQuality">视频质量标识</param>
/// <param name="PreferedVideoQuality">首选视频质量</param>
/// <param name="GuessedTotalBytes">预估总字节数（通常为0）</param>
/// <param name="TotalTimeMilli">视频总时长（毫秒）</param>
/// <param name="DanmakuCount">弹幕数量</param>
/// <param name="TimeUpdateStamp">更新时间戳</param>
/// <param name="TimeCreateStamp">创建时间戳</param>
/// <param name="CanPlayInAdvance">是否可预加载播放</param>
/// <param name="InterruptTransformTempFile">是否为中断转码临时文件</param>
/// <param name="QualityPithyDescription">视频质量简明描述</param>
/// <param name="QualitySuperscript">质量上标（通常为空）</param>
/// <param name="CacheVersionCode">缓存版本号</param>
/// <param name="PreferredAudioQuality">首选音频质量</param>
/// <param name="AudioQuality">音频质量</param>
/// <param name="Avid">视频AID（av号）</param>
/// <param name="Spid">特殊ID（通常为0）</param>
/// <param name="SeasionId">剧集ID（通常为0）</param>
/// <param name="Bvid">视频BVID（bv号）</param>
/// <param name="OwnerId">UP主用户ID</param>
/// <param name="OwnerName">UP主用户名</param>
/// <param name="OwnerAvatar">UP主头像URL</param>
/// <param name="PageData">分页数据信息</param>
public record BilibiliVideoMetadata(
    int MediaType,
    bool HasDashAudio,
    bool IsCompleted,
    long TotalBytes,
    long DownloadedBytes,
    string Title,
    string TypeTag,
    string Cover,
    int VideoQuality,
    int PreferedVideoQuality,
    long GuessedTotalBytes,
    long TotalTimeMilli,
    int DanmakuCount,
    long TimeUpdateStamp,
    long TimeCreateStamp,
    bool CanPlayInAdvance,
    bool InterruptTransformTempFile,
    string QualityPithyDescription,
    string QualitySuperscript,
    int CacheVersionCode,
    int PreferredAudioQuality,
    int AudioQuality,
    long Avid,
    int Spid,
    int SeasionId,
    string Bvid,
    long OwnerId,
    string OwnerName,
    string OwnerAvatar,
    PageData PageData
) : IComparable<BilibiliVideoMetadata>
{
    /// <summary>
    /// 实体路径
    /// </summary>
    public string? EntryPath { get; set; }
    /// <summary>
    /// 视频路径
    /// </summary>
    public string? VideoPath { get; set; }
    /// <summary>
    /// 音频路径
    /// </summary>
    public string? AudioPath { get; set; }
    /// <summary>
    /// 时长
    /// </summary>
    public TimeSpan TotalTime => TimeSpan.FromMilliseconds(TotalTimeMilli);
    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime TimeUpdate => DateTimeOffset.FromUnixTimeMilliseconds(TimeUpdateStamp).LocalDateTime;
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime TimeCreate => DateTimeOffset.FromUnixTimeMilliseconds(TimeCreateStamp).LocalDateTime;

    public int CompareTo(BilibiliVideoMetadata? other)
    {
        if (Bvid != other?.Bvid)
        {
            return 0;
        }
        return PageData.Page.CompareTo(other.PageData.Page);
    }
    public void FindVideoPath(string path)
    {
        var m4sFiles = Directory.GetFiles(path, "*.m4s", SearchOption.AllDirectories);
        EntryPath = path;
        VideoPath = m4sFiles.FirstOrDefault(f => Path.GetFileName(f) == "video.m4s");
        AudioPath = m4sFiles.FirstOrDefault(f => Path.GetFileName(f) == "audio.m4s");
    } 
}
