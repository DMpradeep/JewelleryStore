using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace JewelleryStore.Api.Controllers
{
    public abstract class BaseController: Controller
    {
        private readonly Lazy<IMediator> _mediatorLazy;

        public BaseController()
        {
            _mediatorLazy = new Lazy<IMediator>(() => HttpContext.RequestServices.GetService<IMediator>());
        }

        public IMediator Mediator => _mediatorLazy.Value;
    }
}
