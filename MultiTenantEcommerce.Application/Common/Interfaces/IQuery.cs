﻿using MediatR;

namespace MultiTenantEcommerce.Application.Common.Interfaces;
public interface IQuery<TResponse> : IRequest<TResponse> { }

public interface IQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>{ }