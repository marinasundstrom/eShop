using System;
using YourBrand.Portal.Domain.Entities;
using YourBrand.Portal.Domain.Enums;

namespace YourBrand.Portal.Domain.Specifications;

public class TodosWithStatus : BaseSpecification<Todo>
{
    public TodosWithStatus(TodoStatus status)
    {
        Criteria = todo => todo.Status == status;
    }
}

