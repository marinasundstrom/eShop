using System;
using System.Data;
using System.Security.Claims;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using YourBrand.Portal.Services;

namespace YourBrand.Portal.AppBar;


public interface IAppBarTrayService
{
    IReadOnlyCollection<AppBarTrayItem> Items { get; }

    void AddItem(AppBarTrayItem item);

    void RemoveItem(string id);

    event EventHandler ItemAdded;

    event EventHandler ItemRemoved;
}

public sealed class AppBarTrayItem 
{
    private string? name;

    public AppBarTrayItem(string id, string name, string icon, Action onClick)
    {
        Id = id;
        Name = name;
        Icon = icon;
        OnClick = onClick;
    }

    public AppBarTrayItem(string id, string name, Type componentType)
    {
        Id = id;
        Name = name;
        ComponentType = componentType;
    }

    public AppBarTrayItem(string id, Func<string> nameFunc, string icon, Action onClick)
    {
        Id = id;
        NameFunc = nameFunc;
        Icon = icon;
        OnClick = onClick;
    }

    public AppBarTrayItem(string id, Func<string> nameFunc, Type componentType)
    {
        Id = id;
        NameFunc = nameFunc;
        ComponentType = componentType;
    }

    public string Id { get; set; } = null!;

    public string Name
    {
        get => name ?? NameFunc?.Invoke() ?? throw new Exception();
        set => name = value;
    }

    public Func<string>? NameFunc { get; set; }

    public string Icon { get; set; }

    public Type? ComponentType { get; }

    public Action? OnClick { get; }
    
    public bool IsVisible { get; set; } = true;

    public bool RequiresAuthorization { get; set; }

    public IEnumerable<string>? Roles { get; set; }

    public string? Policy { get; set; }
}

public sealed class AppBarTrayService : IAppBarTrayService
{
    private IDictionary<string, AppBarTrayItem> _items = new Dictionary<string, AppBarTrayItem>();

    public IReadOnlyCollection<AppBarTrayItem> Items => _items.Select(x => x.Value).ToList();

    public event EventHandler ItemAdded = default!;

    public event EventHandler ItemRemoved = default!;

    void IAppBarTrayService.AddItem(AppBarTrayItem item)
    {
        _items.Add(item.Id, item);
        ItemAdded?.Invoke(this, EventArgs.Empty);
    }

    void IAppBarTrayService.RemoveItem(string id)
    {
        _items.Remove(id);
        ItemRemoved?.Invoke(this, EventArgs.Empty);
    }
}