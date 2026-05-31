using Microsoft.AspNetCore.Mvc;

namespace Fgs.Foundation.Api;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class FgsVersionedRouteAttribute(string template)
    : RouteAttribute($"api/v{{version:apiVersion}}/{template.Trim().Trim('/')}");
