﻿using MediatR;
using Shared.Application.Auth;
using System.Text.Json.Serialization;

namespace Shared.Application;

public record RequestBase<TValue> : IRequest<TValue>
{
    [JsonIgnore]
    [SwaggerIgnore]
    public ISessionInfo SessionInfo { get; internal set; }
}
