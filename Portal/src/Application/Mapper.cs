using YourBrand.Portal.Features.Widgets;
using YourBrand.Portal.Todos.Dtos;
using YourBrand.Portal.Users;

namespace YourBrand.Portal;

public static class Mappings
{
    public static TodoDto ToDto(this Todo todo) => new TodoDto(todo.Id, todo.Title, todo.Description, (TodoStatusDto)todo.Status, todo.Assignee?.ToDto(), todo.EstimatedHours, todo.RemainingHours, todo.Created, todo.CreatedBy.ToDto(), todo.LastModified, todo.LastModifiedBy?.ToDto());

    public static UserDto ToDto(this User user) => new UserDto(user.Id, user.Name);

    public static UserInfoDto ToDto2(this User user) => new UserInfoDto(user.Id, user.Name);

    public static WidgetDto ToDto(this Widget widget) => new (widget.Id, widget.WidgetId, widget.WidgetAreaId, widget.UserId, widget?.Settings?.RootElement.ToString());
}
