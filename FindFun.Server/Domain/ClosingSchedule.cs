namespace FindFun.Server.Domain;

using System;
using System.Collections.Generic;
using System.Linq;

public class ClosingSchedule
{
    public int Id { get; private set; }

    private readonly List<ClosingScheduleEntry> _entries = [];
    public IReadOnlyCollection<ClosingScheduleEntry> Entries => _entries;

    public int ParkId { get; set; }
    public Park Park { get; private set; } = null!;

    protected ClosingSchedule() { }

    public ClosingSchedule(IEnumerable<ClosingScheduleEntry> entries)
    {
        _entries = entries.ToList();
    }

    public void AddEntry(ClosingScheduleEntry entry)
    {
        if (!_entries.Contains(entry))
            _entries.Add(entry);
    }

    public void SetPark(Park park)
    {
        Park = park;
        ParkId = park.Id;
    }
}

public record ClosingScheduleEntry(string Day, string OpensAt, string ClosesAt, bool IsClosed);
