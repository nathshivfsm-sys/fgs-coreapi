using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Foundation.Api;

/// <summary>
/// Shared helpers for catalog CRUD controllers.
/// </summary>
public abstract class CatalogCrudControllerBase(IMediator mediator) : FgsApiControllerBase(mediator);
