﻿using Moq;
using Ocelot.Configuration;
using Ocelot.Configuration.Builder;
using Ocelot.LoadBalancer.LoadBalancers;
using Ocelot.ServiceDiscovery.Providers;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Ocelot.UnitTests.LoadBalancer
{
    public class LeastConnectionCreatorTests
    {
        private readonly LeastConnectionCreator _creator;
        private readonly Mock<IServiceDiscoveryProvider> _serviceProvider;
        private DownstreamReRoute _reRoute;
        private ILoadBalancer _loadBalancer;
        private string _typeName;

        public LeastConnectionCreatorTests()
        {
            _creator = new LeastConnectionCreator();
            _serviceProvider = new Mock<IServiceDiscoveryProvider>();
        }
        
        [Fact]
        public void should_return_expected_name()
        {
            var reRoute = new DownstreamReRouteBuilder()
                .WithServiceName("myService")
                .Build();

            this.Given(x => x.GivenAReRoute(reRoute))
                .When(x => x.WhenIGetTheLoadBalancer())
                .Then(x => x.ThenTheLoadBalancerIsReturned<LeastConnection>())
                .BDDfy();
        }
                
        [Fact]
        public void should_return_instance_of_expected_load_balancer_type()
        {
            this.When(x => x.WhenIGetTheLoadBalancerTypeName())
                .Then(x => x.ThenTheLoadBalancerTypeIs("LeastConnection"))
                .BDDfy();
        }

        private void GivenAReRoute(DownstreamReRoute reRoute)
        {
            _reRoute = reRoute;
        }

        private void WhenIGetTheLoadBalancer()
        {
            _loadBalancer = _creator.Create(_reRoute, _serviceProvider.Object);
        }
        
        private void WhenIGetTheLoadBalancerTypeName()
        {
            _typeName = _creator.Type;
        }

        private void ThenTheLoadBalancerIsReturned<T>()
            where T : ILoadBalancer
        {
            _loadBalancer.ShouldBeOfType<T>();
        }

        private void ThenTheLoadBalancerTypeIs(string type)
        {
            _typeName.ShouldBe(type);
        }
    }
}