namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Global master definition of application menus and navigation items available across the FSM platform.
/// </summary>
public class GloMenu
{
    public int Id { get; set; }

    public string MenuCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int? ParentMenuId { get; set; }

    public string MenuType { get; set; } = null!;

    public string? Route { get; set; }

    public string? Icon { get; set; }

    public short SortOrder { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedOn { get; set; }

    public GloMenu? ParentMenu { get; set; }

    public ICollection<GloMenu> ChildMenus { get; set; } = [];
}
