using Conduit.Domain;
using Conduit.Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Conduit.Features.ArticleCategories
{
    [Route("articleCategories")]
    public class ArticleCategoriesController
    {
        private readonly IMediator _mediator;

        public ArticleCategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ArticleCategoriesEnvelope> Get([FromQuery] int CategoryId, [FromQuery] int ArticleId, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            return await _mediator.Send(new List.Query(CategoryId, ArticleId, limit, offset));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<ArticleCategory> Create([FromBody]Create.Command command)
        {
            return await _mediator.Send(command);
        }

        [HttpDelete("{ArticleCategoryId}")]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task Delete(int ArticleCategoryId)
        {
            await _mediator.Send(new Delete.Command(ArticleCategoryId));
        }
    }
}
