using Microsoft.AspNetCore.Mvc.Filters;

namespace Usuarios.API.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class SkipUserFilterAttribute : Attribute, IFilterMetadata { }