using YourBrand.CustomerService.Application.CustomerService.Dtos;
using YourBrand.CustomerService.Application.Users;
using YourBrand.CustomerService.Domain.ValueObjects;

namespace YourBrand.CustomerService.Application;

public static partial class Mappings
{
    public static IssueDto ToDto(this Issue issue) => new(issue.Id, issue.Items.Select(x => x.ToDto()), issue.Created, issue.CreatedBy?.ToDto(), issue.LastModified, issue.LastModifiedBy?.ToDto());

    public static IssueItemDto ToDto(this IssueItem issueItem) => new(issueItem.Id, issueItem.ItemId, issueItem.Quantity, issueItem.Data, issueItem.Created, issueItem.CreatedBy?.ToDto(), issueItem.LastModified, issueItem.LastModifiedBy?.ToDto());

    public static UserDto ToDto(this User user) => new(user.Id, user.Name);

    public static UserInfoDto ToDto2(this User user) => new(user.Id, user.Name);
}
