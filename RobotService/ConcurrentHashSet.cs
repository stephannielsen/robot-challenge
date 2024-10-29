using System.Collections.Concurrent;

namespace RobotService;

#if false
/**
 * Thread-safe HashSet implementation with minimal functionality as needed here.
 * Performance optimization via segmentation, so that items are stored in different HashSets to 
 * reduce collisions and allow for parallel and thread-safe access.
 */
public class ConcurrentHashSet<T>
{
    private readonly int _segmentCount;
    private readonly HashSet<T>[] _segments;
    private readonly int _maxSegments = 128;

    public ConcurrentHashSet(int total)
    {
        _segmentCount = CalculateSegmentCount(total);
        _segments = Enumerable.Range(0, _segmentCount).Select(_ => new HashSet<T>()).ToArray();
    }
    private int CalculateSegmentCount(int total)
    {
        // Use at least 1 segment and max 128, as with too many segments there isn't much gain anymore
        // as there are not so many collisions anyway and each segment requires additional overhead
        return Math.Max(1, Math.Min(total, _maxSegments));
    }

    private int GetSegmentIndex(T item)
    {
        // return the segment based on items hashcode, 0 index as default
        if (item is null) return 0;
        return Math.Abs(item.GetHashCode() % _segmentCount);
    }

    public bool Add(T item)
    {
        int segmentIndex = GetSegmentIndex(item);
        lock (_segments[segmentIndex])
        {
            return _segments[segmentIndex].Add(item);
        }
    }

    public int Count
    {
        get
        {
            return _segments.Sum(s => s.Count);
        }
    }
}
#else
/**
 * Abstraction on top of ConcurrentDictionary to avoid the complexity of Dictionary handling when only a 
 * ConcurrentHashSet is needed.
 * 
 * ConcurrentDictionary provides a thread-safe implementation with segmentation which can be used as ConcurrentHashSet.
 */
public class ConcurrentHashSet<T>(int total) : ConcurrentDictionary<T, byte>(Math.Max(1, Math.Min(total, 128)), 128)
    where T : notnull
{
    const byte DummyByte = byte.MinValue;
    public bool Add(T item) => TryAdd(item, DummyByte);
}
#endif