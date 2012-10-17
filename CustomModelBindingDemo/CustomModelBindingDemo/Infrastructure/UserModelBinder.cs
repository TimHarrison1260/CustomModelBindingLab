using System.Collections.Generic;
using System.Web.Mvc;
using CustomModelBindingDemo.Models;

namespace CustomModelBindingDemo.Infrastructure
{
    public class UserModelBinder : IModelBinder {
    private readonly IModelBinder binder;

    public UserModelBinder(IModelBinder binder) {
        this.binder = binder;
    }

    public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    {
        var user = (User)binder.BindModel(controllerContext, bindingContext);
        AddRoles(user, controllerContext);
        return user;
    }

    private static void AddRoles(User user, ControllerContext controllerContext) {
        foreach (var role in GetRoles(controllerContext)) {
            user.AddRole(role);
        }
    }

    private static IEnumerable<Role> GetRoles(ControllerContext controllerContext) {
        var roles = controllerContext.HttpContext.Request["roles"];
        if (roles == null) yield break;
        foreach (var role in roles.Split(',')) {
            yield return new Role { RoleName = role };
        }
    }
}
}