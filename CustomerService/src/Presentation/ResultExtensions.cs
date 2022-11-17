using Microsoft.AspNetCore.Mvc;
using YourBrand.CustomerService.Domain;

namespace YourBrand.CustomerService.Presentation;

public static class ResultExtensions
{
    public static ActionResult Handle(this Result result, Func<ActionResult> onSuccess, Func<Error, ActionResult> onError) => result.IsFailure ? onError(result) : onSuccess();

    public static ActionResult Handle<T>(this Result<T> result, Func<T, ActionResult> onSuccess, Func<Error, ActionResult> onError) => result.IsFailure ? onError(result) : onSuccess(result);
}
