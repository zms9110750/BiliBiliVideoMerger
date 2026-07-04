
namespace BiliBiliVideoMerger.Model;


/// <summary>
/// 哔哩哔哩视频分页数据
/// </summary>
/// <param name="Cid">分页CID</param>
/// <param name="Page">分页序号（从1开始）</param>
/// <param name="From">视频来源（如"vupload"表示用户上传）</param>
/// <param name="Part">分片标题</param>
/// <param name="Link">链接（通常为空）</param>
/// <param name="Vid">视频ID（通常为空）</param>
/// <param name="HasAlias">是否有别名</param>
/// <param name="Tid">分区ID</param>
/// <param name="Width">视频宽度（像素）</param>
/// <param name="Height">视频高度（像素）</param>
/// <param name="Rotate">旋转角度（0表示正常）</param>
/// <param name="DownloadTitle">下载标题（显示给用户）</param>
/// <param name="DownloadSubtitle">下载副标题（显示给用户）</param>
public record PageData(
    long Cid,
    int Page,
    string From,
    string Part,
    string Link,
    string Vid,
    bool HasAlias,
    int Tid,
    int Width,
    int Height,
    int Rotate,
    string DownloadTitle,
    string DownloadSubtitle
)
{
    public string Part { get; set; } = Part;
}