

using Canalot.Utils;
using FFMpegCore;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Windows.Controls.Primitives;

namespace BiliBiliVideoMerger.Model;

public class BiliBiliModel(BilibiliVideoMetadata metadata)
{
    /// <summary>
    /// 总大小
    /// </summary>
    [Display(Name = "大小", Order = 4)] public long TotalBytes => Collection.Sum(t => t.TotalBytes);
    /// <summary>
    /// 总时长
    /// </summary>
    [Display(Name = "时长", Order = 3)] public TimeSpan TotalTime => Collection.Select(t => t.TotalTime).Aggregate((sum, current) => sum + current);
    /// <summary>
    /// 视频BVID（bv号）
    /// </summary>
    [Display(Name = "bv", Order = 2)] public string Bvid => First.Bvid;
    /// <summary>
    /// 标题
    /// </summary>
    [Display(Name = "标题", Order = 1)] public string Title { get; set; } = metadata.Title;
    /// <summary>
    /// UP主用户名
    /// </summary>
    [Display(Name = "up", Order = 0)] public string OwnerName => First.OwnerName;
    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime TimeUpdate => Collection.Max(t => t.TimeUpdate);
    /// <summary>
    /// 创建时间
    /// </summary>
    [Display(Name = "下载时间", Order = 5)] public DateTime TimeCreate => Collection.Min(t => t.TimeCreate);
    /// <summary>
    /// 分p数量
    /// </summary>
    [Display(Name = "分p", Order = 5)] public int PageCount => Collection.Count;
    private readonly HashSet<BilibiliVideoMetadata> _completed = new();

    [Display(Name = "完成", Order = 6)]
    public int Complete => _completed.Count;
    /// <summary>
    /// 是单独的视频
    /// </summary>
    public bool IsSinge => PageCount == 1;
    /// <summary>
    /// 特征值
    /// </summary>
    public BilibiliVideoMetadata First => Collection.Min!;
    public bool Indeterminate => Selected.Count != 0 && Selected.Count != PageCount;
    public bool AllSelected
    {
        get => Selected.Count == PageCount;
        set
        {
            if (value)
            {
                foreach (var item in Collection)
                {
                    Selected.Add(item);
                }
            }
            else
            {
                Selected.Clear();
            }
        }
    }
    public HashSet<BilibiliVideoMetadata> Selected { get; } = new HashSet<BilibiliVideoMetadata>();
    SortedSet<BilibiliVideoMetadata> Collection { get; } = [metadata];
    public IReadOnlySet<BilibiliVideoMetadata> Pages => Collection;
    public void Add(BilibiliVideoMetadata bilibili)
    {
        if (bilibili.Bvid != First.Bvid)
        {
            throw new ArgumentException("bv号不一致。不是同一个视频。");
        }
        if (bilibili.EntryPath == First.EntryPath)
        {
            return;
        }
        Collection.Add(bilibili);
    }
    public void SelectedChange(BilibiliVideoMetadata bilibili)
    {
        if (!Selected.Add(bilibili))
        {
            Selected.Remove(bilibili);
        }
    }
    public void ClearCompleted()
    {
        _completed.Clear();
    }
    public async IAsyncEnumerable<int> OutPut(string outPath, OutputMode mode)
    {
        var selectedList = Selected.OrderBy(x => x).ToList();
        if (selectedList.Count == 0)
            yield return 0;

        ClearCompleted();
        // 确定最终输出目录
        string finalDir = outPath;
        Directory.CreateDirectory(outPath);
        if (mode.HasFlag(OutputMode.MultiInSeparateFolder) && selectedList.Count > 1)
        {
            finalDir = Path.Combine(outPath, Title);
            Directory.CreateDirectory(finalDir);
        }

        // 处理每个选中的P
        foreach (var item in selectedList)
        {
            // 生成文件名
            string fileName = Title;
            if (!IsSinge || mode.HasFlag(OutputMode.AlwaysAddMulti))
            {
                fileName += "_";
                fileName += mode.HasFlag(OutputMode.UseNumberForMulti) ? item.PageData.Page.ToString("D2") : item.PageData.Part;
            }
            fileName = fileName.ToSafeFileName();
            string outputFile = Path.Combine(finalDir, $"{fileName}.mp4");

            // 检查跳过已存在
            if (mode.HasFlag(OutputMode.SkipExisting) && File.Exists(outputFile))
            {
                continue;
            }

            // 合并输出
            await FFMpegArguments
                    .FromFileInput(item.VideoPath)
                    .AddFileInput(item.AudioPath)
                    .OutputToFile(outputFile, true, options => options
                        .WithCopyCodec())
                    .ProcessAsynchronously();
            _completed.Add(item);
            if (mode.HasFlag(OutputMode.PreserveFileTime))
            {
                File.SetCreationTime(outputFile, item.TimeCreate);
                File.SetLastWriteTime(outputFile, item.TimeUpdate);
            }
            yield return Complete;
        }
    }
}