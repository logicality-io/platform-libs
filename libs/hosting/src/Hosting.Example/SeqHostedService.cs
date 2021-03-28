﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using Microsoft.Extensions.Logging;

namespace Logicality.Extensions.Hosting.Example
{
    public class SeqHostedService : DockerHostedService
    {
        private readonly HostedServiceContext _context;
        public const int Port = 5010;
        private const int ContainerPort = 80;

        public SeqHostedService(
            HostedServiceContext context,
            ILogger<DockerHostedService> logger)
            : base(logger)
        {
            _context = context;
        }

        protected override string ContainerName => "extensions-seq";

        public Uri SinkUri { get; private set; }

        protected override IContainerService CreateContainerService()
            => new Builder()
                .UseContainer()
                .WithName(ContainerName)
                .UseImage("datalust/seq:5.1.3000")
                .ReuseIfExists()
                .ExposePort(Port, ContainerPort)
                .WithEnvironment("ACCEPT_EULA=Y")
                .WaitForPort($"{ContainerPort}/tcp", 5000, "127.0.0.1")
                .Build();

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await base.StartAsync(cancellationToken);
            SinkUri = new Uri($"http://localhost:{Port}");
            _context.Seq = this;
        }
    }
}